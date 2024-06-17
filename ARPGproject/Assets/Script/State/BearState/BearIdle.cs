using Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearIdle : BearState
{
    private float idleTime;
    private float maxIdleTime;
    public BearIdle(Bear controller) : base(controller)
    {

    }

    public override void OnEnterState()
    {

        bearController.ChangeAnimation("Idle", 0.2f, 1f);
        idleTime = 0;

        maxIdleTime = Random.Range(2f, 4f);

        
    }

    public override void OnFixedUpdateState()
    {

    }

    public override void OnUpdateState()
    {   

        //idleTime += Time.deltaTime;

        //if (idleTime >= maxIdleTime)
        //{
        //    if (bearController.nearUnitTransform != null)
        //    {
        //        bearController.stateMachine.ChangeState(UnitState.ENEMY_PATROL);         
        //    }
        //}
    }

    public override void OnLateUpdateState()
    {

    }

    public override void OnExitState()
    {
        
    }
}
