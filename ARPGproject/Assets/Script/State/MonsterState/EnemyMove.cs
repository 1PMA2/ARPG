using PlayerController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : BaseState
{

    public EnemyMove(UnitController controller) : base(controller)
    {

    }

    public override void OnEnterState()
    {
        
        controller.ChangeAnimation("Walk", 0.2f, 1f);
    }

    public override void OnFixedUpdateState()
    {

    }

    public override void OnUpdateState()
    {
        if (Vector3.Distance(controller.transform.position, controller.Player.transform.position) < 1.5f)
        {
            controller.stateMachine.ChangeState(UnitState.ENEMY_ATTACK);
        }
        else
        {
            MoveTowardsPlayer();

        }
    }

    public override void OnLateUpdateState()
    {

    }

    public override void OnExitState()
    {
        
    }

    public void MoveTowardsPlayer()
    {
        Vector3 direction = (controller.Player.transform.position - controller.transform.position).normalized;
        controller.characterController.Move(direction * Time.deltaTime * 3.0f); // 이동 속도

        Quaternion targetRotation = Quaternion.LookRotation(controller.Player.transform.position - controller.transform.position);
        controller.transform.rotation = Quaternion.Slerp(controller.transform.rotation, targetRotation, 2f * Time.deltaTime);
    }
}
