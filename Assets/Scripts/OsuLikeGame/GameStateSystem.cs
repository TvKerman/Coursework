using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameStateSystem : MonoBehaviour
{
    private ISaveSystem _saveSystem;
    //private Movement _movement;

    [SerializeField] private SavePanel savePanel;
    [SerializeField] private GameObject menuPause;

    void Start()
    {
        _saveSystem = new JSONSaveSystem();
        //_movement = FindObjectOfType<Movement>();

 //       savePanel.SaveRequested += OnSaveRequested;
 //       savePanel.LoadRequested += OnLoadRequested;
 //       savePanel.SetSaver(_saveSystem);
    }

    private SaveData GetSaveData()
    {
        SaveData data = new SaveData();
        //data.movementData = _movement.GetMovementData();

        return data;
    }

    private void SetSaveData(SaveData data)
    {
        //_movement.SetMovementData(data.movementData);
    }

    public void AutoSave()
    {
        SaveData saveData = GetSaveData();
        saveData.Info = new SaveInfo() {name = "AutoSave"};
        _saveSystem.AutoSave(saveData);
    }

    public void LoadAutoSave()
    {
        menuPause.SetActive(false);
        //FindObjectOfType<Movement>().PauseIsOver();
        SetSaveData(_saveSystem.LoadAutoSave());
    }


    private void OnSaveRequested()
    {
        SaveData saveData = GetSaveData();
        saveData.Info = new SaveInfo();
        //Debug.Log(DateTime.UtcNow.ToString(
        //    "ss-mm-hh-dd-MM-yyyy") + ".json");
        _saveSystem.Save(saveData, DateTime.UtcNow.ToString(
            "ss-mm-hh-dd-MM-yyyy") + ".json");
        savePanel.Add(saveData.Info);
    }


    private void OnLoadRequested(string save)
    {
        
        SaveData saveData = _saveSystem.Load(save);
        SetSaveData(saveData);
    }
}
