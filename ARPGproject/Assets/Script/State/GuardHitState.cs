using PlayerController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardHitState : BaseState
{
    public GuardHitState(UnitController controller) : base(controller)
    {

    }

    public override void OnEnterState()
    {
        controller.UnitInfo.currentState = UnitState.GUARD_02;

        controller.ChangeAnimation("GuardHit", 0.2f, 1f);

        controller.animator.applyRootMotion = true;

    }

    public override void OnFixedUpdateState()
    {

    }

    public override void OnUpdateState()
    {
        if(controller.CheckAnimation())
        {
            controller.stateMachine.ChangeState(UnitState.IDLE);
            return;
        }

        if (controller.IsSmash())
        {
            controller.stateMachine.ChangeState(UnitState.SMASH_01);
            return;
        }


    }

    public override void OnLateUpdateState()
    {

    }

    public override void OnExitState()
    {
        controller.SetEquip(true);
    }
}
