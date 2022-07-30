using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateSystem : MonoBehaviour
{
    private ISaveSystem _saveSystem;
    private Movement _movement;

    void Start()
    {
        _saveSystem = new JSONSaveSystem();
        _movement = FindObjectOfType<Movement>();
    }

    private SaveData GetSaveData()
    {
        SaveData data = new SaveData();
        data.movementData = _movement.GetMovementData();

        return data;
    }

    private void SetSaveData(SaveData data)
    {
        _movement.SetMovementData(data.movementData);
    }

    public void AutoSave()
    {
        _saveSystem.Save(GetSaveData(), true);
    }

    public void LoadAutoSave()
    {
        SetSaveData(_saveSystem.Load(true));
    }
}
