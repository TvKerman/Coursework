using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderController : MonoBehaviour
{

    private bool isActriveToPress = false;
    private bool isExitCollider = false;
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
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Button")
        {
            isActriveToPress = true;
            isExitCollider = true;
        }
    }
}
