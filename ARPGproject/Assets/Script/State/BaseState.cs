using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlayerController
{
    public enum UnitState
    {
        IDLE = 100,
        MOVE,
        ATTACK_00,
        SMASH_00,
        END
    }

    public struct UnitStateComparer : IEqualityComparer<UnitState>
    {
        public bool Equals(UnitState a, UnitState b) { return a == b; }
        public int GetHashCode(UnitState a) { return (int)a; }
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



