using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collor : MonoBehaviour
{
    private void OnMouseEnter()
    {
        GetComponent<Renderer>().material.color = Color.green;
    }
    private void OnMouseExit()
    {
        GetComponent<Renderer>().material.color = Color.red;
    }
}
