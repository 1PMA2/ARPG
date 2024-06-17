using Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardHitState : PlayerState
{
    public GuardHitState(Player controller) : base(controller)
    {

    }

    public override void OnEnterState()
    {
        playerController.UnitInfo.currentState = UnitState.GUARD_02;
        playerController.ChangeAnimation("GuardHit", 0f, 1f);
        playerController.animator.Play("GuardHit", 0, 0);
        playerController.DisableSmashTrigger();
        playerController.DisableWeaponTrigger();
        playerController.animator.applyRootMotion = true;
    }

    public override void OnFixedUpdateState()
    {

    }

    public override void OnUpdateState()
    {
        if (playerController.IsSmash() && playerController.StatController.AbleStamina(DamageState.smashStamina))
        {
            playerController.UnitInfo.currentState = UnitState.COUNTER;
            playerController.stateMachine.ChangeState(UnitState.SMASH_01);
            playerController.IsCounter = playerController.UnitInfo.MaxCounter;
            playerController.IsCounter--;
            return;
        }

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
        playerController.SetEquip(true);
    }
}
