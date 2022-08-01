using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] private GameObject _player;
    [SerializeField] private GridBehavior _gridBehavior;
    [SerializeField] private bool _isMove = false;

    public float _timeOut = 1f;
    public float _timer;
    public bool _isWait = false;
    public int _currentIndexPath = 0;
    // Start is called before the first frame update
    void Start()
    {
        _timer = _timeOut;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isMove && !_isWait && _gridBehavior.path.Count != 0 && _currentIndexPath != _gridBehavior.path.Count)
        {
            if (_currentIndexPath == 0)
            {
                _gridBehavior.path.Reverse();
            }
            _player.transform.position = _gridBehavior.path[_currentIndexPath++].transform.position;
            _isWait = true;
        }
        else if (_isWait)
        {
            Wait();
        }
        else if (_currentIndexPath >= _gridBehavior.path.Count) {
            _currentIndexPath = 0;
            _isMove = false;
        }
    }

    private void Wait() {
        if (_isWait && _timer > 0f)
        {
            _timer -= Time.deltaTime;
        }
        else if (_isWait && _timer <= 0f) { 
            _isWait = false;
            _timer = _timeOut;
        }
    }
}
