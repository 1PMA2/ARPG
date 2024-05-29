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
        controller.LookForward();

        controller.ChangeAnimation("Guard", 0.2f, 1.5f);
    }

    public override void OnFixedUpdateState()
    {

    }

    public override void OnUpdateState()
    {
        if (!Input.GetKey("a"))
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
