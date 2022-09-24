using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    private Item item;

    public Image icon;


    void Start()
    {  
    }

    void Update()
    {

    }

    public void AddItem(Item newItem)
    {
        item = newItem;
        icon.sprite = item.icon;
        icon.enabled = true;
    }

    public void ClearSlot()
    {   
        //item = null;   
        //icon.sprite = null; 
        //icon.enabled = false;
    }
}
