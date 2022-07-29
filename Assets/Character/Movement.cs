using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{
    private Camera mainCam;
    private NavMeshAgent agent;

    private bool isOnSwamp = false;

    private const float slowSpeed = 1f;
    private const float slowAngSpeed = 50f;
    private const float defSpeed = 3.5f;
    private const float defAngSpeed = 360f;


    void Start()
    {
        mainCam = Camera.main;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = defSpeed;
    }

    void Update()
    {
        if (Input.GetMouseButton(0)) 
        {
            Debug.Log("Click");
            RaycastHit hit;
            if (Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out hit))
            {
                Debug.Log($"{hit.point}");
                agent.SetDestination(hit.point);  
            }
        }
    }

    public Vector3 GetMovementData() {
        return transform.position;
    }

    public void SetMovementData(SaveData data) {
        transform.position = data.position;
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
