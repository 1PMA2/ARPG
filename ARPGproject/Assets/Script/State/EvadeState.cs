using Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvadeState : PlayerState
{
    public EvadeState(Player controller) : base(controller)
    {

    }

    public override void OnEnterState()
    {
        playerController.UnitInfo.currentState = UnitState.EVADE;
        playerController.ChangeAnimation("Evade", 0f, 1.5f);
        playerController.DisableWeaponTrigger();
        playerController.DisableSmashTrigger();
        playerController.ExitComnbo();
        playerController.LookForward();
        playerController.StatController.UseStamina(DamageState.evadeStamina);
        playerController.animator.applyRootMotion = true;
        playerController.gameObject.layer = 8;
    }

    public override void OnFixedUpdateState()
    {

    }

    public override void OnUpdateState()
    {
        playerController.animator.speed = 1.5f;

        if (playerController.CheckAnimation())
            playerController.stateMachine.ChangeState(UnitState.IDLE);
    }

    public override void OnLateUpdateState()
    {

    }

    public override void OnExitState()
    {
        playerController.gameObject.layer = 7;
        playerController.SetEquip(true);
    }
}
