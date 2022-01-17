using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DAE.BoardSystem;
using DAE.HexSystem;
using DAE.StateSystem;


namespace DAE.GameSystem.States
{
    class GamePlayState : GameStateBase
    {
        private ActionManager<Piece, Card> _actionManager;
        private Grid<Position> _grid;
        private Board<Position, Piece> _board;
        private PositionHelper _positionHelper;
        private Piece _player;
        private Transform _boardParent;
        private CardManager _cardManager;
        private List<Hexes> _hexes;

        private Card _currentCard;
        private bool _dragging = false;



        public GamePlayState(StateMachine<GameStateBase> stateMachine, ActionManager<Piece, Card> actionManager, Board<Position, Piece> board, Grid<Position> grid
            , PositionHelper positionHelper, Piece player, Transform boardParent, CardManager cardManager, List<Hexes> hexes)
            : base(stateMachine)
        {
            _actionManager = actionManager;
            _grid = grid;
            _board = board;
            _positionHelper = positionHelper;
            _player = player;
            _boardParent = boardParent;
            _cardManager = cardManager;
            _hexes = hexes;


        }

        public override void OnEnter()
        {
            _cardManager.ShowHand();
            AddListeners();
            GenerateDeck();
            _cardManager.GenerateStartHand();

        }


        public override void OnExit()
        {
            _cardManager.HideHand();
        }

        internal void Select(Hexes s)
        {
            var positions = new List<Position>();
            var partOf = false;
            var (x, y) = _positionHelper.ToGridPosition(_grid, _boardParent, s.transform.position);
            if (_grid.TryGetPositionAt(x, y, out var hexPos))
            {
                positions = _actionManager.AllValidPositionOf(_player, _currentCard, hexPos);
                foreach (var position in positions)
                {
                    if (position == hexPos)
                        partOf = true;
                }
            }

            if (!partOf)
            {
                foreach (var position in positions)
                {
                    position.Activate();
                }
            }
            else
            {
                var actionPositions = _actionManager.ActionValidPositions(_player, _currentCard, hexPos);
                foreach (var position in actionPositions)
                {
                    position.Activate();
                }
            }
        }

        internal void Deselect(Hexes s)
        {
            var posGrid = _positionHelper.ToGridPosition(_grid, _boardParent, s.transform.position);
            _grid.TryGetPositionAt(posGrid.x, posGrid.y, out var pos);
            var positions = _actionManager.AllValidPositionOf(_player, _currentCard, pos);
            foreach (var position in positions)
            {
                position.Deactivate();
            }
        }

        private void CardDraw()
        {
            _cardManager.CardDraw();
        }
        private void GenerateDeck()
        {
            var deck = _cardManager.GenerateDeck();
            foreach (Card card in deck)
            {
                card.BeginDragging += (s, e) =>
                {
                    _currentCard = e.Card;
                    _dragging = true;
                    Debug.Log($"you are dragging a {_currentCard.gameObject.name} card");
                };
                card.EndDragging += (s, e) =>
                {
                    _dragging = false;
                };
            }
        }

        private void AddListeners()
        {
            _player.Killed += (s, e) =>
            {
                StateMachine.MoveToSTate(GameStates.GameStates.EndScreenState);
            };

            foreach (var hex in _hexes)
            {
                hex.StartHover += (s, e) =>
                {
                    if (_dragging && _currentCard != null)
                    {
                        Select(hex);
                    }

                };

                hex.EndHover += (s, e) =>
                {
                    if (_currentCard != null)
                        Deselect(hex);
                };

                hex.Drop += (s, e) =>
                {
                    Deselect(hex);
                    var currentViewPos = hex.transform.position;
                    var currentGridPos = _positionHelper.ToGridPosition(_grid, _boardParent, currentViewPos);
                    _grid.TryGetPositionAt(currentGridPos.x, currentGridPos.y, out var hoverPos);
                    var validPos = _actionManager.AllValidPositionOf(_player, _currentCard, hoverPos);
                    if (validPos.Contains(hoverPos))
                    {
                        _actionManager.PerformAction(_player, _currentCard, hoverPos);
                        _currentCard.OnEndDrag(null);
                        _cardManager.RemoveCard(_currentCard);
                        CardDraw();
                    }
                };
            }
        }
            
    }
}