using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;



namespace Controller
{
    public class StateMachine
    {
        public BaseState CurrentState { get; private set; }
        private Dictionary<UnitState, BaseState> states = new Dictionary<UnitState, BaseState>(new UnitStateComparer());

        public StateMachine(UnitState stateName, BaseState state)
        {
            AddState(stateName, state);
            CurrentState = GetState(stateName);
        }

        public void AddState(UnitState stateName, BaseState state)
        {
            if(!states.ContainsKey(stateName)) //없으면
            {
                states.Add(stateName, state);
            }
        }

        public BaseState GetState(UnitState stateName)
        {
            if(states.TryGetValue(stateName, out BaseState state))
                return state;
            return null;
        }

        public void DeleteState(UnitState removeStateName)
        {
            if(states.ContainsKey(removeStateName))
            {
                states.Remove(removeStateName);
            }

        }

        public void ChangeState(UnitState nextStateName)
        {
            CurrentState?.OnExitState(); //null이 아니면 탈출 실행
            if(states.TryGetValue(nextStateName, out BaseState newState))
            {
                CurrentState = newState;
            }
            CurrentState?.OnEnterState();
        }


        public void OnFixedUpdateState()
        {
            CurrentState?.OnFixedUpdateState();
        }

        public void OnUpdateState()
        {
            CurrentState?.OnUpdateState();
        }

        public void OnLateUpdateState()
        {
            CurrentState?.OnLateUpdateState();
        }
    }
}
