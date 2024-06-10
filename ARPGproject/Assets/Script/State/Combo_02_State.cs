using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerController
{
    public class Combo_02_State : BaseState
    {
        private bool isCombo;
        public Combo_02_State(UnitController controller) : base(controller)
        {
            
        }

        public override void OnEnterState()
        {
            controller.UnitInfo.currentState = UnitState.COMBO_02;

            controller.IsAnimationStart();
            controller.ChangeAnimation("Combo2", 0.05f, 1f);

            isCombo = false;

            controller.LookForward();

            controller.animator.applyRootMotion = true;
        }

        public override void OnFixedUpdateState()
        {

        }

        public override void OnUpdateState()
        {
            if (controller.IsEvade() && controller.StatController.AbleStamina(DamageState.evadeStamina))
            {
                controller.stateMachine.ChangeState(UnitState.EVADE);
                return;
            }

            if (controller.IsComboAttack())
                isCombo = true;

            if (controller.CheckComboAnimation() && isCombo)
            {
                controller.stateMachine.ChangeState(UnitState.COMBO_03);
                return;
            }


            if (controller.CheckAnimation())
            {
                controller.stateMachine.ChangeState(UnitState.IDLE);
                controller.SetEquip(true);
                return;
            }
        }

        public override void OnLateUpdateState()
        {

        }

        public override void OnExitState()
        {
            controller.ExitComnbo();
        }

    }
}