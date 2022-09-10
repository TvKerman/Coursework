using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DynamicBattlePrototype
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameObject _camera;
        private StepSystem _stepSystem;

        private float _timeOut = 1f;
        private float _timer;
        private float _cameraSpeed = 3f;

        private float _xRightBoarder;
        private float _yUpBoarder;
        private float _xLeftBoarder;
        private float _yDownBoarder;

        private float _exp = 0.4f;

        private int _currentRotationCamera = 0;

        //private bool _isWait = false;
        private bool _isAnimationCamera = false;

        private List<Vector3> navigation = new List<Vector3>() {new Vector3(0, 0, 1), 
                                                                new Vector3(1, 0, 0), 
                                                                new Vector3(0, 0, -1), 
                                                                new Vector3(-1, 0, 0)};

        void Start()
        {
            _timer = _timeOut;
            _stepSystem = new StepSystem();

            _xLeftBoarder = 2f;
            _yDownBoarder = 2f;
            _xRightBoarder = _stepSystem.gridBehavior.columns - (int)_xLeftBoarder;
            _yUpBoarder = _stepSystem.gridBehavior.rows - (int)_yDownBoarder;
        }

        void Update()
        {
            if (!_isAnimationCamera)
                CameraControl();

            if (!_stepSystem.isMove)
            {
                _stepSystem.CurrentStep();

                if (_stepSystem.GetUnitCurrentStep().IsStartCoroutine) {
                    StartCoroutine(_stepSystem.GetUnitCurrentStep().Move(_stepSystem));
                    _stepSystem.GetUnitCurrentStep().IsStartCoroutine = false;
                }
                if (_stepSystem.GetUnitCurrentStep().StopAnimationCoroutine && _stepSystem.IsAttackedUnit) {
                    _stepSystem.AnimationAttack();
                }
            }
        }

        void CameraControl() {
            if (_camera != null) {
                CameraMove();
                CameraRotation();
            }
        }

        bool CameraInBoarder(Vector3 navigation) {
            Vector3 newPosition = _camera.transform.position + navigation;
            return newPosition.x > _xLeftBoarder && newPosition.x < _xRightBoarder && newPosition.z > _yDownBoarder && newPosition.z < _yUpBoarder;
        }

        void CameraRotation() {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                _currentRotationCamera++;
                _currentRotationCamera %= 4;
                StartCoroutine(AnimationRotationCamera(_camera.transform.localEulerAngles.y, 90f));
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                _currentRotationCamera--;
                if (_currentRotationCamera < 0)
                    _currentRotationCamera = 3;

                StartCoroutine(AnimationRotationCamera(_camera.transform.localEulerAngles.y, -90f));
            }
        }

        void CameraMove() {
            CameraInputMoveKeyCode(KeyCode.W, 0);
            CameraInputMoveKeyCode(KeyCode.D, 1);
            CameraInputMoveKeyCode(KeyCode.S, 2);
            CameraInputMoveKeyCode(KeyCode.A, 3);
        }

        void CameraInputMoveKeyCode(KeyCode Button, int direction) {
            if (Input.GetKey(Button) && CameraInBoarder(navigation[(direction + _currentRotationCamera) % 4] * Time.deltaTime * _cameraSpeed)) {
                _camera.transform.position = _camera.transform.position + navigation[(direction + _currentRotationCamera) % 4] * Time.deltaTime * _cameraSpeed;
            }
        }

        IEnumerator AnimationRotationCamera(float start, float delta) {
            _isAnimationCamera = true;
            float _currentDelta = delta;
            float end = start + delta;
            while (Mathf.Abs(_currentDelta) > _exp) {
                _currentDelta -= delta * Time.deltaTime;
                _camera.transform.localEulerAngles = new Vector3(_camera.transform.localEulerAngles.x, _camera.transform.localEulerAngles.y + delta * Time.deltaTime, _camera.transform.localEulerAngles.z);
                foreach (var unit in _stepSystem.units) {
                    unit.transform.root.GetComponentInChildren<Canvas>().transform.localEulerAngles = new Vector3(unit.transform.root.GetComponentInChildren<Canvas>().transform.localEulerAngles.x,
                                                                                                                  _camera.transform.localEulerAngles.y,
                                                                                                                  unit.transform.root.GetComponentInChildren<Canvas>().transform.localEulerAngles.z);
                }
                yield return null;
            }
            if (end < 0f) {
                end += 360f;
            }
            _camera.transform.localEulerAngles = new Vector3(_camera.transform.localEulerAngles.x, end % 360f, _camera.transform.localEulerAngles.z);
            foreach (var unit in _stepSystem.units)
            {
                unit.transform.root.GetComponentInChildren<Canvas>().transform.localEulerAngles = new Vector3(unit.transform.root.GetComponentInChildren<Canvas>().transform.localEulerAngles.x,
                                                                                                              _camera.transform.localEulerAngles.y,
                                                                                                              unit.transform.root.GetComponentInChildren<Canvas>().transform.localEulerAngles.z);
            }
            _isAnimationCamera = false;
        }
    }
}