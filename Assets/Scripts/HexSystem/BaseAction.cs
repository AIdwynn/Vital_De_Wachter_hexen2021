using DAE.BoardSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAE.HexSystem
{
    abstract class BaseAction<TPiece, TCard> : IAction<TPiece, TCard>
        where TPiece : IPiece where TCard : ICard
    {
        public delegate List<Position> PositionCollector(Board<Position, TPiece> board, Grid<Position> grid, TPiece piece, TCard card, Position currentPosition);

        protected PositionCollector _positionCollector;

        public BaseAction(PositionCollector positionCollector)
        {
            _positionCollector = positionCollector;
        }


        public bool CanExecute(Board<Position, TPiece> board, Grid<Position> grid, TPiece piece, TCard card)
        =>  true;


        public virtual void Execute(Board<Position, TPiece> board, Grid<Position> grid, TPiece piece, TCard card, Position position)
        {
            if (board.TryGetPieceAt(position, out var toPiece))
                board.TryTake(toPiece);
            board.TryMove(piece, position);
        }
        public abstract List<Position> Positions(Board<Position, TPiece> board, Grid<Position> grid, TPiece piece, TCard card, Position currentPosition);

    }
}
