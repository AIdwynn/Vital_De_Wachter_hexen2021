using DAE.BoardSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAE.HexSystem.Moves
{
    internal class Push<TPiece, TCard> : BaseAction<TPiece, TCard>
        where TPiece : IPiece where TCard : ICard
    {

        public Push(PositionCollector positionCollector) : base(positionCollector) {}

        public override void Execute(Board<Position, TPiece> board, Grid<Position> grid, TPiece piece, TCard card, Position position)
        {
            foreach (var pos in _positionCollector(board, grid, piece, card, position))
            {
                if (board.TryGetPieceAt(pos, out var toPiece))
                {
                    if (!TryPush(toPiece,board, grid, pos))
                        board.TryTake(toPiece, false);
                }

            }
        }

        private bool TryPush(TPiece toPiece, Board<Position, TPiece> board, Grid<Position> grid, Position position)
        {
            if (!grid.TryGetCoordinateOf(position, out var coordinate))
                return false;
            if (!grid.TryGetCoordinateOf(grid.PlayerPos, out var centerCoordinate))
                return false;

            var offsetCoordinate = (coordinate.q - centerCoordinate.q, coordinate.r - centerCoordinate.r);
            var newCoordinate = (coordinate.q + offsetCoordinate.Item1, coordinate.r + offsetCoordinate.Item2);
            if (grid.TryGetPositionAt(newCoordinate.Item1, newCoordinate.Item2, out var newPos))
            {
                board.TryMove(toPiece, newPos);
                return true;
            }

            return false;
        }

        public override List<Position> Positions(Board<Position, TPiece> board, Grid<Position> grid, TPiece piece, TCard card, Position currentPosition)
                    => _positionCollector(board, grid, piece, card, currentPosition);
    }
}
