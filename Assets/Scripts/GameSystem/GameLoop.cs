using DAE.BoardSystem;
using DAE.GameSystem.States;
using DAE.HexSystem;
using DAE.StateSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DAE.GameSystem
{
    class GameLoop : MonoBehaviour

    {
        [SerializeField]
        public Piece Player;
        [SerializeField]
        private PositionHelper _positionHelper;
        [SerializeField]
        private CardManager _cardManager;

        [SerializeField]
        private Transform _boardParent;
        [SerializeField]
        private int _boardRadius;

        [SerializeField]
        private GameObject _startScreen;
        [SerializeField]
        private GameObject _endScreen;


        private Grid<Position> _grid;
        private Board<Position, Piece> _board;


        private List<Hexes> _hexes;
        

        private ActionManager<Piece, Card> _actionManager;

        private StateMachine<GameStateBase> _gameStateMachine;

        public void Start()
        {

            _grid = new Grid<Position>(_boardRadius, _boardRadius);

            _board = new Board<Position, Piece>();
            _actionManager = new ActionManager<Piece, Card>(_board, _grid);
            _hexes = new List<Hexes>();

            ConnectGrid(_grid);
            ConnectPiece( _board, _grid);

            var gridPos = _positionHelper.ToGridPosition(_grid, _boardParent, Player.transform.position);
            _grid.TryGetPositionAt(gridPos.x, gridPos.y, out var pos);
            _grid.PlayerPos = pos;

            _board.Moved += (s, e) =>
            {
                if(_grid.TryGetCoordinateOf(e.ToPosition, out var toCoordinate))
                {
                    var worldPosition = _positionHelper.ToWorldPosition(_grid, _boardParent, toCoordinate);

                    e.piece.MoveTo(worldPosition);
                }
            };

            _board.Taken += (s, e) =>
            {
                e.piece.Taken(e.IsPlayer);
            };



            _gameStateMachine = new StateMachine<GameStateBase>();
            _gameStateMachine.RegisterState(GameStates.GameStates.GamePlayState, new GamePlayState(_gameStateMachine, _actionManager, _board, _grid, 
                _positionHelper, Player, _boardParent, _cardManager, _hexes));
            _gameStateMachine.RegisterState(GameStates.GameStates.EndScreenState, new EndScreenState(_gameStateMachine, _endScreen));
            _gameStateMachine.RegisterState(GameStates.GameStates.StartScreenState, new StartScreenState(_gameStateMachine, _startScreen));

            _gameStateMachine.InitialState = GameStates.GameStates.StartScreenState;
        }

        private void ConnectPiece( Board<Position, Piece> board, Grid<Position> grid)
        {
            var pieces = FindObjectsOfType<Piece>();
            foreach (var piece in pieces)
            {
                var (x, y) = _positionHelper.ToGridPosition(grid, _boardParent, piece.transform.position);
                if (grid.TryGetPositionAt(x, y, out var position))
                {
                    board.Place(piece, position);
                }


            }
        }

        private void ConnectGrid(Grid<Position> grid)
        {
            var hexes = FindObjectsOfType<Hexes>();
            foreach (var hex in hexes)
            {
                var position = new Position();
                hex.Model = position;

                var (x, y) = _positionHelper.ToGridPosition(grid, _boardParent, hex.transform.position);

                grid.Register(x, y, position);

                var s = -x - y;
                hex.gameObject.name = $"Hex ({x}, {y}, {s})";
                _hexes.Add(hex);

            }
        }

        public void SwitchStates(int stateNumber)
        {
            switch (stateNumber)
            {
               case 1:
                    _gameStateMachine.MoveToSTate(GameStates.GameStates.StartScreenState);
                    break;
                case 2:
                    _gameStateMachine.MoveToSTate(GameStates.GameStates.GamePlayState);
                    break;
                case 3:
                    _gameStateMachine.MoveToSTate(GameStates.GameStates.EndScreenState);
                    break;

            }
        }
    }
}


