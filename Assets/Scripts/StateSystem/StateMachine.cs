using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DAE.StateSystem
{
    public class StateMachine<TState>
        where TState : IState<TState>
    {
        private Dictionary<string, TState> _states = new Dictionary<string, TState>();
       private string _currentStateName = "";

        public string InitialState
        {
            set
            {
                _currentStateName = value;
                CurrentState?.OnEnter();
            }
        }
        public TState CurrentState
        {
            get
            {
                if (_states.ContainsKey(_currentStateName))
                    return _states[_currentStateName];
                else
                    return default(TState);
            }
        }

        public void MoveToSTate(string stateName)
        {
            CurrentState?.OnExit();
            _currentStateName = stateName;
            CurrentState.OnEnter();
        }

        public void RegisterState(string statename, TState state)
        {
            if (_states.ContainsKey(statename))
                throw new ArgumentException($"{nameof(statename)} already exists");
            _states[statename] = state;
        }
    }
}