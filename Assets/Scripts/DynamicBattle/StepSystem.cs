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
                if (_friendUnits.Count == 0) return;

                if (!_isWait)
                {
                    EnemyTurn(unit.transform.root.gameObject.GetComponent<EnemyUnit>());
                    _isWait = true;
                }
                else
                {
                    TimeOut();
                }
                if (unit.ActionPoint == 0 && !isMove)
                {
                    unit.InitActionPoint();
                    EndCurrentStep();
                }
            }
            else
            {
                if (unit.ActionPoint != 0 && !InputEnterDown)
                {
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

                    if (InputMouseKeyDown)
                    {
                        RaycastHit hit;
                        bool IsRaycastHit = Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out hit);
                        GameObject hitObject = hit.collider.gameObject;
                        if (IsRaycastHit && hitObject.GetComponent<GridStats>() && (unit.IsEmptyPath || !unit.IsEmptyPath && hitObject.GetComponent<GridStats>() != unit.EndPath) && hitObject.GetComponent<GridStats>().isSelected)
                        {
                            if (!unit.IsEmptyPath)
                            {
                                unit.DeselectPath();
                            }
                            if (_isEnemySelect)
                            {
                                foreach (var enemy in _enemyUnits)
                                {
                                    _gridBehavior.GetGridItem(enemy).GetComponent<GridStats>().DeselectInEnemyGridItem();
                                    _gridBehavior.GetGridItem(enemy).GetComponent<GridStats>().SelectGridItem();
                                }
                            }
                            _gridBehavior.GetGridItem(unit).GetComponent<GridStats>().SetIsFreeGridItem();
                            _gridBehavior.SetStartCoordinates(unit);
                            _gridBehavior.SetEndCoordinates(hitObject.GetComponent<GridStats>());
                            _gridBehavior.FindPath();
                            unit.GetComponent<FriendUnit>().SetPath(_gridBehavior.path);
                            unit.GetComponent<FriendUnit>().SelectPath();
                            _gridBehavior.ClearPath();
                            _gridBehavior.GetGridItem(unit).GetComponent<GridStats>().SetIsOccupiedGridItem();
                        }
                        else if (IsRaycastHit && hitObject.GetComponent<GridStats>() && !unit.IsEmptyPath && hitObject.GetComponent<GridStats>() == unit.EndPath)
                        {
                            _gridBehavior.GetGridItem(unit).GetComponent<GridStats>().SetIsFreeGridItem();
                            unit.IsStartCoroutine = true;
                            isMove = true;
                            unit.performAction();
                            _gridBehavior.ResetMap();
                            if (unit.ActionPoint != 0)
                            {
                                _isStartStep = true;
                            }
                        }

                        EnemyUnit enemyUnit = hitObject.GetComponent<EnemyUnit>();
                        GridStats enemyGridItem = enemyUnit ? _gridBehavior.GetGridItem(enemyUnit).GetComponent<GridStats>() : null;
                        if (IsRaycastHit && enemyUnit && unit is MeleeAttackFriendUnit && !enemyGridItem.isEnemyInGridItem)
                        {
                            if (_isEnemySelect)
                            {
                                foreach (var enemy in _enemyUnits)
                                {
                                    _gridBehavior.GetGridItem(enemy).GetComponent<GridStats>().DeselectInEnemyGridItem();
                                    _gridBehavior.GetGridItem(enemy).GetComponent<GridStats>().SelectGridItem();
                                }
                            }
                            unit.DeselectPath();
                            unit.DeletePath();
                            _gridBehavior.GetGridItem(unit).GetComponent<GridStats>().SetIsFreeGridItem();
                            _gridBehavior.SetStartCoordinates(unit);
                            enemyGridItem.SetIsFreeGridItem();
                            _gridBehavior.SetEndCoordinates(enemyGridItem);
                            _gridBehavior.FindPath();
                            enemyGridItem.SetIsOccupiedGridItem();
                            _gridBehavior.GetGridItem(unit).GetComponent<GridStats>().SetIsOccupiedGridItem();
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
                        else if (IsRaycastHit && enemyUnit && unit is RangedAttackFriendUnit && !enemyGridItem.isEnemyInGridItem && (Math.Sqrt(Math.Pow(unit.x - enemyUnit.x, 2) + Math.Pow(unit.y - enemyUnit.y, 2)) <= (double)unit.distanceAttack))
                        {
                            if (_isEnemySelect)
                            {
                                foreach (var enemy in _enemyUnits)
                                {
                                    _gridBehavior.GetGridItem(enemy).GetComponent<GridStats>().DeselectInEnemyGridItem();
                                    _gridBehavior.GetGridItem(enemy).GetComponent<GridStats>().SelectGridItem();
                                }
                            }
                            unit.DeselectPath();
                            unit.DeletePath();
                            enemyGridItem.SelectInEnemyGridItem();
                            enemyGridItem.SelectGridItem();
                            _isEnemySelect = true;
                        }
                        else if (IsRaycastHit && enemyUnit && unit is MeleeAttackFriendUnit && enemyGridItem.isEnemyInGridItem)
                        {
                            enemyGridItem.DeselectInEnemyGridItem();
                            _isEnemySelect = false;
                            _gridBehavior.GetGridItem(unit).GetComponent<GridStats>().SetIsFreeGridItem();
                            unit.IsStartCoroutine = true;
                            isMove = true;
                            unit.DealDamage(enemyUnit);
                            _attackedUnit = enemyUnit;
                            _isAttackedUnit = true;
                            unit.performAction();

                            enemyGridItem.SelectGridItem();
                            _gridBehavior.ResetMap();
                            if (unit.ActionPoint != 0)
                                _isStartStep = true;
                        }
                        else if (IsRaycastHit && enemyUnit && unit is RangedAttackFriendUnit && enemyGridItem.isEnemyInGridItem)
                        {
                            enemyGridItem.DeselectInEnemyGridItem();
                            _isEnemySelect = false;
                            unit.DealDamage(enemyUnit);
                            _attackedUnit = enemyUnit;
                            _isAttackedUnit = true;
                            unit.performAction();
                            enemyGridItem.SelectGridItem();
                        }
                    }
                }
                else if (!InputEnterDown && unit.ActionPoint == 0)
                {
                    if (!_isEndActionPoint)
                    {
                        _gridBehavior.ResetMap();
                        _isEndActionPoint = true;
                    }
                }
                else if (InputEnterDown && !isMove)
                {
                    if (unit.ActionPoint != 0)
                    {
                        _gridBehavior.ResetMap();
                    }
                    unit.InitActionPoint();
                    unit.DeselectPath();
                    unit.DeletePath();
                    EndCurrentStep();
                    _isEndActionPoint = false;
                    _isStartStep = true;
                }
            }
        }

        private void EnemyTurn(EnemyUnit enemy)
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
            //Debug.Log($"Unit attacked:{_attackedUnit}, health point: {_attackedUnit.CurrentHealthPoint}");
            _isAttackedUnit = false;
            if (_attackedUnit.CurrentHealthPoint > 0)
                _attackedUnit.UpdateSlider();
            else
                KillUnit(_attackedUnit);
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
            GameObject.Destroy(unit.transform.root.gameObject);
        }

        public void EndCurrentStep()
        {
            _currentUnitStep++;
            _currentUnitStep %= _units.Count;
        }

        public void AttackIsEnd() {
            _isAttackedUnit = false;
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

        public Unit AttackedUnit {
            get { return _attackedUnit; }
            
        }

        public bool IsAttackedUnit {
            get {
                return _isAttackedUnit;
            }
        }
    }
}