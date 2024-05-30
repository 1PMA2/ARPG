using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerController
{
    public class Combo_02_State : BaseState
    {
        private static bool isCombo;
        public Combo_02_State(UnitController controller) : base(controller)
        {
            
        }

        public override void OnEnterState()
        {
            controller.ChangeAnimation("Combo2", 0.1f, 1.2f);

            isCombo = false;

            controller.LookForward();

            controller.animator.applyRootMotion = true;

            controller.SetWeaponDamage(1);
        }

        public override void OnFixedUpdateState()
        {

        }

        public override void OnUpdateState()
        {
            

            if (controller.IsComboAttack())
                isCombo = true;

            if (controller.CheckComboAnimation() && isCombo)
                controller.stateMachine.ChangeState(UnitState.COMBO_03);


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
            
        }

    }
}