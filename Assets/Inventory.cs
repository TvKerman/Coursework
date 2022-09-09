using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Prikol

    public static Inventory instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More then one inventory instance");
            return;
        }
        instance = this;
    }

    #endregion

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallBack;

    private List<Item> inventory = new List<Item>();
    private int maxSpace = 30;

    public bool Add(Item newItem)
    {
        if (inventory.Count <= maxSpace)
        {
            inventory.Add(newItem);
            if (onItemChangedCallBack != null)
            {
                onItemChangedCallBack.Invoke();
            } 
            return true;
        }
        else
        {
            Debug.Log("Not enough space!");
            return false;
        }
    }

    public void Remove(Item itemToRemove)
    {
        inventory.Remove(itemToRemove);
    }
}
