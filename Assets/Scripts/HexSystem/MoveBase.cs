using DAE.BoardSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAE.HexSystem
{
    abstract class MoveBase<TPiece, TCard> : IMove<TPiece>
        where TPiece : IPiece where TCard : ICard
    {
        public virtual bool CanExecute(Board<Position, TPiece> board, Grid<Position> grid, TPiece piece)
        {
            return true;

        }

        public virtual void Execute(Board<Position, TPiece> board, Grid<Position> grid, TPiece piece, Position position)
        {
            if (board.TryGetPieceAt(position, out var toPiece))
                board.TryTake(toPiece);
            board.TryMove(piece, position);
        }

        public abstract List<Position> Positions(Board<Position, TPiece> board, Grid<Position> grid, TPiece piece);
        
    }
}
