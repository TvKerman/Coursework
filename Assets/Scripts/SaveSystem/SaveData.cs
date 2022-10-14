using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public PlayerData playerData = new PlayerData();

    public BattleData battleData;

    public List<NPCData> npc = new List<NPCData>();

    public SaveInfo Info = new SaveInfo();
}

[Serializable]
public class PlayerData
{
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 cameraPosition;
    public Vector3 cameraRotation;

    public bool isPlayerCanMove;
    public bool isPlayerNotLose;
    public bool isPlayerNotWin;
}

[Serializable]
public class NPCData 
{
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 centerTriger;
    public Vector3 sizeTriger;

    public bool isActive;
    //public bool isPlayerInTrigger;
    //public bool isGetMessage;
    //public bool isStartBattle;
    //public bool isButtonCloseDialog;
}

[Serializable]
public class BattleData {
    public int keyCodeNPC;
    public bool isOnHellRegion;

    public List<MeleeEnemyData> meleeEnemies = new List<MeleeEnemyData>();
    public List<RangeEnemyData> rangeEnemies = new List<RangeEnemyData>();

    BattleData() {
        for (int i = 0; i < 3; i++) {
            meleeEnemies.Add(new MeleeEnemyData());
            rangeEnemies.Add(new RangeEnemyData());
        }
    }
}

[Serializable]
public class MeleeEnemyData {
    public bool isActive;

    public int maxHealtPoints;
    public int damage;
    public int initiative;
}

[Serializable]
public class RangeEnemyData {
    public bool isActive;

    public int maxHealthPoints;
    public int damage;
    public int initiative;
}

[Serializable]
public class SaveInfo
{
    public string name;
}
