using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAE.BoardSystem;

namespace DAE.HexSystem.Moves
{
    internal class Moving<TPiece, TCard> : BaseAction<TPiece, TCard>
        where TPiece : IPiece where TCard : ICard
    {
        public Moving(PositionCollector positionCollector) : base(positionCollector) { }
        public override void Execute(Board<Position, TPiece> board, Grid<Position> grid, TPiece piece, TCard card, Position position)
        {
            board.TryMove(piece, position);
            grid.PlayerPos = position;
        }

        public override List<Position> Positions(Board<Position, TPiece> board, Grid<Position> grid, TPiece piece, TCard card, Position currentPosition)
            => _positionCollector(board, grid, piece, card, currentPosition);
    }
}
