using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderController : MonoBehaviour
{

    private bool isActriveToPress = false;
    private bool isExitCollider = false;

    private Scroller _scroller;

    private void Start()
    {
        _scroller = FindObjectOfType<Scroller>();
    }

    public bool IsActiveToPress
    {
        get
        {
            return isActriveToPress;
        }
        set
        {
            isActriveToPress = value;
        }
    }

    public bool IsExitCollider
    {
        get { return isExitCollider; }
        set { isExitCollider = value; }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Button")
        {
            isActriveToPress = true;
            isExitCollider = false;
            other.tag = "Tirget Button";
            _scroller.TirgetButtons.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Tirget Button")
        {
            isActriveToPress = true;
            isExitCollider = true;
            _scroller.DeleteButtons.Add(other.gameObject);
        }
    }
}
