using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New item", menuName = "Inventory/Item" )]
public class Item : ScriptableObject
{
    new public string name = "New Item";
    public Sprite icon = null;
    public bool ShowInInventory = true;

    public virtual void Use()
    {
    }

    public void RemoveFromInventory()
    {
        Inventory.instance.Remove(this);
    }
}
