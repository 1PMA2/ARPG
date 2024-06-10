using PlayerController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardState : BaseState
{
    public GuardState(UnitController controller) : base(controller)
    {

    }

    public override void OnEnterState()
    {
        controller.animator.applyRootMotion = false;
        controller.DisableSmashTrigger();
        controller.DisableWeaponTrigger();

        controller.UnitInfo.currentState = UnitState.GUARD_01;

        controller.LookForward();

        controller.IsAnimationStart();
        controller.ChangeAnimation("Guard", 0.1f, 1f);
    }

    public override void OnFixedUpdateState()
    {

    }

    public override void OnUpdateState()
    {
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
        controller.StartGuardCooldown();
        controller.SetEquip(true);
    }
}
