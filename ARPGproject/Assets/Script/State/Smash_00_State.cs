using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controller
{
    public class Smash_00_State : PlayerState
    {
        private int hashMoveAnimation;
        public Smash_00_State(Player controller) : base(controller)
        {
            
        }

        public override void OnEnterState()
        {
            playerController.UnitInfo.currentState = UnitState.SMASH_00;

            playerController.ChangeAnimation("Smash00", 0.2f, 0.8f);
        }

        public override void OnFixedUpdateState()
        {

        }

        public override void OnUpdateState()
        {
            playerController.LookDiraction();

            if (playerController.CheckAnimation())
                playerController.stateMachine.ChangeState(UnitState.SMASH_01);

        }

        public override void OnLateUpdateState()
        {

        }

        public override void OnExitState()
        {
            
        }

    }
}