using DAE.BoardSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAE.HexSystem.Moves
{
    class ConfigurableAction<TPiece, TCard> : BaseAction<TPiece, TCard>
        where TPiece : IPiece where TCard : ICard
    {

        public ConfigurableAction(PositionCollector positionCollector) : base(positionCollector) { }

        public override List<Position> Positions(Board<Position, TPiece> board, Grid<Position> grid, TPiece piece, TCard card, Position currentPosition)
            => _positionCollector(board, grid, piece, card, currentPosition);



    }
}
