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
            }

            if (controller.IsComboAttack())
                controller.stateMachine.ChangeState(UnitState.COMBO_01);

            if (controller.IsSmash())
                controller.stateMachine.ChangeState(UnitState.SMASH_00);

            if (controller.IsEvade())
                controller.stateMachine.ChangeState(UnitState.EVADE);
        }

        public override void OnLateUpdateState()
        {
            
        }

        public override void OnExitState()
        {
            
        }

       
      
    }
}
