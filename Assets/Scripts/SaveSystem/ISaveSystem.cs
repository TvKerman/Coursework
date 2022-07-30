using System;
using System.Collections.Generic;

public interface ISaveSystem {

    IEnumerable<string> GetAll { get; }

    public void Save(SaveData saveData, bool isAutoSave, string fileName = "");

    public SaveData Load(bool isAutoSave, string fileName = "");

    public string GetPathSaveDirectory(bool isAutoSave, string fileName);

    void DeleteSave(string filePath);
}
