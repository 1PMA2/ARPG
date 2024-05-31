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

        }

        public override void OnFixedUpdateState()
        {

        }

        public override void OnUpdateState()
        {
            if(controller.IsMove())
            {
                controller.stateMachine.ChangeState(UnitState.MOVE);
            }
            else
            {
                controller.Idle();
            }

            if (controller.IsComboAttack())
                controller.stateMachine.ChangeState(UnitState.COMBO_01);

            if (controller.IsSmash())
                controller.stateMachine.ChangeState(UnitState.SMASH_00);

            //if(controller.IsGuard())
            //{
            //    controller.stateMachine.ChangeState(UnitState.GUARD_01);
            //}

            if (controller.IsEvade())
                controller.stateMachine.ChangeState(UnitState.EVADE);

            if (controller.IsGuard() && !isGuardCooldown)
            {
                StartGuardCooldown();
                controller.stateMachine.ChangeState(UnitState.GUARD_01);
            }
        }

        public override void OnLateUpdateState()
        {

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
            guardCooldown = 1f; // Set the cooldown duration
            while (guardCooldown > 0)
            {
                guardCooldown -= Time.deltaTime;
                yield return null;
            }
            isGuardCooldown = false; // Reset the cooldown flag
        }
    }
}
