using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlayerController
{
    public class IdleState : BaseState
    {
        public IdleState(UnitController controller) : base(controller)
        {

        }

        public override void OnEnterState()
        {


            controller.UnitInfo.currentState = UnitState.IDLE;

            controller.animator.applyRootMotion = false;

            controller.DisableSmashTrigger();
            controller.DisableWeaponTrigger();

            controller.StatController.RestoreStmina(1.8f, 0.2f);
        }

        public override void OnFixedUpdateState()
        {

        }

        public override void OnUpdateState()
        {
            if(controller.IsMove())
            {
                controller.stateMachine.ChangeState(UnitState.MOVE);
                return;
            }
            else
            {
                controller.Idle();
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

            if (controller.IsGuard() && !isGuardCooldown)
            {
                StartGuardCooldown();
                controller.stateMachine.ChangeState(UnitState.GUARD_01);
                controller.StatController.StopRestore();
                return;
            }
        }

        public override void OnLateUpdateState()
        {
            //controller.GetComponent<TestBox>().TakeDamage(1);
        }

        public override void OnExitState()
        {
            
        }


        private float guardCooldown = 0f;
        private bool isGuardCooldown = false;

        private void StartGuardCooldown()
        {
            isGuardCooldown = true;
            controller.StartCoroutine(GuardCooldown());
        }

        private IEnumerator GuardCooldown()
        {
            guardCooldown = 0.5f; // Set the cooldown duration
            while (guardCooldown > 0)
            {
                guardCooldown -= Time.deltaTime;
                yield return null;
            }
            isGuardCooldown = false; // Reset the cooldown flag
        }
    }
}
