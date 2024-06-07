using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerController
{
    public class Combo_03_State : BaseState
    {
        public Combo_03_State(UnitController controller) : base(controller)
        {
            
        }

        public override void OnEnterState()
        {
            controller.UnitInfo.currentState = UnitState.COMBO_03;

            controller.ChangeAnimation("Combo3", 0.02f, 1.5f);
            
            controller.LookForward();

            controller.animator.applyRootMotion = true;

            controller.DisableWeaponTrigger();

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
                return;
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