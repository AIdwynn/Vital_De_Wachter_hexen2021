using DAE.HexSystem;
using DAE.StateSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DAE.GameSystem.States
{
    public class StartScreenState : GameStateBase
    {
        private GameObject _startScreen;
        public StartScreenState(StateMachine<GameStateBase> stateMachine, GameObject startScreen) : base(stateMachine)
        {
        }
        public override void OnEnter()
        {
            _startScreen.SetActive(true);
        }

        public override void OnExit()
        {
            _startScreen.SetActive(false);
        }

    }
}