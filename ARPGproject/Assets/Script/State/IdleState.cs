using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerController
{
    public class IdleState : BaseState
    {
        private int hashMoveAnimation;
        public IdleState(UnitController controller) : base(controller)
        {

        }

        public override void OnEnterState()
        {
            controller.animator.applyRootMotion = false;
        }

        public override void OnFixedUpdateState()
        {

        }

        public override void OnUpdateState()
        {
            if(controller.IsMove())
            {
                controller.stateMachine.ChangeState(UnitState.MOVE);
            }
            else
            {
                controller.Idle();
            }

            if (controller.IsComboAttack())
                controller.stateMachine.ChangeState(UnitState.COMBO_01);

            if (controller.IsSmash())
                controller.stateMachine.ChangeState(UnitState.SMASH_00);

            if(controller.IsGuard())
                controller.stateMachine.ChangeState(UnitState.GUARD_01);

            if (controller.IsEvade())
                controller.stateMachine.ChangeState(UnitState.EVADE);


        }

        public override void OnLateUpdateState()
        {

        }

        public override void OnExitState()
        {

        }

    }
}
