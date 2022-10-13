using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAnimator : MonoBehaviour
{
    private const float locomationAnimationSmoothTime = 0.1f;

    [SerializeField] private Animator animator;
    [SerializeField] private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        float speedPercent = agent.velocity.magnitude / agent.speed;
        animator.SetFloat("SpeedPercent", speedPercent, locomationAnimationSmoothTime, Time.deltaTime);
    }
}
