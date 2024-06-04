using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerController
{
    public class Smash_01_State : BaseState
    {
        private bool isSmash;
        private float initialSmashSpeed = 50f;
        public Smash_01_State(UnitController controller) : base(controller)
        {
            
        }

        public override void OnEnterState()
        {
            controller.animator.applyRootMotion = false;

            if(controller.UnitInfo.currentState != UnitState.COUNTER)
                controller.UnitInfo.currentState = UnitState.SMASH_01;

            controller.gameObject.layer = 8;

            controller.ChangeAnimation("Smash01", 0f, 2f);

            controller.smashSpeed = initialSmashSpeed;

            isSmash = false;

            
        }

        public override void OnFixedUpdateState()
        {

        }

        public override void OnUpdateState()
        {
            if (controller.IsSmash())
                isSmash = true;

            if(controller.IsSmashMoveStart())
                controller.SmashMove();

            if (controller.CheckAnimation())
            {
                if ((controller.IsCounter > 0) && isSmash)
                {
                    ComboSmash();
                    isSmash = false;
                    return;
                }

                controller.IsCounter = 0;
                controller.stateMachine.ChangeState(UnitState.IDLE);
                controller.SetEquip(true);
            }




        }

        public override void OnLateUpdateState()
        {

        }

        public override void OnExitState()
        {
            controller.gameObject.layer = 7;
        }

        private void ComboSmash()
        {
            

            controller.animator.CrossFade("Smash01", 0.2f, -1, 0f);
            controller.animator.speed = 2f;
            controller.smashSpeed = 50f;

            if(controller.InputDir.magnitude <= 0)
            {
                Vector3 currentRotation = controller.transform.rotation.eulerAngles;
                currentRotation.y += 180;
                controller.transform.rotation = Quaternion.Euler(currentRotation);
            }
            else
            {
                controller.transform.rotation = Quaternion.LookRotation(controller.InputDir);
            }

            controller.IsCounter--;
        }
    }
}