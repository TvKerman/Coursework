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

    private Interactable focus;

    private bool isPause = false;

    void Start()
    {
        mainCam = Camera.main;
        motor = GetComponent<PlayerMotor>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (focus != null)
            {
                RemoveFocus();
            }
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100, movementMask) && !isPause)
            {
                Vector3 positionToMove = hit.point;
                motor.MoveTo(positionToMove);
                Interactable interactable = hit.transform.GetComponent<Interactable>();
                if (interactable != null)
                {
                    SetFocus(interactable);
                }
            }
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

    public MovementData GetMovementData()
    {
        return new MovementData() { position = transform.position, hitPoint = hitPoint };
    }

    public void SetMovementData(MovementData data)
    {
        transform.position = data.position;
        motor.Agent.SetDestination(data.hitPoint);
    }
}
