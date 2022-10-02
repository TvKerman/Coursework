using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DynamicBattlePrototype
{
    public class Unit : MonoBehaviour, IComparable<Unit>
    {
        [SerializeField] private Slider slider;
        [SerializeField] private Animator _animator;

        public bool isSelected;
        public int x;
        public int y;
        public int initiative;
        public int distance;
        public int actionPoint;
        public int healthPoint;
        public int damage;
        public int distanceAttack;

        public bool _isAnimation = false;
        public bool _isStopAnimationCoroutine = true;
        public bool _isStartCoroutine = false;
        private bool _isAttackAnimation = false;
        private bool _isDeadAnimation = false;
        private bool _isDeadUnit = false;
        private bool _isPositiveDeltaX;
        private bool _isPositiveDeltaZ;
        private int _currentDistance;
        private int _currentActionPoint;
        private int _currentHealthPoint;
        private float _deltaX;
        private float _deltaZ;
        private float _Exc = 0.0001f;
        private float _speed = 2f;
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

        public IEnumerator Move(StepSystem stepSystem) {
            _isStopAnimationCoroutine = false;
            if (!IsEmptyPath && StartPath.x == x && StartPath.y == y)
                _path.RemoveAt(_path.Count - 1);
            if (_animator)
                _animator.SetBool("Move", true);
            while (!IsEmptyPath)
            {
                if (_currentDistance == 0)
                {
                    break;
                }
        
                _deltaX = StartPath.transform.position.x - transform.position.x;
                _deltaZ = StartPath.transform.position.z - transform.position.z;
                _isPositiveDeltaX = _deltaX > 0;
                _isPositiveDeltaZ = _deltaZ > 0;
                _isAnimation = true;
                StartPath.DeleteItemInPath();
                StartPath.SelectGridItem();
                SetGridCoordinates(StartPath);
                _currentDistance--;
                
                yield return StartCoroutine(this.AnimationMove());  
            }
            _isAnimation = false;
            if (_animator)
                _animator.SetBool("Move", false);
            _isStopAnimationCoroutine = true;
            stepSystem.isMove = false;
            stepSystem.gridBehavior.GetGridItem(this).GetComponent<GridStats>().SetIsOccupiedGridItem();
            _currentDistance = distance;
        }

        public IEnumerator AnimationMove() {
            while ((Math.Abs(StartPath.transform.position.x - transform.position.x) > _Exc ||
                        Math.Abs(StartPath.transform.position.z - transform.position.z) > _Exc) &&
                        ((StartPath.transform.position.x - transform.position.x > 0) == _isPositiveDeltaX) && 
                        ((StartPath.transform.position.z - transform.position.z > 0) == _isPositiveDeltaZ)) {
                transform.position = new Vector3(transform.position.x + _deltaX * Time.deltaTime * _speed, transform.position.y, transform.position.z + _deltaZ * Time.deltaTime * _speed);
                yield return null;
            }
            transform.position = new Vector3(StartPath.x, transform.position.y, StartPath.y);
            _path.RemoveAt(_path.Count - 1);
            _isAnimation = false;
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

        private GridStats StartPath
        {
            get {
                if (IsEmptyPath) throw new System.Exception();
                return _path[^1];
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

        private void TakeDamage(int damage)
        {
            _currentHealthPoint -= damage;
        }

        public void AnimationKillUnit() {
            _isDeadAnimation = true;
            slider.gameObject.SetActive(false);
            if (_animator != null)
                _animator.SetBool("Dead", true);
        }

        public void AnimationAttackUnit() {
            if (_animator != null)
                _animator.SetBool("Attack", true);
        }

        public void AnimationHitUnit() {
            if (_animator != null) {
                _animator.SetBool("Hit", true);
            }
        }

        public void DeletePath()
        {
            _path.Clear();
        }

        public void InitHealthPoint() {
            _currentHealthPoint = healthPoint;
        }

        public void UpdateSlider() {
            slider.value = ((float)_currentHealthPoint / healthPoint);
        }

        public Animator Animator {
            get { return _animator; }
        }

        public bool IsAnimation {
            get {
                return _isAnimation;
            }
        }

        public bool StopAnimationCoroutine {
            get {
                return _isStopAnimationCoroutine;
            }
        }

        public bool IsStartCoroutine {
            get {
                return _isStartCoroutine;
            }
            set {
                _isStartCoroutine = value;
            }
        }

        public int CurrentHealthPoint {
            get {
                return _currentHealthPoint;
            }
        }

        public bool isDeadUnit {
            get { return _isDeadUnit; }
            set { _isDeadUnit = value; }
        }

        public bool isAnimationDead {
            get { return _isDeadAnimation; }
        }
    }
}
