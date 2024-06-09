using PlayerController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : BaseState
{
    public EnemyAttack(UnitController controller) : base(controller)
    {

    }

    public override void OnEnterState()
    {
        
        controller.ChangeAnimation("Attack", 0.2f, 0.5f);

    }

    public override void OnFixedUpdateState()
    {

    }

    public override void OnUpdateState()
    {
        //LookAtPlayer();

        if (controller.CheckAnimation())
            controller.stateMachine.ChangeState(UnitState.ENEMY_IDLE);

    }

    public override void OnLateUpdateState()
    {

    }

    public override void OnExitState()
    {
        
    }

    private void LookAtPlayer()
    {
        if (controller.nearUnitTransform != null)
        {
            Quaternion targetRotation = Quaternion.LookRotation(controller.nearUnitTransform.position - controller.transform.position);
            controller.transform.rotation = Quaternion.Slerp(controller.transform.rotation, targetRotation, 2f * Time.deltaTime);
        }
    }
}
