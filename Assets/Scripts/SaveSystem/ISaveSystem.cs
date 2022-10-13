using System;
using System.Collections.Generic;

public interface ISaveSystem {

    IEnumerable<string> GetAll { get; }

    public void Save(SaveData saveData, string fileName);

    public SaveData Load(string fileName);

    public void AutoSave(SaveData saveData);

    public SaveData LoadAutoSave();

    public bool SavingExists();

    public SaveData CreateStartSave();

    public string GetPathSaveDirectory(string fileName);

    void DeleteSave(string filePath);
}
