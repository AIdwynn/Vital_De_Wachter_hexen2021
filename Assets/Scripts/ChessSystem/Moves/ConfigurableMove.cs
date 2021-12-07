using DAE.BoardSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAE.ChessSystem.Moves
{
    class ConfigurableMove<TPiece> : MoveBase<TPiece>
        where TPiece : IPiece
    {
        public delegate List<Position> PositionCollector(Board<Position, TPiece> board, Grid<Position> grid, TPiece piece);

        private PositionCollector _positionCollector;

        public ConfigurableMove(PositionCollector positionCollector)
        {
            _positionCollector = positionCollector;
        }
        public override List<Position> Positions(Board<Position, TPiece> board, Grid<Position> grid, TPiece piece)
            => _positionCollector(board, grid, piece);
    }
}
