using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public MovementData movementData;

    public SaveInfo Info;
}

[Serializable]
public class MovementData
{
    public Vector3 position;
    public Vector3 hitPoint;
}

[Serializable]
public class SaveInfo
{
    public string Id;
}
