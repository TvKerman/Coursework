using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStateSave
{
    public void SaveState(ref SaveData saveData);

    public void LoadState(SaveData saveData);
}
