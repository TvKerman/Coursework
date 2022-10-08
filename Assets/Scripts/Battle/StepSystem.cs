using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


namespace TurnBasedBattleSystemFromRomchik
{
    public class StepSystem
    {
        private static List<Unit> unitList;
        private bool _isAnimationOn = false;

        private static int _currentUnitIndex = 0;
        private float _bonusDamage = 0.1f;
        private int _maxBonusDamage = 10;
        public StepSystem(List<Unit> _unitList)
        {
            unitList = _unitList;
            Refresh(ref unitList);
        }

        public bool isAnimationOn
        {
            get { return _isAnimationOn; }
            set { _isAnimationOn = value; }
        }

        public int CurrentUnitIndex
        {
            get { return _currentUnitIndex; }
            set
            {
                _currentUnitIndex = value;
                if (_currentUnitIndex >= unitList.Count)
                {
                    _currentUnitIndex = 0;
                }
            }
        }

        public Unit UnitInList
        {
            get
            {
                if (unitList[CurrentUnitIndex] != null)
                {
                    return unitList[CurrentUnitIndex];
                }
                else
                {
                    unitList.RemoveAt(CurrentUnitIndex);
                    if (CurrentUnitIndex < unitList.Count)
                    {
                        return unitList[CurrentUnitIndex];
                    }
                    else
                    {
                        CurrentUnitIndex = 0;
                        return unitList[CurrentUnitIndex];
                    }
                }
            }
            set
            {
                unitList[CurrentUnitIndex] = value;
            }
        }

        public void Refresh(ref List<Unit> unitList)
        {
            unitList = unitList.OrderBy(unit => unit.initiative).ToList();
        }

        public void NewTurn()
        {
            Refresh(ref unitList);
            CurrentUnitIndex++;
        }

        public void PlayerAttack(RaycastHit hit, MeleeEnemy isMeleeEnemyOnScene, IMiniGameLogic miniGame)
        {
            Unit unit = UnitInList.GetComponent<Unit>();
            int damage;
            damage = unit.damage;

            Enemy enemy = hit.collider.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                if (unit is RangeFriendly ||
                    unit is MeleeFriendly && enemy is MeleeEnemy ||
                    unit is MeleeFriendly && isMeleeEnemyOnScene == null)
                {
                    enemy.Damage(FormationOfTheFinalDamage(damage, _maxBonusDamage ,miniGame.GetScore, miniGame.MaxScore));
                    NewTurn();
                }
            }
        }

        public Unit EnemyAttack(Enemy enemyType)
        {
            int damage;
            damage = enemyType.damage;
            Unit friendlyToAttack = Unit.FindObjectOfType<MeleeFriendly>();
            if (enemyType is MeleeEnemy)
            {
                if (friendlyToAttack is null)
                {
                    friendlyToAttack = Unit.FindObjectOfType<RangeFriendly>();
                }
            }
            else if (enemyType is RangeEnemy)
            {
                friendlyToAttack = Unit.FindObjectOfType<RangeFriendly>();
                if (friendlyToAttack is null)
                {
                    friendlyToAttack = Unit.FindObjectOfType<MeleeFriendly>();
                }
            }
            friendlyToAttack.Damage(damage);
            NewTurn();
            return friendlyToAttack;
        }

        public IEnumerator DeleyFriendly(Unit friendly, Unit enemy)
        {
            _isAnimationOn = true;
            if (friendly != null)
            {
                friendly.AnimationAttack();
            }
            yield return new WaitForSeconds(0.5f);
            if (friendly != null)
            {
                enemy.AnimationHit();
            }
            _isAnimationOn = false;
        }

        public IEnumerator DeleyEnemy(Unit enemy, Unit friendly)
        {
            _isAnimationOn = true;
            if (enemy != null)
            {
                enemy.AnimationAttack();
            }
            yield return new WaitForSeconds(0.5f);
            if (enemy != null)
            {
                friendly.AnimationHit();
            }
            yield return new WaitForSeconds(0.5f);
            _isAnimationOn = false;
        }

        public bool isMeleeUnitOnScene() {
            foreach (var unit in unitList) {
                if (unit is MeleeEnemy) {
                    return true;
                }
            }

            return false;
        }

        public bool PlayerWin() {
            foreach (var unit in unitList) {
                if (unit is Enemy) {
                    return false;
                }
            }

            return true;
        }

        public bool PlayerLose()
        {
            foreach (var unit in unitList)
            {
                if (unit is Friendly)
                {
                    return false;
                }
            }

            return true;
        }

        private int FormationOfTheFinalDamage(int basicDamage, int score, int MaxScore)
        {
            if (Mathf.Abs(score) > Mathf.Abs(MaxScore))
            {
                score = MaxScore * (score / Mathf.Abs(score));
            }

            return (int)(basicDamage * (1f + (score / (float)MaxScore) * _bonusDamage));
        }

        private int FormationOfTheFinalDamage(int basicDamage, int MaxDamageBonus, int score, int MaxScore)
        {
            if (Mathf.Abs(score) > Mathf.Abs(MaxScore))
            {
                score = MaxScore * (score / Mathf.Abs(score));
            }

            return (int)(basicDamage + (score / (float)MaxScore) * MaxDamageBonus);
        }
    }
}
