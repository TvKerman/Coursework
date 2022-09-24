using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Transform itemParents;
    public GameObject inventoryUI;

    Inventory inventory;


    void Start()
    {
        inventory = Inventory.instance;
        inventory.onItemChangedCallBack += UpdateUI;
    }

    void Update()
    { 
        //if (Input.GetButtonDown("Inventory"))
        //{
          //  inventoryUI.SetActive(!inventoryUI.activeSelf);
           // UpdateUI();
       // }
    }

    void UpdateUI()
    {
        InventorySlot[] slots = GetComponentsInChildren<InventorySlot>();

        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
}
