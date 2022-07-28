using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{
    private Camera mainCam;
    private NavMeshAgent agent;

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
            RaycastHit hit;
            if (Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out hit))
            {
                agent.SetDestination(hit.point);  
            }
        }
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
