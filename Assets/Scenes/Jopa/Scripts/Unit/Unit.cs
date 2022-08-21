using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DynamicBattlePrototype
{
    public class Unit : MonoBehaviour, IComparable<Unit>
    {
        public bool isSelected;
        public int x;
        public int y;
        public int initiative;
        public int distance;
        public int actionPoint;
        public int healthPoint;
        public int damage;
        public int distanceAttack;

        private int _currentDistance;
        private int _currentActionPoint;
        private List<GridStats> _path = new List<GridStats>();

        public void SetPath(List<GameObject> newPath)
        {
            _currentDistance = distance;
            _path.Clear();
            foreach (GameObject item in newPath)
            {
                _path.Add(item.GetComponent<GridStats>());
            }
        }

        public IEnumerator Move(StepSystem stepSystem)
        {
            //Debug.Log($"Start move");
            for (int i = 0; i < _path.Count;)
            {
                Debug.Log($"{_currentDistance}");
                if (_currentDistance == 0)
                {
                    break;
                }
                gameObject.transform.position = new Vector3(_path[^1].transform.position.x, gameObject.transform.position.y, _path[^1].transform.position.z);
                _path[^1].DeleteItemInPath();
                _path[^1].SelectGridItem();
                SetGridCoordinates(_path[^1]);
                _path.RemoveAt(_path.Count - 1);

                _currentDistance--;
                if (_path.Count != 0)
                {
                    yield return new WaitForSeconds(1000000000000000000000f);
                }
            }
            stepSystem.isMove = false;
            stepSystem.gridBehavior.GetGridItem(this).GetComponent<GridStats>().SetIsOccupiedGridItem();
            _currentDistance = distance;
        }

        public void SetGridCoordinates(GridStats currentItem)
        {
            x = currentItem.x;
            y = currentItem.y;
        }

        public bool IsEmptyPath
        {
            get
            {
                return _path.Count == 0;
            }
        }

        public int CompareTo(Unit? unit)
        {
            if (unit is null) throw new System.Exception();
            return initiative - unit.initiative;
        }

        public void InitActionPoint()
        {
            _currentActionPoint = actionPoint;
        }

        public GridStats EndPath
        {
            get
            {
                if (IsEmptyPath) throw new System.Exception();
                return _path[0];
            }
        }

        public int ActionPoint
        {
            get
            {
                return _currentActionPoint;
            }
        }

        public void performAction()
        {
            _currentActionPoint--;
        }

        public void SelectPath()
        {
            foreach (var item in _path)
            {
                item.AddItemInPath();
                item.SelectGridItem();
            }
        }

        public void DeselectPath()
        {
            foreach (var item in _path)
            {
                if (item.visited >= distance)
                    item.DeselectItem();
                item.DeleteItemInPath();
                item.SelectGridItem();
            }
        }

        public void DealDamage(Unit enemy)
        {
            enemy.TakeDamage(damage);
        }

        public void TakeDamage(int damage)
        {
            healthPoint -= damage;
        }

        public void DeletePath()
        {
            _path.Clear();
        }
    }
}