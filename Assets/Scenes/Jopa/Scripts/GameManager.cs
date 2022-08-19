using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            _stepSystem.CurrentStep();
        else {
            
            StartCoroutine(_stepSystem.GetUnitCurrentStep().Move(_stepSystem));
            
        }
    }
}
