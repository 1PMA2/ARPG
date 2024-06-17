using Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardState : PlayerState
{
    public GuardState(Player controller) : base(controller)
    {

    }

    public override void OnEnterState()
    {
        playerController.UnitInfo.currentState = UnitState.GUARD_01;
        playerController.ChangeAnimation("Guard", 0.1f, 1f);
        playerController.IsAnimationStart();
        playerController.DisableWeaponTrigger();
        playerController.DisableSmashTrigger();
        playerController.LookForward();
        playerController.animator.applyRootMotion = false;
    }

    public override void OnFixedUpdateState()
    {

    }

    public override void OnUpdateState()
    {
        if (playerController.CheckAnimation())
        {
            playerController.stateMachine.ChangeState(UnitState.IDLE);
            return;
        }

    }

    public override void OnLateUpdateState()
    {

    }

    public override void OnExitState()
    {
        playerController.StartGuardCooldown();
        playerController.SetEquip(true);
    }
}
