using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerController
{
    public class Smash_01_State : BaseState
    {
        private int hashMoveAnimation;
        public Smash_01_State(UnitController controller) : base(controller)
        {
            
        }

        public override void OnEnterState()
        {
            controller.gameObject.layer = 8;

            controller.ChangeAnimation("Smash01",0.2f, 2f);

            controller.smashSpeed = 50;
        }

        public override void OnFixedUpdateState()
        {

        }

        public override void OnUpdateState()
        {

            controller.SmashMove();

            if (controller.CheckAnimation())
            {
                controller.gameObject.layer = 0;
                controller.stateMachine.ChangeState(UnitState.IDLE);
            }


        }

        public override void OnLateUpdateState()
        {

        }

        public override void OnExitState()
        {
            controller.SetEquip(true);
        }

    }
}