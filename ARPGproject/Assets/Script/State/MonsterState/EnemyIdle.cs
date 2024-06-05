using PlayerController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdle : BaseState
{
    private float idleTime;
    private float maxIdleTime;
    public EnemyIdle(UnitController controller) : base(controller)
    {

    }

    public override void OnEnterState()
    {
        
        controller.ChangeAnimation("Idle", 0.2f, 1f);
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
                if (controller.nearUnitTransform != null)
                {
                    controller.stateMachine.ChangeState(UnitState.ENEMY_PATROL);         
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
