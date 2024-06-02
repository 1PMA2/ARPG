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

        controller.animator.Play("GuardHit", 0, 0);

        controller.ChangeAnimation("GuardHit", 0f, 0.8f);

        controller.animator.applyRootMotion = true;
    }

    public override void OnFixedUpdateState()
    {

    }

    public override void OnUpdateState()
    {
        if (controller.IsSmash())
        {
            
            controller.stateMachine.ChangeState(UnitState.SMASH_01);
            controller.IsCounter = controller.UnitInfo.MaxCounter;
            controller.IsCounter -= 1;
            return;
        }

        if (controller.CheckAnimation())
        {
            controller.stateMachine.ChangeState(UnitState.IDLE);
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
