using System;
using System.IO;
using UnityEngine;

public class JSONSaveSystem : ISaveSystem
{
    private readonly string _filePath;

    public JSONSaveSystem ()
    {
        _filePath = Application.persistentDataPath + "/Save.json";
    }

    public void Save(SaveData saveData) {
        var json = JsonUtility.ToJson(saveData);
        using (var writer = new StreamWriter(_filePath))
        {
            writer.WriteLine(json);
        }
        Debug.Log(_filePath);
    }

    public SaveData Load() {
        string json = "";
        using (var reader = new StreamReader(_filePath))
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
}
