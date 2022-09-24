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

    public List<Item> items = new List<Item>();
    private int maxSpace = 5;

    public void Add(Item newItem)
    {
        if (items.Count >= maxSpace)
        {
            Debug.Log("Not enough room.");
            return;
        }

        items.Add(newItem);

        if (onItemChangedCallBack != null)
        {
            onItemChangedCallBack.Invoke();
        }
    }

    public void Remove(Item itemToRemove)
    {
        items.Remove(itemToRemove);

        if (onItemChangedCallBack != null)
        {
            onItemChangedCallBack.Invoke();
        }
    }
}
