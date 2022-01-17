using DAE.StateSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DAE.GameSystem.States
{
    public class EndScreenState : GameStateBase
    {
        private GameObject _endScreen;
        public EndScreenState(StateMachine<GameStateBase> stateMachine, GameObject endScreen) : base(stateMachine)
        {
            _endScreen = endScreen;
        }

        public override void OnEnter()
        {
            _endScreen.SetActive(true);
        }

        public override void OnExit()
        {
            _endScreen.SetActive(false);
        }


    }
}