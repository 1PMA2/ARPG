using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnitController;

public class IdleBehaviour : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        //bool isMove = Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) ||
        //       Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow);

        //if (isMove) { animator.SetInteger("State", (int)UnitState.MOVE); }
        //else { animator.SetInteger("State", (int)UnitState.IDLE); }

        //if (Input.GetKey("d"))
        //{
        //    animator.SetInteger("State", (int)UnitState.SMASH_START);
        //}
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("exit");
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
