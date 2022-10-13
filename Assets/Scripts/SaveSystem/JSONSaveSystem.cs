using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;

public class JSONSaveSystem : ISaveSystem
{
    private readonly string _filePathSavesDirectory;
    private readonly string _fileNameAutoSave;
    public readonly string _fileNameDirectory;
    public JSONSaveSystem()
    {
        _filePathSavesDirectory = Application.persistentDataPath;
        _fileNameAutoSave = "/Autosave.json";
        _fileNameDirectory = "/Saves/";
        System.IO.Directory.CreateDirectory(_filePathSavesDirectory + _fileNameDirectory);
    }

    public IEnumerable<string> GetAll => Directory.GetFiles(_filePathSavesDirectory + _fileNameDirectory);

    public void Save(SaveData saveData, string fileName) {
        Debug.Log(fileName);
        saveData.Info.name = fileName;
        var json = JsonUtility.ToJson(saveData);
        using (var writer = new StreamWriter(GetPathSaveDirectory(false, fileName)))
        {
            writer.WriteLine(json);
        }
    }

    public SaveData Load(string fileName) {
        string json = "";
        using (var reader = new StreamReader(GetPathSaveDirectory(false, fileName)))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                json += line;
            }
        }

        if (string.IsNullOrEmpty(json))
        {
            return new SaveData();
        }

        return JsonUtility.FromJson<SaveData>(json);
    }

    public void AutoSave(SaveData saveData) {
        saveData.Info.name = _fileNameAutoSave;
        var json = JsonUtility.ToJson(saveData);
        using (var writer = new StreamWriter(GetPathSaveDirectory(false, _fileNameAutoSave)))
        {
            writer.WriteLine(json);
        }
    }

    public SaveData LoadAutoSave() {
        string json = "";
        using (var reader = new StreamReader(GetPathSaveDirectory(false, _fileNameAutoSave)))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                json += line;
            }
        }

        if (string.IsNullOrEmpty(json))
        {
            return new SaveData();
        }

        return JsonUtility.FromJson<SaveData>(json);
    }

    public bool SavingExists() {
        return File.Exists(GetPathSaveDirectory(true, _fileNameAutoSave));
    }

    public SaveData CreateStartSave() {
        SaveData saveData = new SaveData();
        saveData.Info.name = _fileNameAutoSave;

        saveData.playerData.isPlayerCanMove = true;
        saveData.playerData.position = new Vector3(61.6f, 9.2f, 117.18f);
        saveData.playerData.rotation = new Vector3(0, 0, 0);
        saveData.playerData.cameraPosition = new Vector3(61.6f, 14.2f, 112.18f);
        saveData.playerData.cameraRotation = new Vector3(41.696f, 0, 0);
        saveData.playerData.isPlayerNotLose = true;
        saveData.playerData.isPlayerNotWin = true;

        // saveData.npc ...
        for (int i = 0; i < 3; i++) {
            saveData.npc.Add(new NPCData());
            saveData.npc[i].isActive = true;
        }

        saveData.npc[0].position = new Vector3(160.7747f, 15.51929f, 109.2277f);
        saveData.npc[0].rotation = new Vector3(0, 210, 0);
        saveData.npc[0].centerTriger = new Vector3(-0.003216982f, 0.7288874f, -0.05846584f);
        saveData.npc[0].sizeTriger = new Vector3(4.309021f, 2.327804f, 3.010195f);

        saveData.npc[1].position = new Vector3(200.1623f, 25.12322f, 170.0776f);
        saveData.npc[1].rotation = new Vector3(0, 120, 0);
        saveData.npc[1].centerTriger = new Vector3(0.08932853f, 0.7288874f, -0.1614792f);
        saveData.npc[1].sizeTriger = new Vector3(6.38472f, 2.327804f, 7.267451f);

        saveData.npc[2].position = new Vector3(222.8189f, 22.6938f, 174.5726f);
        saveData.npc[2].rotation = new Vector3(0, -55, 0);
        saveData.npc[2].centerTriger = new Vector3(0.1536164f, 0.7288874f, 0.002384539f);
        saveData.npc[2].sizeTriger = new Vector3(7.255716f, 2.327804f, 5.515227f);

        return saveData;
    }

    public string GetPathSaveDirectory(bool isAutoSave, string fileName) {
        if (isAutoSave)
            return _filePathSavesDirectory + _fileNameAutoSave;
        else 
            return _filePathSavesDirectory + _fileNameDirectory + fileName;
    }

    public void DeleteSave(string filePath)
    {
        Debug.Log(filePath);
        filePath = GetPathSaveDirectory(false, filePath);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }
}
