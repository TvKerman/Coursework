using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DynamicBattlePrototype
{
    public class GameManager : MonoBehaviour
    {

        [SerializeField] private bool _isMove = false;
        private StepSystem _stepSystem;

        private float _timeOut = 1f;
        private float _timer;
        private bool _isWait = false;

        void Start()
        {
            _timer = _timeOut;
            _stepSystem = new StepSystem();
        }

        void Update()
        {
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
    }
}