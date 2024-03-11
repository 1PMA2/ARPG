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
            Debug.Log("Move");
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
        }

        public override void OnLateUpdateState()
        {
            
        }

        public override void OnExitState()
        {
            
        }

       
      
    }
}
