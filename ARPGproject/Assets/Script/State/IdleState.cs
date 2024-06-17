using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Controller
{
    public class IdleState : PlayerState
    {
        public IdleState(Player controller) : base(controller)
        {

        }

        public override void OnEnterState()
        {   
            playerController.UnitInfo.currentState = UnitState.IDLE;
            playerController.DisableWeaponTrigger();
            playerController.DisableSmashTrigger();
            playerController.StatController.RestoreStmina(4f, 0.5f);
            playerController.animator.applyRootMotion = false;       
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
                playerController.stateMachine.ChangeState(UnitState.MOVE);
                return;
            }
            else
            {
                playerController.Idle();
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
