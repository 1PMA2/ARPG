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
            Debug.Log("Smash_00");
        }

        public override void OnFixedUpdateState()
        {

        }

        public override void OnUpdateState()
        {
            if (controller.IsMove())
            {
                controller.stateMachine.ChangeState(UnitState.MOVE);
            }
            else
            {
                controller.Idle();
            }
        }

        public override void OnLateUpdateState()
        {

        }

        public override void OnExitState()
        {

        }

    }
}