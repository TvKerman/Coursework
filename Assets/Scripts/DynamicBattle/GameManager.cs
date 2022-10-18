using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace DynamicBattlePrototype
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameObject _camera;
        private StepSystem _stepSystem;
        private Unit _DeadInside;

        private float _timeOut = 1f;
        private float _timer;
        private float _cameraSpeed = 3f;

        private float _xRightBoarder;
        private float _yUpBoarder;
        private float _xLeftBoarder;
        private float _yDownBoarder;

        private float _exp = 0.1f;

        private int _currentRotationCamera = 0;

        //private bool _isWait = false;
        private bool _isAnimationCamera = false;
        private bool _isAnimationDead = false;

        private List<Vector3> navigation = new List<Vector3>() {new Vector3(0, 0, 1), 
                                                                new Vector3(1, 0, 0), 
                                                                new Vector3(0, 0, -1), 
                                                                new Vector3(-1, 0, 0)};

        void Start()
        {
            _timer = _timeOut;
            _stepSystem = new DynamicBattlePrototype.StepSystem();

            _xLeftBoarder = 2f;
            _yDownBoarder = 2f;
            _xRightBoarder = _stepSystem.gridBehavior.columns - (int)_xLeftBoarder;
            _yUpBoarder = _stepSystem.gridBehavior.rows - (int)_yDownBoarder;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                SceneManager.LoadScene(0);
            }

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
                    _stepSystem.GetUnitCurrentStep().AnimationAttackUnit();
                    _isAnimationDead = _stepSystem.AttackedUnit.isAnimationDead;
                    _DeadInside = _stepSystem.AttackedUnit;
                }
                if (_DeadInside != null && _DeadInside.isDeadUnit) {
                    _stepSystem.DestroyUnit(_DeadInside);
                }
            }

        }

        private void CameraControl() {
            if (_camera != null) {
                CameraMove();
                CameraRotation();
            }
        }

        private void CameraMove() {
            CameraInputMoveKeyCode(KeyCode.W, 0);
            CameraInputMoveKeyCode(KeyCode.D, 1);
            CameraInputMoveKeyCode(KeyCode.S, 2);
            CameraInputMoveKeyCode(KeyCode.A, 3);
        }

        private bool CameraInBoarder(Vector3 navigation) {
            Vector3 newPosition = _camera.transform.position + navigation;
            return newPosition.x > _xLeftBoarder && newPosition.x < _xRightBoarder && newPosition.z > _yDownBoarder && newPosition.z < _yUpBoarder;
        }

        private void CameraRotation() {
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

        private void CameraInputMoveKeyCode(KeyCode Button, int direction) {
            if (Input.GetKey(Button) && CameraInBoarder(navigation[(direction + _currentRotationCamera) % 4] * Time.deltaTime * _cameraSpeed)) {
                _camera.transform.position = _camera.transform.position + navigation[(direction + _currentRotationCamera) % 4] * Time.deltaTime * _cameraSpeed;
            }
        }

        IEnumerator AnimationRotationCamera(float start, float delta) {
            _isAnimationCamera = true;
            float currentDelta = delta;
            float end = start + delta;
            while (Mathf.Abs(currentDelta) > Mathf.Abs(delta) * Time.deltaTime + _exp) {
                currentDelta -= delta * Time.deltaTime;
                DeltaRotateCamera(_camera, delta * Time.deltaTime);
                foreach (var unit in _stepSystem.units) {
                    DeltaRotateUI(unit, getEndLocalEulerAnglesUI(unit));
                }
                yield return null;
            }

            if (end < 0f) {
                end += 360f;
            }

            SetCameraRotation(_camera, GetEndRotation(_camera, end));
            foreach (var unit in _stepSystem.units)
            {
                DeltaRotateUI(unit, getEndLocalEulerAnglesUI(unit));
            }

            _isAnimationCamera = false;
        }

        private void DeltaRotateUI(Unit unit, Vector3 localEulerAngles) {
            unit.transform.root.GetComponentInChildren<Slider>().transform.localEulerAngles = localEulerAngles;
        }

        private void DeltaRotateCamera(GameObject camera, float delta) {
            camera.transform.localEulerAngles = new Vector3(camera.transform.localEulerAngles.x, 
                                                            camera.transform.localEulerAngles.y + delta, 
                                                            camera.transform.localEulerAngles.z);
        }

        private void SetCameraRotation(GameObject camera, Vector3 rotate) {
            camera.transform.localEulerAngles = rotate;
        }

        private Vector3 GetEndRotation(GameObject camera, float end) {
            return new Vector3(_camera.transform.localEulerAngles.x, end % 360f, _camera.transform.localEulerAngles.z);
        }
        private Vector3 getEndLocalEulerAnglesUI(Unit unit) { 
        return new Vector3(unit.transform.root.GetComponentInChildren<Slider>().transform.localEulerAngles.x,
                           _camera.transform.localEulerAngles.y,
                           unit.transform.root.GetComponentInChildren<Slider>().transform.localEulerAngles.z);
        }
    }
}