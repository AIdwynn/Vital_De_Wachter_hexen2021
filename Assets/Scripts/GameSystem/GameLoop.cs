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

        private Grid<Position> _grid;
        private Board<Position, Piece> _board;


        private List<Card> _cards;
        private Card _currentCard;

        private ActionManager<Piece, Card> _actionManager;

        public void Start()
        {

            _grid = new Grid<Position>(_boardRadius, _boardRadius);
            _board = new Board<Position, Piece>();
            _actionManager = new ActionManager<Piece, Card>(_board, _grid);

            _cards = new List<Card>();

            ConnectGrid(_grid);
            ConnectPiece( _board, _grid);

            _board.Moved += (s, e) =>
            {
                if(_grid.TryGetCoordinateOf(e.ToPosition, out var toCoordinate))
                {
                    var worldPosition = _positionHelper.ToWorldPosition(_grid, _boardParent, toCoordinate);

                    e.piece.MoveTo(worldPosition);
                }
            };

            for (int i = 0; i < _startingHandSize; i++)
            {
                CardDraw();
            }
        }

        private void Deselect(Hexes s)
        {
            var positions = _actionManager.ValidPositionOf(Player);
            foreach (var position in positions)
            {
                position.Deactivate();
            }
        }

        private void Select(Hexes hex)
        {
            var positions = _actionManager.ValidPositionOf(Player);
            var partOf = false;
            var (x, y) = _positionHelper.ToGridPosition(_grid, _boardParent, hex.transform.position);
            if (_grid.TryGetPositionAt(x, y, out var hexPos))
            {
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
                hexPos.Activate();
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
                    Select(hex);
                };

                hex.EndHover += (s, e) =>
                {
                    Deselect(hex);
                };
            }
        }

        private void CardDraw()
        {
            var card = _cardManager.CardDraw();
            card.BeginDragging += (s, e) =>
            {
                _currentCard = e.Card;
                Debug.Log($"you are dragging a {_currentCard.gameObject.name} card");
            };
            card.Drop += (s,e) =>
            {
                var validPos = _actionManager.ValidPositionOf(Player, _currentCard);
                //if ()
            };
            _cards.Add(card);
        }



    }
}


