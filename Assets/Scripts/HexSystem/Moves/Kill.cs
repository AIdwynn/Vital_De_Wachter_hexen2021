using DAE.BoardSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAE.HexSystem.Moves
{
    internal class Kill<TPiece, TCard> : BaseAction<TPiece, TCard>
        where TPiece : IPiece where TCard : ICard
    {
        public Kill(PositionCollector positionCollector) : base(positionCollector) {}

        public override void Execute(Board<Position, TPiece> board, Grid<Position> grid, TPiece piece, TCard card, Position position)
        {
            foreach (var pos in _positionCollector(board, grid, piece, card, position))
            {
                if (board.TryGetPieceAt(pos, out var toPiece))
                    board.TryTake(toPiece, false);
            }
           
        }

        public override List<Position> Positions(Board<Position, TPiece> board, Grid<Position> grid, TPiece piece, TCard card, Position currentPosition)
            => _positionCollector(board, grid, piece, card, currentPosition);

    }
}
