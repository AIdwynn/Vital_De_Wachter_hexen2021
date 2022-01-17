using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DAE.StateSystem
{
    public interface IState<TState>
        where TState : IState<TState>
    {
        void OnEnter();
        void OnExit();
        StateMachine<TState> StateMachine { get; }
    }
}