using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controller
{
    public class Combo_02_State : PlayerState
    {
        private bool isCombo;
        public Combo_02_State(Player controller) : base(controller)
        {

        }

        public override void OnEnterState()
        {
            playerController.UnitInfo.currentState = UnitState.COMBO_02;
            playerController.ChangeAnimation("Combo2", 0.05f, 1f);
            playerController.IsAnimationStart();
            playerController.DisableWeaponTrigger();
            playerController.LookForward();
            playerController.animator.applyRootMotion = true;
            isCombo = false;
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

            if (playerController.IsComboAttack())
                isCombo = true;

            if (playerController.CheckComboAnimation() && isCombo)
            {
                playerController.stateMachine.ChangeState(UnitState.COMBO_03);
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