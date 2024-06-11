using PlayerController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvadeState : BaseState
{
    public EvadeState(UnitController controller) : base(controller)
    {

    }

    public override void OnEnterState()
    {
        controller.ExitComnbo();
        controller.DisableSmashTrigger();
        controller.DisableWeaponTrigger();

        controller.UnitInfo.currentState = UnitState.EVADE;
        controller.LookForward();

        controller.ChangeAnimation("Evade", 0f, 1.5f);

        controller.animator.applyRootMotion = true;

        controller.gameObject.layer = 8;

        controller.StatController.UseStamina(DamageState.evadeStamina);
    }

    public override void OnFixedUpdateState()
    {

    }

    public override void OnUpdateState()
    {
        controller.animator.speed = 1.5f;

        if (controller.CheckAnimation())
            controller.stateMachine.ChangeState(UnitState.IDLE);
    }

    public override void OnLateUpdateState()
    {

    }

    public override void OnExitState()
    {
        controller.gameObject.layer = 7;
        controller.SetEquip(true);
    }
}
