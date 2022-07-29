using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public MovementData movementData;
}

[Serializable]
public class MovementData
{
    public Vector3 position;
    public Vector3 hitPoint;
}