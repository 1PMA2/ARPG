using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controller
{
    public class Combo_03_State : PlayerState
    {
        public Combo_03_State(Player controller) : base(controller)
        {

        }

        public override void OnEnterState()
        {
            playerController.UnitInfo.currentState = UnitState.COMBO_03;
            playerController.ChangeAnimation("Combo3", 0.05f, 1.5f);
            playerController.IsAnimationStart();
            playerController.DisableWeaponTrigger();
            playerController.LookForward();
            playerController.animator.applyRootMotion = true;
        }

        public override void OnFixedUpdateState()
        {

        }

        public override void OnUpdateState()
        {
            StateBranch();
        }

        public override void OnLateUpdateState()
        {

        }

        public override void OnExitState()
        {
            //controller.ExitComnbo();
        }

        private void StateBranch()
        {
            if (playerController.IsEvade() && playerController.StatController.AbleStamina(DamageState.evadeStamina))
            {
                playerController.stateMachine.ChangeState(UnitState.EVADE);
                return;
            }

            if (playerController.CheckAnimation())
            {
                playerController.stateMachine.ChangeState(UnitState.IDLE);
                playerController.SetEquip(true);
                return;
            }
        }
    }
}