using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadAnimation : StateMachineBehaviour
{
    private double _timer= 1.5d;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_timer < stateInfo.normalizedTime)
            if (animator.gameObject.GetComponent<DynamicBattlePrototype.Unit>() != null)
                animator.gameObject.GetComponent<DynamicBattlePrototype.Unit>().isDeadUnit = true;
            else if (animator.GetComponentInParent<TurnBasedBattleSystemFromRomchik.Unit>() != null)
                animator.GetComponentInParent<TurnBasedBattleSystemFromRomchik.Unit>().Dead();
    }


    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    animator.gameObject.GetComponent<DynamicBattlePrototype.Unit>().isDeadUnit = true;   
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //    
    //}

    
}
