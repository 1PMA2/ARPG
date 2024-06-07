using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerController
{
    public class MoveState : BaseState
    {
        private int hashMoveAnimation;
       public MoveState(UnitController controller) : base(controller)
        {
            
        }

        public override void OnEnterState()
        {
            controller.UnitInfo.currentState = UnitState.MOVE;

            controller.animator.applyRootMotion = false;

            controller.DisableSmashTrigger();
            controller.DisableWeaponTrigger();

            //controller.StatController.RestoreStmina(1, 1);
        }

        public override void OnFixedUpdateState()
        {

        }

        public override void OnUpdateState()
        {
            if (controller.IsMove())
            {
                controller.Move();
                controller.LookDiraction();
            }
            else
            {
                controller.stateMachine.ChangeState(UnitState.IDLE);
                return;
            }

            if (controller.IsComboAttack())
            {
                controller.stateMachine.ChangeState(UnitState.COMBO_01);
                controller.StatController.StopRestore();
                return;
            }

            if (controller.IsSmash() && controller.StatController.AbleStamina(DamageState.smashStamina))
            {
                controller.stateMachine.ChangeState(UnitState.SMASH_00);
                controller.StatController.StopRestore();
                return;
            }

            if (controller.IsEvade() && controller.StatController.AbleStamina(DamageState.evadeStamina))
            {
                controller.stateMachine.ChangeState(UnitState.EVADE);
                controller.StatController.StopRestore();
                return;
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
