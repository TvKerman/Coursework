using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavePanel : MonoBehaviour
{
    [SerializeField]
    private Button _saveBtn;
    [SerializeField]
    private Button _loadBtn;
    [SerializeField]
    private Button _deleteAllBtn;
    [SerializeField]
    private SaveItem _itemPrefab;
    [SerializeField]
    private GameObject _panel;
    [SerializeField]
    private Transform _container;

    public event Action SaveRequested;
    public event Action<string> LoadRequested;

    private ISaveSystem _saveSystem;
    private List<SaveItem> _items = new List<SaveItem>();

    private void Awake()
    {
        _saveBtn.onClick.AddListener(OnSaveBtnClicked);
        _loadBtn.onClick.AddListener(OnLoadBtnClicked);
        _deleteAllBtn.onClick.AddListener(OnDeleteAllBtnClicked);
        _panel.gameObject.SetActive(false);
    }

    public void SetSaver(ISaveSystem saveSystem)
    {
        _saveSystem = saveSystem;
        foreach (var save in _saveSystem.GetAll)
        {
            string fileName = save.Remove(0, Application.persistentDataPath.Length + "/Saves/".Length);
            
            Debug.Log(fileName);
            var saveData = _saveSystem.Load(false, fileName);
            Add(saveData.Info);
        }
    }

    private void OnSaveBtnClicked()
    {
        SaveRequested();
    }

    public void Add(SaveInfo save)
    {   
        var item = Instantiate(_itemPrefab, _itemPrefab.transform.position + new Vector3(0, -50 * _items.Count, 0) , _container.rotation, _container);
        item.gameObject.SetActive(true);
        item.Init(save.Id);
        _items.Add(item);
        item.Selected += OnItemSelected;
        item.Deleted += OnItemDeleted;
    }

    private void OnLoadBtnClicked()
    {
        _panel.gameObject.SetActive(true);
    }

    private void OnItemSelected(SaveItem item)
    {
        LoadRequested?.Invoke(item.Id);
        _panel.gameObject.SetActive(false);
    }

    private void OnDeleteAllBtnClicked()
    {
        foreach (var item in _items)
        {
            DeleteItem(item, false);
        }
        _items.Clear();
    }

    private void OnItemDeleted(SaveItem item)
    {
        DeleteItem(item, true);
    }

    private void DeleteItem(SaveItem item, bool removeFromList)
    {
        item.Deleted -= OnItemDeleted;
        item.Selected -= OnItemSelected;

        bool isFlag = false;
        foreach (var _item in _items) {
            if (isFlag) {
                _item.transform.position = new Vector3(_item.transform.position.x, _item.transform.position.y + 50, _item.transform.position.z);
            }
            
            if (_item == item) {
                isFlag = true;
            }

        }

        if (removeFromList)
        {
            _items.Remove(item);
        }

        Destroy(item.gameObject);
        _saveSystem.DeleteSave(item.Id);
    }

    public void ClosePanel() {
        
        _panel.gameObject.SetActive(false);
    }
}
