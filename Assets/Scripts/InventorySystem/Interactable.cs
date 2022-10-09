using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//[RequireComponent(typeof())]
public class Interactable : MonoBehaviour
{
    private float radius = 3f;
    public Transform interactableObject;

    public bool isFocus = false;
    Transform player;

    bool hasInteracted = false;

    void Update()
    {
        if (isFocus)
        {
            float distance = Vector3.Distance(player.position, interactableObject.position);

            if (!hasInteracted && distance <= radius)
            {
                hasInteracted = true;
                Interact();
            }
        }
    }

    public void OnFocused(Transform playerTransform)
    {
        isFocus = true;
        hasInteracted = false;
        player = playerTransform;
    }

    public void OnDeFocused()
    {
        isFocus = false;
        hasInteracted = true;
        player = null;
    }

    public virtual void Interact()
    {

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(interactableObject.transform.position, radius);
    }
}
