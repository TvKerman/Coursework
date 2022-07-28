using System;

public interface ISaveSystem {
    
    public void Save(SaveData saveData);


    
    public SaveData Load();

}
