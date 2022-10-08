using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : Interactable
{
    public Item item;
    public override void Interact()
    {
        base.Interact();

        PickUp();
    }


    public void PickUp()
    {
        Debug.Log(item.name);
        Inventory.instance.Add(item);
    
        Destroy(gameObject);
    }

    void Update()
    {
        
    }
}
