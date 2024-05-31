using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerController
{
    public class Smash_00_State : BaseState
    {
        private int hashMoveAnimation;
        public Smash_00_State(UnitController controller) : base(controller)
        {
            
        }

        public override void OnEnterState()
        {
            controller.UnitInfo.currentState = UnitState.SMASH_00;

            controller.ChangeAnimation("Smash00");
        }

        public override void OnFixedUpdateState()
        {

        }

        public override void OnUpdateState()
        {
            controller.LookDiraction();

            if (controller.CheckAnimation())
                controller.stateMachine.ChangeState(UnitState.SMASH_01);

        }

        public override void OnLateUpdateState()
        {

        }

        public override void OnExitState()
        {
            
        }

    }
}