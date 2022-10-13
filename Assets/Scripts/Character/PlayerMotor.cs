using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class PlayerMotor : MonoBehaviour
{
    NavMeshAgent agent;
    Transform target;

    private const float slowSpeed = 2f;
    private const float slowAngSpeed = 100f;
    private const float defSpeed = 3.5f;
    private const float defAngSpeed = 600f;

    public NavMeshAgent Agent { get { return agent; } }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = defSpeed;
        agent.angularSpeed = defAngSpeed;
    }

    void Update()
    {
        if (target != null)
        {
            MoveTo(target.position);
            FaceOnTarget();
        }
    }

    public void MoveTo(Vector3 position)
    {
        agent.SetDestination(position);
    }

    public void Warp(Vector3 pos) {
        agent.Warp(pos);
    }

    public void FollowTarget(Transform newTarget)
    {
        agent.stoppingDistance = 1f;
        agent.updateRotation = false;

        target = newTarget;
    }

    public void StopFollowingTarget()
    {
        agent.stoppingDistance = 0f;
        agent.updateRotation = true;

        target = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Swamp"))
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

    private void FaceOnTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5f);
    }
}
