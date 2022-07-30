using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;
using System;

public class SaveItem : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Text _name;
    [SerializeField]
    private Button _deleteBtn;

    public event Action<SaveItem> Selected;
    public event Action<SaveItem> Deleted;

    public string Id { get; private set; }

    private void Awake()
    {
        _deleteBtn.onClick.AddListener(OnDeleteBtnPressed);
    }

    public void Init(string id)
    {
        Id = id;
        _name.text = Path.GetFileNameWithoutExtension(id);
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Selected?.Invoke(this);
    }

    private void OnDeleteBtnPressed()
    {
        Deleted?.Invoke(this);
    }
}
