using System;
using System.IO;
using UnityEngine;

public class JSONSaveSystem : ISaveSystem
{
    private readonly string _filePathSavesDirectory;
    private readonly string _fileNameAutoSave;
    private readonly string _fileNameDirectory;
    public JSONSaveSystem ()
    {
        _filePathSavesDirectory = Application.persistentDataPath;
        _fileNameAutoSave = "/Save.json";
        _fileNameDirectory = "/Saves";
        System.IO.Directory.CreateDirectory(_filePathSavesDirectory + _fileNameDirectory);
    }

    public void Save(SaveData saveData, bool isAutoSave, string fileName = "") {
        var json = JsonUtility.ToJson(saveData);
        using (var writer = new StreamWriter(GetPathSaveDirectory(isAutoSave, fileName)))
        {
            writer.WriteLine(json);
        }
    }

    public SaveData Load(bool isAutoSave, string fileName = "") {
        string json = "";
        using (var reader = new StreamReader(GetPathSaveDirectory(isAutoSave, fileName)))
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

    public string GetPathSaveDirectory(bool isAutoSave, string fileName = "/Save.json") {
        if (isAutoSave)
            return _filePathSavesDirectory + _fileNameAutoSave;
        else 
            return _filePathSavesDirectory + _fileNameDirectory + fileName;
    }
}
