using DAE.BoardSystem;
using DAE.ChessSystem;
using DAE.SelectionSystem;
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
        private PositionHelper _positionHelper;

        [SerializeField]
        private Transform _boardParent;
        [SerializeField]
        private int _boardRadius;

        private Grid<Position> _grid;
        private Board<Position, Piece> _board;
        private SelectionManager<Piece> _selectionManager;
        private MoveManager<Piece> _moveManager;
        public void Start()
        {
            _grid = new Grid<Position>(_boardRadius, _boardRadius);
            _board = new Board<Position, Piece>();
            _selectionManager = new SelectionManager<Piece>();
            _moveManager = new MoveManager<Piece>(_board, _grid);

            ConnectGrid(_grid);
            ConnectPiece(_selectionManager, _board, _grid);

            _selectionManager.Selected += (s, e) =>
            {
               
                var positions = _moveManager.ValidPositionOf(e.SelectableItem);
                foreach (var position in positions)
                {
                    position.Activate();
                }
            };

            _selectionManager.Deselected += (s, e) =>
            {
                var positions = _moveManager.ValidPositionOf(e.SelectableItem);
                foreach (var position in positions)
                {
                    position.Deactivate();
                }
            };
        }

        public void DeselectAll()
        {
            _selectionManager.DeselectAll();
        }

        private void ConnectPiece(SelectionManager<Piece> _selectionManager, Board<Position, Piece> board, Grid<Position> grid)
        {
            var pieces = FindObjectsOfType<Piece>();
            foreach (var piece in pieces)
            {
                var (x, y) = _positionHelper.ToGridPosition(grid, _boardParent, piece.transform.position);
                if(grid.TryGetPositionAt(x,y, out var position))
                {


                    piece.Clicked += (s, e) =>
                    {
                        _selectionManager.DeselectAll();
                        _selectionManager.Toggle(s as Piece);
                    };
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

                var (x,y) = _positionHelper.ToGridPosition(grid, _boardParent, hex.transform.position);
                              
                grid.Register(x, y, position);

                hex.gameObject.name = $"Tile ({x}, {y})";
            }
        }

    }
}


