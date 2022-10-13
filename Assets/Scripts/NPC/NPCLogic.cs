using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCLogic : MonoBehaviour
{
    private string message = "Великие диалоги. Не знаю нужно или нет но пусть будет...";

    [SerializeField] private Movement _player;
    [SerializeField] private GameManager _gameManager;

    private int _keyScene = 2;

    private double _waitingTime = 0.5f;
    private double _timer;

    private bool _isPlayerInTrigger = false;
    private bool _isGetMessage = false;
    private bool _isStartBattle = false;
    private bool _isButtonCloseDialog = false;

    void Start()
    {
        _timer = _waitingTime;
    }

    void Update()
    {
        if (_isStartBattle) {
            //Выполнить  сохранение и запустить загрузку сцены
            _gameManager.SaveState();
            StartLoadBattleScene();
        }

        if (_isPlayerInTrigger && _timer <= 0.0)
        {
            //Надо заблокировать игроку возможность двигаться 
            _player.StopPlayer();
            _player.PlayerCanMove = false;
            GetMessage();
        }
        else if (_isPlayerInTrigger) { 
            _timer -= Time.deltaTime;
        }
    }

    private void GetMessage() {
        if (!_isGetMessage) {
            Debug.Log(message);
            _isGetMessage = true;
        }
        if (Input.GetKey(KeyCode.Return) || _isButtonCloseDialog) {
            _isStartBattle = true;
        }
    }

    private void StartLoadBattleScene() {
        _gameManager.LoadScene(_keyScene);
    }

    public bool StartBattle {
        get { return _isStartBattle; }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") {
            _isPlayerInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player") {
            _isPlayerInTrigger = false;
            _timer = _waitingTime;
        }
    }

}
