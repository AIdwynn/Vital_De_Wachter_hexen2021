using DAE.BoardSystem;
using DAE.HexSystem;
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
        private int _startingHandSize;
        [SerializeField]
        private int _deckSize;

        private Grid<Position> _grid;
        private Board<Position, Piece> _board;


        private List<Card> _cards;
        private Card _currentCard;
        

        private ActionManager<Piece, Card> _actionManager;
        private bool _dragging = false;

        public void Start()
        {

            _grid = new Grid<Position>(_boardRadius, _boardRadius);

            _board = new Board<Position, Piece>();
            _actionManager = new ActionManager<Piece, Card>(_board, _grid);

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

            GenerateDeck();

            for (int i = 0; i < _startingHandSize; i++)
            {
                CardDraw();
            }
        }

        private void Deselect(Hexes s)
        {
            var posGrid = _positionHelper.ToGridPosition(_grid, _boardParent, s.transform.position);
            _grid.TryGetPositionAt(posGrid.x, posGrid.y, out var pos);
            var positions = _actionManager.AllValidPositionOf(Player, _currentCard, pos);
            foreach (var position in positions)
            {
                position.Deactivate();
            }
        }

        private void Select(Hexes hex)
        {
            var positions = new List<Position>();
            var partOf = false;
            var (x, y) = _positionHelper.ToGridPosition(_grid, _boardParent, hex.transform.position);
            if (_grid.TryGetPositionAt(x, y, out var hexPos))
            {
                positions = _actionManager.AllValidPositionOf(Player, _currentCard, hexPos);
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
                var actionPositions = _actionManager.ActionValidPositions(Player, _currentCard, hexPos);
                foreach (var position in actionPositions)
                {
                    position.Activate();
                }
            }
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
                    var validPos = _actionManager.AllValidPositionOf(Player, _currentCard, hoverPos);
                    if (validPos.Contains(hoverPos))
                    {
                        _actionManager.PerformAction(Player, _currentCard, hoverPos);
                        _currentCard.OnEndDrag(null);
                        Destroy(_currentCard.gameObject);
                        CardDraw();                      
                    }              
                };
            }
        }

        private void CardDraw()
        {
            _cardManager.CardDraw();
        }
        private void GenerateDeck()
        {
            var deck = _cardManager.GenerateDeck(_deckSize);
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



    }
}


