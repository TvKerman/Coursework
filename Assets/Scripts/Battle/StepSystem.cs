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


        public StepSystem(List<Unit> _unitList)
        {
            unitList = _unitList;
            Refresh(ref unitList);
        }

        public bool isAnimationOn
        {
            get
            {
                return _isAnimationOn;
            }
            set
            {
                _isAnimationOn = value;
            }
        }

        public int CurrentUnitIndex
        {
            get
            {
                return _currentUnitIndex;
            }
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

        public void PlyerAttack(RaycastHit hit, MeleeEnemy isMeleeEnemyOnScene)
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
                    enemy.Damage(damage);
                    Debug.Log(damage);
                    NewTurn();
                }
            }
        }

        public void EnemyAttack(Enemy enemyType)
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
        }

        public IEnumerator DeleyFriendly(Unit friendly)
        {
            _isAnimationOn = true;
            if (friendly != null)
            {
                friendly.GetComponent<Renderer>().material.color = Color.red;
            }
            yield return new WaitForSeconds(0.5f);
            if (friendly != null)
            {
                friendly.GetComponent<Renderer>().material.color = Color.blue;
            }
            _isAnimationOn = false;
        }

        public IEnumerator DeleyEnemy(Unit enemy)
        {
            _isAnimationOn = true;
            if (enemy != null)
            {
                enemy.GetComponent<Renderer>().material.color = Color.blue;
            }
            yield return new WaitForSeconds(0.5f);
            if (enemy != null)
            {
                enemy.GetComponent<Renderer>().material.color = Color.red;
            }
            _isAnimationOn = false;
        }
    }
}
