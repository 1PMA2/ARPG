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
            Debug.Log("idle");
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
        }

        public override void OnLateUpdateState()
        {

        }

        public override void OnExitState()
        {

        }

    }
}
