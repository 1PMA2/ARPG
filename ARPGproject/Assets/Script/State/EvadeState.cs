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
        controller.LookForward();

        controller.ChangeAnimation("Evade", 0.2f, 1.5f);

        controller.animator.applyRootMotion = true;
    }

    public override void OnFixedUpdateState()
    {

    }

    public override void OnUpdateState()
    {
        if (controller.CheckAnimation())
            controller.stateMachine.ChangeState(UnitState.IDLE);
    }

    public override void OnLateUpdateState()
    {

    }

    public override void OnExitState()
    {
        controller.SetEquip(true);
    }
}
