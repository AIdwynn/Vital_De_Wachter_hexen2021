using DAE.BoardSystem;
using DAE.HexSystem.Moves;
using DAE.Commons;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DAE.HexSystem
{
    public class ActionManager<TPiece, TCard>
        where TPiece : IPiece where TCard : ICard
    {
        private MultiValueDictionary<CardType, IMove<TPiece>> _moves = new MultiValueDictionary<CardType, IMove<TPiece>>();
        private readonly Board<Position, TPiece> _board;
        private readonly Grid<Position> _grid;
        
        public ActionManager(Board<Position, TPiece> board, Grid<Position> grid)
        {
            _board = board;
            _grid = grid;

            InitializeMoves();
        }

        public List<Position> ValidPositionOf(TPiece piece, TCard card)
        {
            var a = _moves[card.Type];

            var a1 = a.Where(m => m.CanExecute(_board, _grid, piece));
            var b = a1.SelectMany(m => m.Positions(_board, _grid, piece));
            var c = b.ToList() ;
            return c;

        }
        public void Move(TPiece piece, TCard card, Position position)
        {
             _moves[piece.Type]
                .Where(m => m.CanExecute( _board, _grid, piece))
                .First(m => m.Positions( _board, _grid, piece).Contains(position))
                .Execute(_board, _grid, piece, position);
        }

        private void InitializeMoves()
        {               
            _moves.Add(CardType.Laser, new ConfigurableMove<TPiece>(
                (b,g,p) => new MovementHelper<TPiece>(b,g,p)
                .TopRight(12)
                .Right(12)
                .BottemRight(12)
                .BottemLeft(12)
                .Left(12)
                .TopLeft(12)
                .Collect()));

        }
    }
}
