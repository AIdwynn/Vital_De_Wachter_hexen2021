using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DAE.BoardSystem;



namespace DAE.HexSystem.Moves
{


    class ActionHelper<TPiece, TCard>
        where TPiece: IPiece where TCard : ICard
    {
        private Board<Position, TPiece> _board;
        private Grid<Position>  _grid;
        private List<Position> _validPositions = new List<Position>();
        private List<Delegate> _delegates = new List<Delegate>();
        private TPiece _piece;
        private TCard _card;
        private Position _currentPosition;

        public ActionHelper(Board<Position, TPiece> board, Grid<Position> grid, TPiece piece, TCard card, Position currentPosition)
        {
            _board = board;
            this._piece = piece;
            this._grid = grid;
            this._card = card;
            this._currentPosition = currentPosition;
        }
        public ActionHelper<TPiece, TCard> MoveEverywhere(int boardRadius)
        {
            var nextQCoordinate = -boardRadius;
            var nextRCoordinate = -boardRadius;
            for (int i = -boardRadius; i <= boardRadius; i++)
            {
                for (int j = -boardRadius; j < boardRadius; j++)
                {
                    if (_grid.TryGetPositionAt(nextQCoordinate, nextRCoordinate, out var position))
                    {
                        var hasPiece = _board.TryGetPieceAt(position, out var piece);
                        if (!hasPiece)
                            _validPositions.Add(position);
                    }
                    nextRCoordinate += 1;
                }
                nextQCoordinate += 1;
                nextRCoordinate = -boardRadius;
            }


            return this;
        }

        public ActionHelper<TPiece, TCard> Move(int qOffset, int rOffset, int numTiles = int.MaxValue, params Validator[] validators)
        {
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
                _validPositions.Add(nextPosition);

                nextQCoordinate +=  qOffset;
                nextRCoordinate +=  rOffset;


                hasNextPosition = _grid.TryGetPositionAt(nextQCoordinate, nextRCoordinate, out nextPosition);

                
                step++;

            }

            return this;
        }

        public ActionHelper<TPiece, TCard> ValidMove( int qOffset, int rOffset, int numTiles = int.MaxValue,   params Validator[] validators)
        {
            if (!_board.TryGetPositionOf(_piece, out var position))
                return this;

            if (!_grid.TryGetCoordinateOf(position, out var coordinate))
                return this;

            var nextQCoordinate = coordinate.q + qOffset;
            var nextRCoordinate = coordinate.r + rOffset;

            var step = 0;
            var hasNextPosition = _grid.TryGetPositionAt(nextQCoordinate, nextRCoordinate, out var nextPosition);
            var temporaryPositions = new List<Position>();

            while (hasNextPosition && step < numTiles)
            {
                var isOk = validators.All((v) => v(_board, _grid, _piece, nextPosition));
                if (!isOk)
                    return this;

                var hasPiece = _board.TryGetPieceAt(nextPosition, out var nextPiece);
                temporaryPositions.Add(nextPosition);

                nextQCoordinate += qOffset;
                nextRCoordinate += rOffset;


                hasNextPosition = _grid.TryGetPositionAt(nextQCoordinate, nextRCoordinate, out nextPosition);


                step++;

            }
            for (int i = 0; i < temporaryPositions.Count; i++)
            {
                var pos = temporaryPositions[i];
                if(pos == _currentPosition)
                    _validPositions.AddRange(temporaryPositions);
            }
                

            return this;
        }
        public ActionHelper<TPiece, TCard> OnPosition(params Validator[] validators)
        {
            _validPositions.Add(_currentPosition);
            return this;
        }

        public ActionHelper<TPiece, TCard> HalfCircle( params Validator[] validators)
        {
            _validPositions.Add(_currentPosition);

            if (!_grid.TryGetCoordinateOf(_currentPosition, out var coordinate))
                return this;
            if (!_grid.TryGetCoordinateOf(_grid.PlayerPos, out var centerCoordinate))
                return this;

            var centeredCoordinate = (coordinate.q - centerCoordinate.q, coordinate.r - centerCoordinate.r);

            var q = centeredCoordinate.Item1;
            var r = centeredCoordinate.Item2;
            var s = -q-r;

            var centeredLeft = (-r, -s, -q);
            var centeredRight = (-s, -q, -r);

            var left = (centeredLeft.Item1 + centerCoordinate.q, centeredLeft.Item2 + centerCoordinate.r);
            var right = (centeredRight.Item1 + centerCoordinate.q, centeredRight.Item2 + centerCoordinate.r);

            if(_grid.TryGetPositionAt(left.Item1, left.Item2, out Position leftPosition))
                _validPositions.Add(leftPosition);
            if(_grid.TryGetPositionAt(right.Item1, right.Item2, out Position rightPosition))
                _validPositions.Add(rightPosition);

            return this;
        }

        public delegate bool Validator(Board<Position, TPiece> board, Grid<Position> grid, TPiece piece, Position position);
        internal ActionHelper<TPiece, TCard> TopRight(int numTiles = int.MaxValue, params Validator[] validators)
           => Move(0, 1, numTiles, validators);

        internal ActionHelper<TPiece, TCard> Right(int numTiles = int.MaxValue, params Validator[] validators)
            => Move(1, 0, numTiles, validators);

        internal ActionHelper<TPiece, TCard> BottemRight(int numTiles = int.MaxValue, params Validator[] validators)
            => Move(1, -1, numTiles, validators);

        internal ActionHelper<TPiece, TCard> BottemLeft(int numTiles = int.MaxValue, params Validator[] validators)
            => Move(0, -1, numTiles, validators);

        internal ActionHelper<TPiece, TCard> Left(int numTiles = int.MaxValue, params Validator[] validators)
            => Move(-1, 0, numTiles, validators);

        internal ActionHelper<TPiece, TCard> TopLeft(int numTiles = int.MaxValue, params Validator[] validators)
            => Move(-1, 1, numTiles, validators);

        internal ActionHelper<TPiece, TCard> TempTopRight(int numTiles = int.MaxValue, params Validator[] validators)
           => ValidMove( 0, 1, numTiles, validators);

        internal ActionHelper<TPiece, TCard> TempRight( int numTiles = int.MaxValue, params Validator[] validators)
            => ValidMove( 1, 0, numTiles, validators);

        internal ActionHelper<TPiece, TCard> TempBottemRight( int numTiles = int.MaxValue, params Validator[] validators)
            => ValidMove( 1, -1, numTiles, validators);

        internal ActionHelper<TPiece, TCard> TempBottemLeft( int numTiles = int.MaxValue, params Validator[] validators)
            => ValidMove( 0, -1, numTiles, validators);

        internal ActionHelper<TPiece, TCard> TempLeft( int numTiles = int.MaxValue, params Validator[] validators)
            => ValidMove( -1, 0, numTiles, validators);

        internal ActionHelper<TPiece, TCard> TempTopLeft( int numTiles = int.MaxValue, params Validator[] validators)
            => ValidMove( -1, 1, numTiles, validators);

        internal ActionHelper<TPiece, TCard> Everywhere(int boardRadius)
            => MoveEverywhere(boardRadius);

        internal List<Position> Collect()
        {
            return _validPositions;
        }


        public static bool IsEmptyTile(Board<Position, TPiece> board, Grid<Position> grid, TPiece piece, Position position)
        =>  !board.TryGetPieceAt(position, out _);

        public static bool HasEnemyPiece(Board<Position, TPiece> board, Grid<Position> grid, TPiece piece, Position position)
        => board.TryGetPieceAt(position, out var enemyPiece);
            

    }
}