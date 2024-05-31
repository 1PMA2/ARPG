using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerController
{
    public class Smash_01_State : BaseState
    {
        private bool isSmash;
        private float initialSmashSpeed = 50f;
        private float comboSmashSpeed = 30f;
        public Smash_01_State(UnitController controller) : base(controller)
        {
            
        }

        public override void OnEnterState()
        {
            controller.UnitInfo.currentState = UnitState.SMASH_01;

            controller.gameObject.layer = 8;

            controller.ChangeAnimation("Smash01",0.2f, 2f);

            controller.smashSpeed = initialSmashSpeed;

            isSmash = false;
        }

        public override void OnFixedUpdateState()
        {

        }

        public override void OnUpdateState()
        {
            if(controller.IsSmashMoveStart())
                controller.SmashMove();

            //ComboSmash();

            if (controller.CheckAnimation())
            {
                controller.stateMachine.ChangeState(UnitState.IDLE);
                controller.SetEquip(true);
            }




        }

        public override void OnLateUpdateState()
        {

        }

        public override void OnExitState()
        {
            controller.gameObject.layer = 0;
        }

        private void ComboSmash()
        {
            if (controller.IsSmash())//Å³Á¶°Ç
            {
                isSmash = true;
            }
            if (controller.CheckSmashAnimation() && isSmash)
            {
                controller.animator.CrossFade("Smash01", 0.2f, -1, 0f);
                controller.animator.speed = 2f;
                controller.smashSpeed = 50f;
                controller.LookForward();
                isSmash = false;
                return;
            }
        }
    }
}