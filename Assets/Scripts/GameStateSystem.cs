using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateSystem : MonoBehaviour
{
    private ISaveSystem _saveSystem;

    

    void Start()
    {
        _saveSystem = new JSONSaveSystem();

    }

    private SaveData GetSaveData() {
        SaveData data = new SaveData();

        return data;
    }

    private void SetSaveData(SaveData data) {

    }

    public void SaveGame() {
        _saveSystem.Save(GetSaveData());
    }

    public void LoadGame() {
        SetSaveData(_saveSystem.Load());
    }
}
