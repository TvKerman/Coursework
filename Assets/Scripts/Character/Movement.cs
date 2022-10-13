using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(PlayerMotor))]
public class Movement : MonoBehaviour
{
    private Camera mainCam;
    public LayerMask movementMask;
    private Vector3 hitPoint;
    private PlayerMotor motor;

    private Animator animator;
    private Interactable focus;

    Vector3 positionToMove;


    private bool _isPlayerCanMove = true;
    private bool isPause = false;

    void Start()
    {
        mainCam = Camera.main;
        animator = GetComponentInChildren<Animator>();
        motor = GetComponent<PlayerMotor>();
        positionToMove = new Vector3(0, 0, 0);
    }

    void Update()
    {

        if (Input.GetMouseButton(0) && _isPlayerCanMove)
        {
            if (focus != null)
            {
                RemoveFocus();
            }

            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100, movementMask) && !isPause)
            {
                animator.SetBool("Move", true);
                positionToMove = hit.point;
                motor.MoveTo(positionToMove);
                Interactable interactable = hit.transform.GetComponent<Interactable>();
                if (interactable != null)
                {
                    SetFocus(interactable);
                }
            }    
        }

        if (Mathf.Abs(motor.transform.position.x - positionToMove.x) < 1 &&
                Mathf.Abs(motor.transform.position.y - positionToMove.y) < 1 &&
                Mathf.Abs(motor.transform.position.z - positionToMove.z) < 1)
        {
            animator.SetBool("Move", false);
        }
    }

   private void SetFocus(Interactable newFocus)
   {
       if (newFocus != null)
       {
           if (focus != null)
           {
               focus.OnDeFocused();
           }
   
           focus = newFocus;
           motor.FollowTarget(newFocus.gameObject.transform);
       }
       newFocus.OnFocused(transform);
   }

   private void RemoveFocus()
   {
       if (focus != null)
       {
           focus.OnDeFocused();
       }
       focus = null;
       motor.StopFollowingTarget();
   }

    public void PauseIsActive() {
        isPause = true;
    }

    public void PauseIsOver() {
        isPause = false;
    }

    public void StopPlayer() {
        motor.MoveTo(gameObject.transform.position);
    }

    public void StopPlayer(Vector3 pos) { 
        gameObject.transform.position = pos;
        motor.Warp(pos);
    }

    public bool PlayerCanMove {
        get { return _isPlayerCanMove; }
        set { _isPlayerCanMove = value; }
    }
}
