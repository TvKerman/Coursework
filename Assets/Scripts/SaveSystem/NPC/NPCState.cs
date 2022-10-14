using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCState : MonoBehaviour, IStateSave
{
    [SerializeField] private bool isOnHellRegion;
    [SerializeField] private int keyCode;
    [SerializeField] private BattleData battleData;
    public void SaveState(ref SaveData saveData) {
        saveData.npc[keyCode].position = gameObject.transform.position;
        saveData.npc[keyCode].rotation = gameObject.transform.localEulerAngles;
        saveData.npc[keyCode].centerTriger = gameObject.GetComponent<BoxCollider>().center;
        saveData.npc[keyCode].sizeTriger = gameObject.GetComponent<BoxCollider>().size;

        saveData.npc[keyCode].isActive = gameObject.activeSelf;
        if (gameObject.GetComponent<NPCLogic>().StartBattle) {
            saveData.battleData = this.battleData;
            saveData.battleData.isOnHellRegion = isOnHellRegion;
        }
    }

    public void LoadState(SaveData saveData) {
        gameObject.SetActive(saveData.npc[keyCode].isActive);

        gameObject.transform.position = saveData.npc[keyCode].position;
        gameObject.transform.localEulerAngles = saveData.npc[keyCode].rotation;
        gameObject.GetComponent<BoxCollider>().center = saveData.npc[keyCode].centerTriger;
        gameObject.GetComponent<BoxCollider>().size = saveData.npc[keyCode].sizeTriger;
    }
}
