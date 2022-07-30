using System;

public interface ISaveSystem {
    
    public void Save(SaveData saveData, bool isAutoSave, string fileName = "");

    public SaveData Load(bool isAutoSave, string fileName = "");

    public string GetPathSaveDirectory(bool isAutoSave, string fileName);
}
