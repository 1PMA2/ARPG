using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlayerController
{
    public enum UnitState
    {
        IDLE = 0,
        MOVE = 1,
        COMBAT_IDLE = 2,
        ATTACK = 3,
        SMASH_START = 4,
        SMASH_CHARGE = 5,
        SMASH = 6,
        END = 7
    }

    public abstract class BaseState
    {
        protected UnitController controller { get; private set; }

        public BaseState(UnitController controller)
        {
            this.controller = controller;
        }

        public abstract void OnEnterState();
        public abstract void OnFixedUpdateState();
        public abstract void OnUpdateState();
        public abstract void OnLateUpdateState();
        public abstract void OnExitState();
    }
}



