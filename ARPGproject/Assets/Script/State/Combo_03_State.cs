using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerController
{
    public class Combo_03_State : BaseState
    {
        private int hashMoveAnimation;
        public Combo_03_State(UnitController controller) : base(controller)
        {
            
        }

        public override void OnEnterState()
        {
            controller.ChangeAnimation("Combo3", 0.2f, 1.5f);

            controller.LookForward();

            controller.animator.applyRootMotion = true;

            controller.DisableWeaponTrigger();

            controller.SetWeaponDamage(3);

        }

        public override void OnFixedUpdateState()
        {

        }

        public override void OnUpdateState()
        {
            

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
            controller.SetEquip(true);

            controller.SetWeaponDamage(1);
        }

    }
}