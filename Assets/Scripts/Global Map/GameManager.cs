using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private GameObject _Player;
    private List<GameObject> _npc = new List<GameObject>();
    private ISaveSystem _saveSystem;

    private LoadScene loadScene;

    private bool _isPlayerNotLose = true;
    private bool _isPlayerNotWin = true;

    void Start()
    {
        _Player = GameObject.FindGameObjectWithTag("Player");
        var array = GameObject.FindGameObjectsWithTag("NPC");
        foreach (var npc in array) { 
            _npc.Add(npc);
        }
        _saveSystem = new JSONSaveSystem();
   
        LoadState();
    }


    void Update()
    {
        if (!_isPlayerNotLose) {
            Debug.Log("Поплачь...");
        }
        if (!_isPlayerNotWin) {
            Debug.Log("МегаХорош. Сильно не радуйся");
        }
    }

    public void SaveState() { 
        SaveData saveData = new SaveData();
        for (int i = 0; i < _npc.Count; i++) {
            saveData.npc.Add(new NPCData());
        }

        _Player.GetComponent<PlayerState>().SaveState(ref saveData);
        foreach (var npc in _npc) {
            npc.GetComponent<NPCState>().SaveState(ref saveData);
        }

        _saveSystem.AutoSave(saveData);
    }

    private void LoadState() {
        SaveData saveData = _saveSystem.LoadAutoSave();

        _Player.GetComponent<PlayerState>().LoadState(saveData);
        foreach (var npc in _npc) {
            npc.GetComponent<NPCState>().LoadState(saveData);
        }
    }

    public void LoadScene(int key) {
        SaveState();
        SceneManager.LoadSceneAsync(key);
    }

    public bool PlayerNotLose {
        get { return _isPlayerNotLose; }
        set { _isPlayerNotLose = value; }
    }

    public bool PlayerNotWin {
        get { return _isPlayerNotWin; }
        set { _isPlayerNotWin = value; }
    }
}
