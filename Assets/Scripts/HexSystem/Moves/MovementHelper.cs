using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DAE.BoardSystem;


namespace DAE.HexSystem.Moves
{


    class MovementHelper<TPiece>
        where TPiece: IPiece
    {
        private Board<Position, TPiece> _board;
        private Grid<Position>  _grid;
        private List<Position> _validPositions = new List<Position>();
        private TPiece _piece;

        public MovementHelper(Board<Position, TPiece> board, Grid<Position> grid, TPiece piece)
        {
            _board = board;
            this._piece = piece;
            this._grid = grid;
        }

        public MovementHelper<TPiece> Move(int qOffset, int rOffset, int numTiles = int.MaxValue, params Validator[] validators)
        {
            //if(_piece.PlayerID == 1) //black
            //{
            //    xOffset *= -1;
            //    yOffset *= -1;
            //}
            if (!_board.TryGetPositionOf(_piece, out var position))
                return this;

            if (!_grid.TryGetCoordinateOf(position, out var coordinate))
                return this;

            var nextQCoordinate = coordinate.q + qOffset;
            var nextRCoordinate = coordinate.r + rOffset;
           
            var step = 0;
            var hasNextPosition = _grid.TryGetPositionAt(nextQCoordinate, nextRCoordinate, out var nextPosition);

            while (hasNextPosition && step < numTiles)
            {
                var isOk = validators.All((v) => v(_board, _grid, _piece, nextPosition));
                if (!isOk)
                    return this;

                var hasPiece = _board.TryGetPieceAt(nextPosition, out var nextPiece);
                if (!hasPiece)
                {
                    _validPositions.Add(nextPosition);
                }
                else
                {
                    if (nextPiece.PlayerID == _piece.PlayerID)
                        return this;

                    _validPositions.Add(nextPosition);
                    return this;
                }
                nextQCoordinate +=  qOffset;
                nextRCoordinate +=  rOffset;


                hasNextPosition = _grid.TryGetPositionAt(nextQCoordinate, nextRCoordinate, out nextPosition);

                
                step++;

            }

            return this;
        }

        

        public delegate bool Validator(Board<Position, TPiece> board, Grid<Position> grid, TPiece piece, Position position);
        internal MovementHelper<TPiece> TopRight(int numTiles = int.MaxValue, params Validator[] validators)
           => Move(0, 1, numTiles, validators);

        internal MovementHelper<TPiece> Right(int numTiles = int.MaxValue, params Validator[] validators)
            => Move(1, 0, numTiles, validators);


        internal MovementHelper<TPiece> BottemRight(int numTiles = int.MaxValue, params Validator[] validators)
            => Move(1, -1, numTiles, validators);


        internal MovementHelper<TPiece> BottemLeft(int numTiles = int.MaxValue, params Validator[] validators)
            => Move(0, -1, numTiles, validators);

        internal MovementHelper<TPiece> Left(int numTiles = int.MaxValue, params Validator[] validators)
            => Move(-1, 0, numTiles, validators);

        internal MovementHelper<TPiece> TopLeft(int numTiles = int.MaxValue, params Validator[] validators)
            => Move(-1, 1, numTiles, validators);


        internal List<Position> Collect()
        {
            return _validPositions;
        }

        public static bool IsEmptyTile(Board<Position, TPiece> board, Grid<Position> grid, TPiece piece, Position position)
        =>  !board.TryGetPieceAt(position, out _);

        public static bool HasEnemyPiece(Board<Position, TPiece> board, Grid<Position> grid, TPiece piece, Position position)
        => board.TryGetPieceAt(position, out var enemyPiece) && enemyPiece.PlayerID != piece.PlayerID;
            

    }
}