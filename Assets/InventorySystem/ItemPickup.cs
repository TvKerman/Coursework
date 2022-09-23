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

    private void PickUp()
    {
        if (item != null)
        {
            Debug.Log($"pick up {item.name}");
            Inventory.instance.Add(item);

            Destroy(gameObject);
        }
    }
}
