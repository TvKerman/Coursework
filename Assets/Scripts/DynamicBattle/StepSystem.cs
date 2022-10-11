using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DynamicBattlePrototype
{
    public class StepSystem
    {
        private List<Unit> _units = new List<Unit>();
        private List<EnemyUnit> _enemyUnits = new List<EnemyUnit>();
        private List<FriendUnit> _friendUnits = new List<FriendUnit>();
        private GridBehavior _gridBehavior;
        private Unit _attackedUnit;
        private Camera mainCam;

        private bool _isEndActionPoint = false;
        private bool _isStartStep = true;
        private bool _isEnemySelect = false;
        private bool _isAttackedUnit = false;
        private bool _isAttack = false;

        private float _timeOut = 1f;
        private float _timer;
        private bool _isWait = true;
        public bool isMove = false;

        private int _currentUnitStep = 0;
        public StepSystem()
        {
            Unit[] units = MonoBehaviour.FindObjectsOfType<Unit>();
            EnemyUnit[] enemyUnits = MonoBehaviour.FindObjectsOfType<EnemyUnit>();
            FriendUnit[] friendUnits = MonoBehaviour.FindObjectsOfType<FriendUnit>();
            foreach (var unit in units)
            {
                _units.Add(unit);
            }
            foreach (var enemy in enemyUnits)
            {
                _enemyUnits.Add(enemy);
            }
            foreach (var friend in friendUnits)
            {
                _friendUnits.Add(friend);
            }

            _units.Sort();
            _gridBehavior = MonoBehaviour.FindObjectOfType<GridBehavior>();
            mainCam = Camera.main;

            foreach (Unit unit in _units)
            {
                _gridBehavior.GetGridItem(unit).GetComponent<GridStats>().SetIsOccupiedGridItem();
            }

            _timer = _timeOut;
        }

        public Unit GetUnitCurrentStep()
        {
            return _units[_currentUnitStep];
        }

        public void CurrentStep()
        {
            Unit unit = GetUnitCurrentStep();
            bool InputEnterDown = Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return);
            bool InputMouseKeyDown = Input.GetMouseButtonDown(0);

            if (unit is EnemyUnit)
            {
                EnemyTurn(unit as EnemyUnit);
            }
            else
            {
                if (unit.ActionPoint != 0 && !InputEnterDown)
                {
                    StartPlayerTurn(unit as FriendUnit);

                    if (InputMouseKeyDown)
                    {
                        RaycastHit hit;
                        bool IsRaycastHit = Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out hit);
                        GameObject hitObject = hit.collider.gameObject;
                        if (IsRaycastHit && hitObject.GetComponent<GridStats>() && (unit.IsEmptyPath || !unit.IsEmptyPath && hitObject.GetComponent<GridStats>() != unit.EndPath) && hitObject.GetComponent<GridStats>().isSelected)
                        {
                            DeselectPathPlayer(unit as FriendUnit);
                            DeselectEnemyInPlayerTurn(unit as FriendUnit);

                            _gridBehavior.FindPathBeforeGridItem(unit, hitObject.GetComponent<GridStats>());
                        }
                        else if (IsRaycastHit && hitObject.GetComponent<GridStats>() && !unit.IsEmptyPath && hitObject.GetComponent<GridStats>() == unit.EndPath)
                        {
                            MovePlayerUnitNonAttack(unit as FriendUnit);
                        }

                        EnemyUnit enemyUnit = hitObject.GetComponent<EnemyUnit>();
                        GridStats enemyGridItem = enemyUnit ? _gridBehavior.GetGridItem(enemyUnit).GetComponent<GridStats>() : null;
                        if (IsRaycastHit && enemyUnit && unit is MeleeAttackFriendUnit && !enemyGridItem.isEnemyInGridItem)
                        {
                            DeselectEnemyInPlayerTurn(unit as FriendUnit);

                            _gridBehavior.FindPathToAttack(unit, enemyGridItem);
                            SelectAndSetPathByMeleeAttackBeforePlayer(unit, enemyGridItem);
                        }
                        else if (IsRaycastHit && enemyUnit && unit is RangedAttackFriendUnit && !enemyGridItem.isEnemyInGridItem && (Math.Sqrt(Math.Pow(unit.x - enemyUnit.x, 2) + Math.Pow(unit.y - enemyUnit.y, 2)) <= (double)unit.distanceAttack))
                        {
                            DeselectEnemyInPlayerTurn(unit as FriendUnit);
                            DeselectPathEnemyByRangeAttackPlayer(unit, enemyGridItem);
                        }
                        else if (IsRaycastHit && enemyUnit && unit is MeleeAttackFriendUnit && enemyGridItem.isEnemyInGridItem)
                        {
                            PlayerAttackMeleeUnit(unit, enemyUnit, enemyGridItem);
                        }
                        else if (IsRaycastHit && enemyUnit && unit is RangedAttackFriendUnit && enemyGridItem.isEnemyInGridItem)
                        {
                            PlayerAttackRangeUnit(unit, enemyUnit, enemyGridItem);
                        }
                    }
                }
                else if (!InputEnterDown && unit.ActionPoint == 0)
                {
                    ResetMapInPlayerTurn();
                }
                else if (InputEnterDown && !isMove)
                {
                    EndPlayerTurn(unit);
                }
            }
        }


        private void StartPlayerTurn(FriendUnit unit) {
            if (_isStartStep)
            {
                _gridBehavior.SetRangeMovement(unit.x, unit.y, unit.distance);
                if (unit is RangedAttackFriendUnit)
                {
                    _gridBehavior.RangeAttackDistance(unit, unit.distanceAttack);
                }
                _gridBehavior.UpdateMap();
                if (!unit.IsEmptyPath)
                {
                    unit.GetComponent<FriendUnit>().SelectPath();
                }

                _isStartStep = false;
            }
        }

        private void PlayerAttackMeleeUnit(Unit unit, Unit enemyUnit, GridStats enemyGridItem) {
            enemyGridItem.DeselectInEnemyGridItem();
            _gridBehavior.GetGridItem(unit).GetComponent<GridStats>().SetIsFreeGridItem();
            enemyGridItem.SelectGridItem();
            _gridBehavior.ResetMap();


            unit.IsStartCoroutine = true;
            unit.DealDamage(enemyUnit);
            unit.performAction();
            if (unit.ActionPoint != 0)
                _isStartStep = true;

            _attackedUnit = enemyUnit;

            isMove = true;
            _isAttackedUnit = true;
            _isEnemySelect = false;
        }

        private void PlayerAttackRangeUnit(Unit unit, Unit enemyUnit, GridStats enemyGridItem) {
            enemyGridItem.DeselectInEnemyGridItem();
            enemyGridItem.SelectGridItem();

            unit.DealDamage(enemyUnit);
            unit.performAction();

            _attackedUnit = enemyUnit;
            _isEnemySelect = false;
            _isAttackedUnit = true;
        }

        private void EndPlayerTurn(Unit unit) {
            if (unit.ActionPoint != 0)
            {
                _gridBehavior.ResetMap();
            }

            unit.InitActionPoint();
            unit.DeselectPath();
            unit.DeletePath();

            _isEndActionPoint = false;
            _isStartStep = true;

            EndCurrentStep();
        }
    

        private void EnemyTurn(EnemyUnit enemy) {
            if (_friendUnits.Count == 0) return;

            if (!_isWait)
            {
                EnemyII(enemy);
                _isWait = true;
            }
            else
            {
                TimeOut();
            }

            if (enemy.ActionPoint == 0 && !isMove)
            {
                enemy.InitActionPoint();
                EndCurrentStep();
            }
        }

        private void EnemyII(EnemyUnit enemy)
        {
            GridStats enemyItem = _gridBehavior.GetGridItem(enemy).GetComponent<GridStats>();
            enemyItem.SetIsFreeGridItem();
            _gridBehavior.SetStartCoordinates(enemy);
            FriendUnit priorityTarget = null;
            if (enemy is MeleeAttackEnemyUnit)
            {
                int minDistance = _gridBehavior.columns * _gridBehavior.rows * _gridBehavior.scale;
                foreach (var unit in _friendUnits)
                {
                    _gridBehavior.GetGridItem(unit).GetComponent<GridStats>().SetIsFreeGridItem();
                    _gridBehavior.SetEndCoordinates(_gridBehavior.GetGridItem(unit).GetComponent<GridStats>());
                    _gridBehavior.FindPath();
                    _gridBehavior.path.RemoveAt(0);
                    if (_gridBehavior.path.Count < minDistance)
                    {
                        minDistance = _gridBehavior.path.Count;
                        priorityTarget = unit;
                    }
                    _gridBehavior.GetGridItem(unit).GetComponent<GridStats>().SetIsOccupiedGridItem();
                    _gridBehavior.ClearPath();
                }
            }
            else if (enemy is RangedAttackEnemyUnit)
            {
                int minDistance = _gridBehavior.columns * _gridBehavior.rows * _gridBehavior.scale;
                foreach (var unit in _friendUnits)
                {
                    _gridBehavior.GetGridItem(unit).GetComponent<GridStats>().SetIsFreeGridItem();
                    _gridBehavior.SetEndCoordinates(_gridBehavior.GetGridItem(unit).GetComponent<GridStats>());
                    _gridBehavior.FindPath();
                    if (_gridBehavior.path.Count != 0)
                        _gridBehavior.path.RemoveAt(0);
                    
                    int currentDistance = 0;
                    for (int index = 1; index <= _gridBehavior.path.Count; index++)
                    {
                        double distance = Math.Sqrt(Math.Pow(_gridBehavior.path[^index].GetComponent<GridStats>().x - unit.x, 2) + Math.Pow(_gridBehavior.path[^index].GetComponent<GridStats>().y - unit.y, 2));
                        if (distance < enemy.distanceAttack)
                        {
                            break;
                        }
                        currentDistance++;
                    }
                    
                    if (currentDistance < minDistance)
                    {
                        minDistance = currentDistance;
                        priorityTarget = unit;
                    }

                    _gridBehavior.GetGridItem(unit).GetComponent<GridStats>().SetIsOccupiedGridItem();
                    _gridBehavior.ClearPath();
                }
                
            }

            if (enemy is MeleeAttackEnemyUnit)
            {
                _gridBehavior.GetGridItem(priorityTarget).GetComponent<GridStats>().SetIsFreeGridItem();
                _gridBehavior.SetEndCoordinates(_gridBehavior.GetGridItem(priorityTarget).GetComponent<GridStats>());
                _gridBehavior.FindPath();
                _gridBehavior.GetGridItem(priorityTarget).GetComponent<GridStats>().SetIsOccupiedGridItem();
                _gridBehavior.path.RemoveAt(0);
                enemy.SetPath(_gridBehavior.path);
                isMove = true;
                enemy.IsStartCoroutine = true;
                enemy.performAction();
                if (Math.Sqrt(Math.Pow(priorityTarget.x - enemy.EndPath.x, 2) + Math.Pow(priorityTarget.y - enemy.EndPath.y, 2)) <= 1d && enemy.EndPath.visited <= enemy.distance)
                {
                    enemy.DealDamage(priorityTarget);
                    _attackedUnit = priorityTarget;
                    _isAttackedUnit = true;
                }
            }
            else
            {
                if (Math.Sqrt(Math.Pow(priorityTarget.x - enemy.x, 2) + Math.Pow(priorityTarget.y - enemy.y, 2)) <= enemy.distanceAttack)
                {
                    enemy.DealDamage(priorityTarget);
                    _attackedUnit = priorityTarget;
                    _isAttackedUnit = true;
                    enemyItem.SetIsOccupiedGridItem();
                }
                else
                {
                    _gridBehavior.GetGridItem(priorityTarget).GetComponent<GridStats>().SetIsFreeGridItem();
                    _gridBehavior.SetEndCoordinates(_gridBehavior.GetGridItem(priorityTarget).GetComponent<GridStats>());
                    _gridBehavior.FindPath();
                    _gridBehavior.GetGridItem(priorityTarget).GetComponent<GridStats>().SetIsOccupiedGridItem();
                    _gridBehavior.path.RemoveAt(0);
                    int copyPath = 0;
                    for (int index = 1; index <= _gridBehavior.path.Count; index++)
                    {
                        copyPath++;
                        if (Math.Sqrt(Math.Pow(_gridBehavior.path[^index].GetComponent<GridStats>().x - priorityTarget.x, 2) + Math.Pow(_gridBehavior.path[^index].GetComponent<GridStats>().y - priorityTarget.y, 2)) <= enemy.distanceAttack)
                        {
                            break;
                        }
                    }
                    List<GameObject> tmpPath = new List<GameObject>();
                    for (int i = _gridBehavior.path.Count - copyPath; i < _gridBehavior.path.Count; i++)
                    {
                        tmpPath.Add(_gridBehavior.path[i]);
                    }
                    enemy.SetPath(tmpPath);
                    //enemy.Move();
                    isMove = true;
                    enemy.IsStartCoroutine = true;
                }
                enemy.performAction();
            }
            _gridBehavior.ClearPath();
        }

        public void AnimationAttack() {
            _isAttackedUnit = false;
            if (_attackedUnit.CurrentHealthPoint > 0) {
                _attackedUnit.AnimationHitUnit();
                _attackedUnit.UpdateSlider();
            } else {
                KillUnit(_attackedUnit);
            }
        }

        public void KillUnit(Unit unit) {
            _gridBehavior.GetGridItem(unit).GetComponent<GridStats>().SetIsFreeGridItem();
            if (unit.initiative < GetUnitCurrentStep().initiative)
                _currentUnitStep--;
            if (unit is FriendUnit) {
                _friendUnits.Remove(unit.transform.root.gameObject.GetComponent<FriendUnit>());
            } else {
                _enemyUnits.Remove(unit.transform.root.gameObject.GetComponent<EnemyUnit>());
            }
            _units.Remove(unit);
            unit.AnimationKillUnit();
        }

        private void MovePlayerUnitNonAttack(FriendUnit unit) {
            _gridBehavior.GetGridItem(unit).GetComponent<GridStats>().SetIsFreeGridItem();
            _gridBehavior.ResetMap();

            unit.IsStartCoroutine = true;
            unit.performAction();
            if (unit.ActionPoint != 0)
            {
                _isStartStep = true;
            }

            isMove = true;
        }

        private void DeselectPathPlayer(FriendUnit unit) {
            if (!unit.IsEmptyPath)
            {
                unit.DeselectPath();
            }
        }

        private void DeselectEnemyInPlayerTurn(FriendUnit unit) {
            if (_isEnemySelect)
            {
                foreach (var enemy in _enemyUnits)
                {
                    _gridBehavior.GetGridItem(enemy).GetComponent<GridStats>().DeselectInEnemyGridItem();
                    _gridBehavior.GetGridItem(enemy).GetComponent<GridStats>().SelectGridItem();
                }
            }
        }

        private void DeselectPathEnemyByRangeAttackPlayer(Unit unit, GridStats enemyGridItem) {
            unit.DeselectPath();
            unit.DeletePath();

            enemyGridItem.SelectInEnemyGridItem();
            enemyGridItem.SelectGridItem();

            _isEnemySelect = true;
        }

        private void ResetMapInPlayerTurn() {
            if (!_isEndActionPoint)
            {
                _gridBehavior.ResetMap();
                _isEndActionPoint = true;
            }
        }

        private void SelectAndSetPathByMeleeAttackBeforePlayer(Unit unit, GridStats enemyGridItem) {
            if (_gridBehavior.lastItemInPath.GetComponent<GridStats>().visited <= unit.distance)
            {
                _gridBehavior.path.RemoveAt(0);

                unit.SetPath(_gridBehavior.path);
                unit.SelectPath();

                enemyGridItem.SelectInEnemyGridItem();
                enemyGridItem.SelectGridItem();

                _isEnemySelect = true;
            }

            _gridBehavior.ClearPath();
        }

        public void EndCurrentStep()
        {
            _currentUnitStep++;
            _currentUnitStep %= _units.Count;
        }

        public void AttackIsEnd() {
            _isAttackedUnit = false;
        }

        public void DestroyUnit(Unit unit) {
            Debug.Log("YbiVat");
            GameObject.Destroy(unit.gameObject);
        }

        public GridBehavior gridBehavior
        {
            get
            {
                return _gridBehavior;
            }
        }

        private void TimeOut()
        {
            if (_timer > 0f)
            {
                _timer -= Time.deltaTime;
            }
            else
            {
                _timer = _timeOut;
                _isWait = false;
            }
        }


        public List<Unit> units {
            get { return _units; }
        }

        public bool IsAttackedUnit {
            get {
                return _isAttackedUnit;
            }
        }

        public Unit AttackedUnit {
            get { return _attackedUnit; }
        }
    }
}