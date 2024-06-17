using Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : SkeletonState
{
    public EnemyMove(Skeleton controller) : base(controller)
    {

    }

    public override void OnEnterState()
    {

        skeletonController.ChangeAnimation("Walk", 0.2f, 1f);

    }

    public override void OnFixedUpdateState()
    {

    }

    public override void OnUpdateState()
    {
        if(skeletonController.nearUnitTransform == null)
        {
            skeletonController.stateMachine.ChangeState(UnitState.ENEMY_IDLE);
            return;
        }

        if (Vector3.Distance(skeletonController.transform.position, skeletonController.nearUnitTransform.position) < 2f)
        {
            skeletonController.stateMachine.ChangeState(UnitState.ENEMY_ATTACK);
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
        

        if (skeletonController.nearUnitTransform != null)
        {
            Vector3 direction = (skeletonController.nearUnitTransform.position - skeletonController.transform.position).normalized;
            skeletonController.characterController.Move(direction * Time.deltaTime * 3.0f); // 이동 속도

            Quaternion targetRotation = Quaternion.LookRotation(skeletonController.nearUnitTransform.position - skeletonController.transform.position);
            skeletonController.transform.rotation = Quaternion.Slerp(skeletonController.transform.rotation, targetRotation, 2f * Time.deltaTime);
        }

        
    }
}
