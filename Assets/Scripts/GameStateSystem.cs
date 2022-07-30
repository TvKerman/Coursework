using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameStateSystem : MonoBehaviour
{
    private ISaveSystem _saveSystem;
    private Movement _movement;

    [SerializeField] private SavePanel savePanel;

    void Start()
    {
        _saveSystem = new JSONSaveSystem();
        _movement = FindObjectOfType<Movement>();

        savePanel.SaveRequested += OnSaveRequested;
        savePanel.LoadRequested += OnLoadRequested;
        savePanel.SetSaver(_saveSystem);
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
        SaveData saveData = GetSaveData();
        saveData.Info = new SaveInfo() {Id = "AutoSave"};
        _saveSystem.Save(saveData, true);
    }

    public void LoadAutoSave()
    {
        SetSaveData(_saveSystem.Load(true));
    }


    private void OnSaveRequested()
    {
        SaveData saveData = GetSaveData();
        saveData.Info = new SaveInfo();
        Debug.Log(DateTime.UtcNow.ToString(
            "ss-mm-hh-dd-MM-yyyy") + ".json");
        _saveSystem.Save(saveData, false, DateTime.UtcNow.ToString(
            "ss-mm-hh-dd-MM-yyyy") + ".json");
        savePanel.Add(saveData.Info);
    }


    private void OnLoadRequested(string save)
    {
        
        SaveData saveData = _saveSystem.Load(false ,save);
        SetSaveData(saveData);
    }
}