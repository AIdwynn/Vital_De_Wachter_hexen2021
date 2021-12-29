using DAE.BoardSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAE.HexSystem
{
    interface IAction<TPiece, TCard>
        where TPiece : IPiece where TCard : ICard
    {
        bool CanExecute(Board<Position, TPiece> board, Grid<Position> grid, TPiece piece, TCard card);
        void Execute(Board<Position, TPiece> board, Grid<Position> grid, TPiece piece, TCard card, Position position);

        List<Position> Positions(Board<Position, TPiece> board, Grid<Position> grid, TPiece piece, TCard card, Position currentPosition);
    }
}
