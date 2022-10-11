using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public PlayerMovementData movementData;

    public SaveInfo Info;
}

[Serializable]
public class PlayerMovementData
{
    public Vector3 position;
    public bool isPlayerCanMove;
}


public class NPCData 
{
    public Vector3 position;

    public bool isActive;
    public bool isPlayerInTrigger;
    public bool isGetMessage;
    public bool isStartBattle;
    public bool isButtonCloseDialog;
}

[Serializable]
public class SaveInfo
{
    public string Id;
}
