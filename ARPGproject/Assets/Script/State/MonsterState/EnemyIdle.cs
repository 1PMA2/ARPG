using Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdle : SkeletonState
{
    private float idleTime;
    private float maxIdleTime;
    public EnemyIdle(Skeleton controller) : base(controller)
    {

    }

    public override void OnEnterState()
    {

        skeletonController.ChangeAnimation("Idle", 0.2f, 1f);
        idleTime = 0;

        maxIdleTime = Random.Range(2f, 4f);

        
    }

    public override void OnFixedUpdateState()
    {

    }

    public override void OnUpdateState()
    {

            idleTime += Time.deltaTime;

            if (idleTime >= maxIdleTime)
            {
                if (skeletonController.nearUnitTransform != null)
                {
                    skeletonController.stateMachine.ChangeState(UnitState.ENEMY_PATROL);         
                }
            }
    }

    public override void OnLateUpdateState()
    {

    }

    public override void OnExitState()
    {
        
    }
}
