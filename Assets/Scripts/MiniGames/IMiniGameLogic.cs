using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMiniGameLogic  
{
    public void GameLogic();

    public void InitMiniGame();

    public bool isEndMiniGame { get; }
}
