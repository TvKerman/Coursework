using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region SingleTon

    public static Inventory instance;

    private void Awake()
    {
        instance = this;   
    }

    #endregion

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangerdCallBack;

    public int maxSpace = 10;


    public List<Item> items = new List<Item>();

    public void Add(Item item)
    {
        if (item.ShowInInventory)
        {
            if (items.Count >= maxSpace)
            {
                Debug.Log("sosi zalupu");
                return;
            }

            items.Add(item);

            if (onItemChangerdCallBack != null)
            {
                onItemChangerdCallBack.Invoke();
            }
        }
    }

    public void Remove(Item item)
    {
        if (item != null)
        {
            items.Remove(item);
        }

        if (onItemChangerdCallBack != null)
        {
            onItemChangerdCallBack.Invoke();
        } 
    }
}
