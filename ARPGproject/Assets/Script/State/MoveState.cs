using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controller
{
    public class MoveState : PlayerState
    {
        private int hashMoveAnimation;
       public MoveState(Player controller) : base(controller)
        {
            
        }

        public override void OnEnterState()
        {
            playerController.UnitInfo.currentState = UnitState.MOVE;
            playerController.DisableWeaponTrigger();
            playerController.DisableSmashTrigger();
            playerController.animator.applyRootMotion = false;
            //controller.StatController.RestoreStmina(1, 1);
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
            if (playerController.IsMove())
            {
                playerController.Move();
                playerController.LookDiraction();
            }
            else
            {
                playerController.stateMachine.ChangeState(UnitState.IDLE);
                return;
            }

            if (playerController.IsComboAttack())
            {
                playerController.stateMachine.ChangeState(UnitState.COMBO_01);
                playerController.StatController.StopRestore();
                return;
            }

            if (playerController.IsSmash() && playerController.StatController.AbleStamina(DamageState.smashStamina))
            {
                playerController.stateMachine.ChangeState(UnitState.SMASH_00);
                playerController.StatController.StopRestore();
                return;
            }

            if (playerController.IsEvade() && playerController.StatController.AbleStamina(DamageState.evadeStamina))
            {
                playerController.stateMachine.ChangeState(UnitState.EVADE);
                playerController.StatController.StopRestore();
                return;
            }

            if (playerController.IsGuard())
            {
                playerController.stateMachine.ChangeState(UnitState.GUARD_01);
                playerController.StatController.StopRestore();
                return;
            }
        }


    }
}
