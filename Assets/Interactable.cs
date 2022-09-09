using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    private Transform interactionTransform;
    public Transform player;

    private float radius = 3f;

    private bool isFocused = false;
    private bool isInteracted = false;
    

    private void Start()
    {
        interactionTransform = GetComponent<Transform>();
    }

    private void Update()
    {
        if (isFocused)
        {
           float distance = Vector3.Distance(player.position, interactionTransform.position);
           if (isInteracted && distance <= 0.5f)
           {
               Debug.Log("INTERACT");
               isInteracted = false;
               Interact();
           }
        }
    }

    public virtual void Interact() {}

    public void OnFocused(Transform playerTransform)
    {
        isFocused = true;
        isInteracted = true;
        player = playerTransform;
    }

    public void OnDeFocused()
    {
        isFocused = false; 
        player = null;  
        isInteracted = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (interactionTransform == null)
        {
            interactionTransform = transform;
        }

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
