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
            
        }

        public override void OnFixedUpdateState()
        {

        }

        public override void OnUpdateState()
        {
            controller.LookDiraction();
        }

        public override void OnLateUpdateState()
        {

        }

        public override void OnExitState()
        {

        }
    }
}
