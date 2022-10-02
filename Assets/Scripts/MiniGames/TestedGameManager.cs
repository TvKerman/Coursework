using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestedGameManager : MonoBehaviour
{
    private IMiniGameLogic miniGameLogic;

    void Start()
    {
        IMiniGameLogic miniGame1 = FindObjectOfType<SpawnCircle>();
        IMiniGameLogic miniGame2 = FindObjectOfType<Scroller>();
        if (miniGame1 != null)
            miniGameLogic = miniGame1;
        if (miniGame2 != null)
            miniGameLogic = miniGame2;
        miniGameLogic.InitMiniGame();
    }

    void Update()
    {
        miniGameLogic.GameLogic();
        if (miniGameLogic.isEndMiniGame) {
            miniGameLogic.InitMiniGame();
        }
    }
}
