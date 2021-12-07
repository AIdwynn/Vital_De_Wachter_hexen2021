using DAE.BoardSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAE.ChessSystem.Moves
{
    class PawnDoubleMove<TPiece> : MoveBase<TPiece>
        where TPiece: IPiece
    {
        public override bool CanExecute(Board<Position, TPiece> board, Grid<Position> grid, TPiece piece)
        {
            if (piece.Moved)
                return false;

            if (!board.TryGetPositionOf(piece, out var position))
                return false;
            if (!grid.TryGetCoordinateOf(position, out var coordinate))
                return false;
            
            int yPlus1 = coordinate.r + ((piece.PlayerID == 0) ? 1 : -1);
            if (!IsEmptyPosition(coordinate.q, yPlus1, board, grid, piece))
                return false;
            int yPlus2 = coordinate.r + ((piece.PlayerID == 0) ? 2 : -2);
            if (!IsEmptyPosition(coordinate.q, yPlus2, board, grid, piece))
                return false;

            if (!IsOnPawnStartingRow( piece, coordinate))
                return false;

            return base.CanExecute(board, grid, piece);
        }

        private bool IsOnPawnStartingRow(TPiece piece, (int x, int y) coordinate)
        =>  (piece.PieceType == 0 && coordinate.y == 1) || (piece.PlayerID == 1 && coordinate.y == 6);
        

        public override List<Position> Positions(Board<Position, TPiece> board, Grid<Position> grid, TPiece piece)
        {
            if(!board.TryGetPositionOf(piece, out var position))
                return new List<Position>(0);
            if (!grid.TryGetCoordinateOf(position, out var coordinate))
                return new List<Position>(0);

            coordinate.r += (piece.PlayerID == 0) ? 2 : -2;

            if (grid.TryGetPositionAt(coordinate.q, coordinate.r, out var newPosition))
                return new List<Position>() { newPosition };
            else
                return new List<Position>(0);
        }

        private bool IsEmptyPosition(int x, int y, Board<Position, TPiece> board, Grid<Position> grid, TPiece piece)
        {
           
            if (!grid.TryGetPositionAt(x, y, out var position))
                return false;
            if (!board.TryGetPieceAt(position, out _))
                return false;

            return true;
        }
    }
}
