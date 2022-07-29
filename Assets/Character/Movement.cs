using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{
    private Camera mainCam;
    private NavMeshAgent agent;
    private RaycastHit hit;

    private Vector3 hitPoint;

    private bool isOnSwamp = false;

    private const float slowSpeed = 3f;
    private const float slowAngSpeed = 50f;
    private const float defSpeed = 8f;
    private const float defAngSpeed = 360f;


    void Start()
    {
        mainCam = Camera.main;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = 8;
    }

    void Update()
    {
        if (Input.GetMouseButton(0)) 
        {
            //RaycastHit hit;
            if (Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out hit))
            {
                hitPoint = hit.point;
                agent.SetDestination(hit.point);  
            }
        }
    }

    public MovementData GetMovementData() {
        Debug.Log($"{hitPoint.x}, {hitPoint.y}, {hitPoint.z}");
        return new MovementData() {position = transform.position, point = hitPoint};
    }

    public void SetMovementData(MovementData data) {
        transform.position = data.position;
        agent.SetDestination(data.point);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Swamp"))
        {
            agent.speed = slowSpeed;
            agent.angularSpeed = slowAngSpeed;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Swamp"))
        {
            agent.speed = defSpeed;
            agent.angularSpeed = defAngSpeed;
        }
    }
}
