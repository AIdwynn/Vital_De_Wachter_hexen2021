using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DAE.StateSystem;
using DAE.HexSystem;

namespace DAE.GameSystem.States
{
    public class GameStateBase : IState<GameStateBase>
    {
        public StateMachine<GameStateBase> StateMachine { get; }

        public GameStateBase(StateMachine<GameStateBase> stateMachine)
        {
            StateMachine = stateMachine;
        }

        public virtual void OnEnter()
        {

        }

        public virtual void OnExit()
        {

        }


    }
}