using Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : SkeletonState
{
    public EnemyAttack(Skeleton controller) : base(controller)
    {

    }

    public override void OnEnterState()
    {
        
        skeletonController.ChangeAnimation("Attack", 0.2f, 0.5f);

    }

    public override void OnFixedUpdateState()
    {

    }

    public override void OnUpdateState()
    {
        LookAtPlayer();

        if (skeletonController.CheckAnimation())
            skeletonController.stateMachine.ChangeState(UnitState.ENEMY_IDLE);

    }

    public override void OnLateUpdateState()
    {

    }

    public override void OnExitState()
    {
        
    }

    private void LookAtPlayer()
    {
        if (skeletonController.nearUnitTransform != null)
        {
            Quaternion targetRotation = Quaternion.LookRotation(skeletonController.nearUnitTransform.position - skeletonController.transform.position);
            skeletonController.transform.rotation = Quaternion.Slerp(skeletonController.transform.rotation, targetRotation, 2f * Time.deltaTime);
        }
    }
}
