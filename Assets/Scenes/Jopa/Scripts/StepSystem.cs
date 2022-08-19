using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepSystem
{
    private List<Unit> _units = new List<Unit>();
    private List<EnemyUnit> _enemyUnits = new List<EnemyUnit>();
    private List<FriendUnit> _friendUnits = new List<FriendUnit>();
    private GridBehavior _gridBehavior;
    private Camera mainCam;

    private bool _isEndActionPoint = false;
    private bool _isStartStep = true;
    private bool _isEnemySelect = false;

    private float _timeOut = 3f;
    private float _timer;
    private bool _isWait = true;
    public bool isMove = false;

    private int _currentUnitStep = 0;
    public StepSystem ()
    {
        Unit[] units = MonoBehaviour.FindObjectsOfType<Unit>();
        EnemyUnit[] enemyUnits = MonoBehaviour.FindObjectsOfType<EnemyUnit>();
        FriendUnit[] friendUnits = MonoBehaviour.FindObjectsOfType<FriendUnit>();
        foreach (var unit in units) {
            _units.Add(unit);
        }
        foreach (var enemy in enemyUnits) {
            _enemyUnits.Add(enemy);
        }
        foreach (var friend in friendUnits) {
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

    public Unit GetUnitCurrentStep() {
        return _units[_currentUnitStep];
    }

    public void CurrentStep() {
        Unit unit = GetUnitCurrentStep();
        bool InputEnterKeypadDown = Input.GetKeyDown(KeyCode.KeypadEnter);
        bool InputMouseKeyDown = Input.GetMouseButtonDown(0);
        if (unit is EnemyUnit) {
            if (_friendUnits.Count == 0) return;

            if (!_isWait) {
                EnemyTurn(unit.transform.root.gameObject.GetComponent<EnemyUnit>());
                Debug.Log("Enemy Step");
                _isWait = true;
            } else {
                TimeOut();
            }
            if (unit.ActionPoint == 0 && !isMove) {
                unit.InitActionPoint();
                EndCurrentStep();
            }
        } else {
            if (unit.ActionPoint != 0 && !InputEnterKeypadDown) {
                if (_isStartStep) {
                    _gridBehavior.SetRangeMovement(unit.x, unit.y, unit.distance);
                    if (unit is RangedAttackFriendUnit) {
                        _gridBehavior.RangeAttackDistance(unit, unit.distanceAttack);
                    }
                    _gridBehavior.UpdateMap();
                    if (!unit.IsEmptyPath) {
                        unit.GetComponent<FriendUnit>().SelectPath();
                    }

                    _isStartStep = false;
                }

                if (InputMouseKeyDown) {
                    RaycastHit hit;
                    bool IsRaycastHit = Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out hit);
                    GameObject hitObject = hit.collider.gameObject;
                    Debug.Log($"{IsRaycastHit}, {hit}, {hitObject}");
                    if (IsRaycastHit && hitObject.GetComponent<GridStats>() && (unit.IsEmptyPath || !unit.IsEmptyPath && hitObject.GetComponent<GridStats>() != unit.EndPath) && hitObject.GetComponent<GridStats>().isSelected) {
                        if (!unit.IsEmptyPath) {
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
                    } else if (IsRaycastHit && hitObject.GetComponent<GridStats>() && !unit.IsEmptyPath && hitObject.GetComponent<GridStats>() == unit.EndPath) {
                        _gridBehavior.GetGridItem(unit).GetComponent<GridStats>().SetIsFreeGridItem();
                        isMove = true;
                        unit.performAction();
                        _gridBehavior.ResetMap();
                        if (unit.ActionPoint != 0) {
                            _isStartStep = true;
                        }
                    }

                    EnemyUnit enemyUnit = hitObject.GetComponent<EnemyUnit>();
                    GridStats enemyGridItem = enemyUnit ? _gridBehavior.GetGridItem(enemyUnit).GetComponent<GridStats>(): null;
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
                    } else if (IsRaycastHit && enemyUnit && unit is RangedAttackFriendUnit && !enemyGridItem.isEnemyInGridItem && (Math.Sqrt(Math.Pow(unit.x - enemyUnit.x, 2) + Math.Pow(unit.y - enemyUnit.y, 2)) <= (double)unit.distanceAttack)) {
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
                    } else if (IsRaycastHit && enemyUnit && unit is MeleeAttackFriendUnit && enemyGridItem.isEnemyInGridItem) {
                        enemyGridItem.DeselectInEnemyGridItem();
                        _isEnemySelect = false;
                        _gridBehavior.GetGridItem(unit).GetComponent<GridStats>().SetIsFreeGridItem();
                        isMove = true;
                        enemyUnit.TakeDamage(unit.damage);
                        unit.performAction();
                        if (enemyUnit.healthPoint <= 0) {
                            enemyGridItem.SetIsFreeGridItem();
                            enemyGridItem.SelectItem();
                            if (enemyUnit.initiative < unit.initiative)
                                _currentUnitStep--;
                            _enemyUnits.Remove(enemyUnit);
                            _units.Remove(enemyUnit);
                            GameObject.Destroy(hitObject);
                        }
                        enemyGridItem.SelectGridItem();
                    } else if (IsRaycastHit && enemyUnit && unit is RangedAttackFriendUnit && enemyGridItem.isEnemyInGridItem) {
                        enemyGridItem.DeselectInEnemyGridItem();
                        _isEnemySelect = false;
                        enemyUnit.TakeDamage(unit.damage);
                        unit.performAction();
                        if (enemyUnit.healthPoint <= 0)
                        {
                            enemyGridItem.SetIsFreeGridItem();
                            if (enemyUnit.initiative < unit.initiative)
                                _currentUnitStep--;
                            _enemyUnits.Remove(enemyUnit);
                            _units.Remove(enemyUnit);
                            GameObject.Destroy(hitObject);
                        }
                        enemyGridItem.SelectGridItem();
                    }
                }
            } else if (!InputEnterKeypadDown && unit.ActionPoint == 0) {
                if (!_isEndActionPoint) {
                    _gridBehavior.ResetMap();
                    _isEndActionPoint = true;
                }
            } else if (InputEnterKeypadDown && !isMove) {
                if (unit.ActionPoint != 0) {
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

    private void EnemyTurn(EnemyUnit enemy) {
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
        else if (enemy is RangedAttackEnemyUnit){
            //float minDistance = Mathf.Sqrt(Mathf.Pow(_gridBehavior.columns, 2) + Mathf.Pow(_gridBehavior.rows, 2));
            int minDistance = _gridBehavior.columns * _gridBehavior.rows * _gridBehavior.scale;
            foreach (var unit in _friendUnits)
            {
                _gridBehavior.GetGridItem(unit).GetComponent<GridStats>().SetIsFreeGridItem();
                _gridBehavior.SetEndCoordinates(_gridBehavior.GetGridItem(unit).GetComponent<GridStats>());
                _gridBehavior.FindPath();
                _gridBehavior.path.RemoveAt(0);
                int currentDistance = 0;
                for (int index = 1; index <= _gridBehavior.path.Count; index++) {
                    double distance = Math.Sqrt(Math.Pow(_gridBehavior.path[^index].GetComponent<GridStats>().x - unit.x, 2) + Math.Pow(_gridBehavior.path[^index].GetComponent<GridStats>().y - unit.y, 2));
                    if (distance < enemy.distanceAttack) {
                        break;
                    }
                    currentDistance++;
                }
                //Debug.Log($"{unit}: distance {currentDistance}");
                if (currentDistance < minDistance)
                {
                    minDistance = currentDistance;
                    priorityTarget = unit;
                }

                _gridBehavior.GetGridItem(unit).GetComponent<GridStats>().SetIsOccupiedGridItem();
                _gridBehavior.ClearPath();
            }
            //Debug.Log($"Priority target:{priorityTarget}");
        }

        if (enemy is MeleeAttackEnemyUnit)
        {
            _gridBehavior.GetGridItem(priorityTarget).GetComponent<GridStats>().SetIsFreeGridItem();
            _gridBehavior.SetEndCoordinates(_gridBehavior.GetGridItem(priorityTarget).GetComponent<GridStats>());
            _gridBehavior.FindPath();
            _gridBehavior.path.RemoveAt(0);
            enemy.SetPath(_gridBehavior.path);
            isMove = true;
            enemy.performAction();
            if (Math.Sqrt(Math.Pow(enemy.x - priorityTarget.x, 2) + Math.Pow(enemy.y - priorityTarget.y, 2)) <= 1d)
            {
                enemy.DealDamage(priorityTarget);
                if (priorityTarget.healthPoint <= 0)
                {
                    _gridBehavior.GetGridItem(priorityTarget).GetComponent<GridStats>().SetIsFreeGridItem();
                    if (priorityTarget.initiative < enemy.initiative)
                        _currentUnitStep--;
                    _friendUnits.Remove(priorityTarget);
                    _units.Remove(priorityTarget);
                    GameObject.Destroy(priorityTarget.transform.root.gameObject);
                }

            }
        } else {
            if (Math.Sqrt(Math.Pow(priorityTarget.x - enemy.x, 2) + Math.Pow(priorityTarget.y - enemy.y, 2)) <= enemy.distanceAttack)
            {
                enemy.DealDamage(priorityTarget);
                enemyItem.SetIsOccupiedGridItem();
                if (priorityTarget.healthPoint <= 0)
                {
                    _gridBehavior.GetGridItem(priorityTarget).GetComponent<GridStats>().SetIsFreeGridItem();
                    if (priorityTarget.initiative < enemy.initiative)
                        _currentUnitStep--;
                    _friendUnits.Remove(priorityTarget);
                    _units.Remove(priorityTarget);
                    GameObject.Destroy(priorityTarget.transform.root.gameObject);
                }

            } else {
                _gridBehavior.GetGridItem(priorityTarget).GetComponent<GridStats>().SetIsFreeGridItem();
                _gridBehavior.SetEndCoordinates(_gridBehavior.GetGridItem(priorityTarget).GetComponent<GridStats>());
                _gridBehavior.FindPath();
                _gridBehavior.path.RemoveAt(0);
                int copyPath = 0;
                for (int index = 1; index <= _gridBehavior.path.Count; index++) {
                    copyPath++;
                    if (Math.Sqrt(Math.Pow(_gridBehavior.path[^index].GetComponent<GridStats>().x - priorityTarget.x, 2) + Math.Pow(_gridBehavior.path[^index].GetComponent<GridStats>().y - priorityTarget.y, 2)) <= enemy.distanceAttack) {
                        break;
                    }
                }
                List<GameObject> tmpPath = new List<GameObject>();
                for (int i = _gridBehavior.path.Count - copyPath; i < _gridBehavior.path.Count; i++) {
                    tmpPath.Add(_gridBehavior.path[i]);
                }
                enemy.SetPath(tmpPath);
                //enemy.Move();
                isMove = true;
            }
            enemy.performAction();
        }
        _gridBehavior.ClearPath();
        //enemyItem.SetIsOccupiedGridItem();
    }

    public void EndCurrentStep() {
        _currentUnitStep++;
        _currentUnitStep %= _units.Count;
    }

    public GridBehavior gridBehavior {
        get {
            return _gridBehavior;
        }
    }

    private void TimeOut() {
        if (_timer > 0f)
        {
            _timer -= Time.deltaTime;
        }
        else {
            _timer = _timeOut;
            _isWait = false;
        }
    }
}
