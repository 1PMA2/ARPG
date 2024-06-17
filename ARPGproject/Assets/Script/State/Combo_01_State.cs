using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controller
{
    public class Combo_01_State : PlayerState
    {
        private bool isCombo;
        public Combo_01_State(Player controller) : base(controller)
        {
            
        }

        public override void OnEnterState()
        {

            playerController.UnitInfo.currentState = UnitState.COMBO_01;
            playerController.ChangeAnimation("Combo1", 0.2f, 1.2f);
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
            
        }

        private void StateBranch()
        {
            if (playerController.IsComboAttack())
                isCombo = true;

            if (playerController.CheckComboAnimation() && isCombo)
            {
                playerController.stateMachine.ChangeState(UnitState.COMBO_02);
                return;
            }

            if (playerController.IsEvade() && controller.StatController.AbleStamina(DamageState.evadeStamina))
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