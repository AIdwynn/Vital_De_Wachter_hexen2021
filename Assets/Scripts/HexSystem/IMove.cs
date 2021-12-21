using DAE.BoardSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAE.HexSystem
{
    interface IMove<TPiece>
        where TPiece : IPiece
    {
        bool CanExecute(Board<Position, TPiece> board, Grid<Position> grid, TPiece piece);
        void Execute(Board<Position, TPiece> board, Grid<Position> grid, TPiece piece, Position position);

        List<Position> Positions(Board<Position, TPiece> board, Grid<Position> grid, TPiece piece);
    }
}
