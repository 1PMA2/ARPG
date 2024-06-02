using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlayerController
{
    public enum UnitState
    {
        IDLE = 100,
        MOVE,
        COMBO_01,
        COMBO_02,
        COMBO_03,
        SMASH_00,
        SMASH_01,
        GUARD_01,
        GUARD_02,
        EVADE,
        HIT,
        COUNTER,
        ENEMY_IDLE,
        ENEMY_MOVE,
        ENEMY_PATROL,
        ENEMY_ATTACK,
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



