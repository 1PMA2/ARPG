using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controller
{
    public class Smash_01_State : PlayerState
    {
        private bool isSmash;
        private float initialSmashSpeed = 50f;
        private TestBox statController;
        public Smash_01_State(Player controller) : base(controller)
        {
            
        }

        public override void OnEnterState()
        {
            if(playerController.UnitInfo.currentState != UnitState.COUNTER)
                playerController.UnitInfo.currentState = UnitState.SMASH_01;
            playerController.ChangeAnimation("Smash01", 0f, 2f);
            playerController.animator.applyRootMotion = false;
            playerController.smashSpeed = initialSmashSpeed;
            playerController.gameObject.layer = 8;
            isSmash = false;

            statController = playerController.GetComponent<TestBox>();
            statController.UseStamina(DamageState.smashStamina);
        }

        public override void OnFixedUpdateState()
        {

        }

        public override void OnUpdateState()
        {
            if (playerController.IsSmash())
                isSmash = true;

            if(playerController.IsSmashMoveStart())
                playerController.SmashMove();

            if (playerController.CheckAnimation())
            {
                if ((controller.IsCounter > 0) && isSmash && statController.AbleStamina(DamageState.smashStamina))
                {
                    ComboSmash();
                    isSmash = false;
                    return;
                }
                else
                {
                    playerController.IsCounter = 0;
                    playerController.stateMachine.ChangeState(UnitState.IDLE);
                    playerController.SetEquip(true);
                    return;
                }
            }
        }

        public override void OnLateUpdateState()
        {

        }

        public override void OnExitState()
        {
            playerController.gameObject.layer = 7;
        }

        private void ComboSmash()
        {
            statController.UseStamina(DamageState.smashStamina);

            playerController.animator.CrossFade("Smash01", 0.2f, -1, 0f);
            playerController.animator.speed = 2f;
            playerController.smashSpeed = 50f;

            if(playerController.InputDir.magnitude <= 0)
            {
                Vector3 currentRotation = playerController.transform.rotation.eulerAngles;
                currentRotation.y += 180;
                playerController.transform.rotation = Quaternion.Euler(currentRotation);
            }
            else
            {
                playerController.transform.rotation = Quaternion.LookRotation(playerController.InputDir);
            }

            playerController.IsCounter--;
        }
    }
}