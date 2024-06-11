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
        if(controller.nearUnitTransform == null)
        {
            controller.stateMachine.ChangeState(UnitState.ENEMY_IDLE);
            return;
        }

        if (Vector3.Distance(controller.transform.position, controller.nearUnitTransform.position) < 2f)
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
        

        if (controller.nearUnitTransform != null)
        {
            Vector3 direction = (controller.nearUnitTransform.position - controller.transform.position).normalized;
            controller.characterController.Move(direction * Time.deltaTime * 3.0f); // 이동 속도

            Quaternion targetRotation = Quaternion.LookRotation(controller.nearUnitTransform.position - controller.transform.position);
            controller.transform.rotation = Quaternion.Slerp(controller.transform.rotation, Quaternion.Euler(targetRotation.x, 0f, targetRotation.z), 2f * Time.deltaTime);
        }

        
    }
}
