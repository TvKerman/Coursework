using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UICurrentTurn : MonoBehaviour
{
    private TextMeshProUGUI textMeshPro;

    private bool _isEnemyTurn = false;
    private bool _isPlayerTurn = false;
    private bool _isPlayerWin = false;
    private bool _isPlayerLose = false;

    private const string _enemyTurn = "Ход противника";
    private const  string _playerTurn = "Ваш ход";

    private const string _win = "Победа";
    private const string _lose = "Поражение";

    private void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    public void SetEnemyTurn() {
        textMeshPro.text = _enemyTurn;
        _isEnemyTurn = true;
        _isPlayerTurn = false;
    }

    public void SetPlayerTurn() {
        textMeshPro.text = _playerTurn;
        _isPlayerTurn = true;
        _isEnemyTurn = false;
    }

    public void SetPlayerWin() {
        textMeshPro.text = _win;
        _isPlayerWin = true;
    }

    public void SetPlayerLose() {
        textMeshPro.text = _lose;
        _isPlayerLose = true;
    }

    public bool EnemyTurn {
        get { return _isEnemyTurn; }
    }

    public bool PlayerTurn {
        get { return _isPlayerTurn; }
    }

    public bool PlayerWin {
        get { return _isPlayerWin; }
    }

    public bool PlayerLose {
        get { return _isPlayerLose; }
    }
}
