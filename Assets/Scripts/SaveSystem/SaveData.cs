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

[Serializable]
public class SaveInfo
{
    public string Id;
}
