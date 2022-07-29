using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData {
    public MovementData MovementData;
}

[Serializable]
public class MovementData {
    public Vector3 position;
    //public Quaternion rotation;
    public Vector3 point;
}

