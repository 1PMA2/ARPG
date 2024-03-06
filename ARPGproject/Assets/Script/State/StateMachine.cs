using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace PlayerController
{
    public class StateMachine
    {
        public BaseState CurrentState { get; private set; }
        private Dictionary<UnitState, BaseState> states = new Dictionary<UnitState, BaseState>();

        public StateMachine(UnitState stateName, BaseState state)
        {
            
        }

        public void AddState(UnitState stateName, BaseState state)
        {
            if(!states.ContainsKey(stateName))
            {
                states.Add(stateName, state);
            }
        }
    }
}
