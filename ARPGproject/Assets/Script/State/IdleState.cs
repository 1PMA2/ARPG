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

        }

        public override void OnFixedUpdateState()
        {

        }

        public override void OnUpdateState()
        {
            

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                controller.stateMachine.ChangeState(UnitState.MOVE);
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
