using DAE.BoardSystem;
using DAE.HexSystem.Moves;
using DAE.Commons;

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace DAE.HexSystem
{
    public class ActionManager<TPiece, TCard>
        where TPiece : IPiece where TCard : ICard
    {
        private MultiValueDictionary<CardType, IAction<TPiece, TCard>> _actions = new MultiValueDictionary<CardType, IAction<TPiece, TCard>>();
        private MultiValueDictionary<CardType, IAction<TPiece, TCard>> _validActions = new MultiValueDictionary<CardType, IAction<TPiece, TCard>>();
        private readonly Board<Position, TPiece> _board;
        private readonly Grid<Position> _grid;

        
        public ActionManager(Board<Position, TPiece> board, Grid<Position> grid)
        {
            _board = board;
            _grid = grid;

            InitializeActions();
            InitialiseValidActions();
        }

        public List<Position> ActionValidPositions(TPiece piece, TCard card, Position position)
        {
            var a = _validActions[card.Type];

            var a1 = a.Where(m => m.CanExecute(_board, _grid, piece, card));
            var b = a1.SelectMany(m => m.Positions(_board, _grid, piece, card, position));
            var c = b.ToList();

            return c;
        }
        public List<Position> AllValidPositionOf(TPiece piece, TCard card, Position position)
        {
            var a = _actions[card.Type];

            var a1 = a.Where(m => m.CanExecute(_board, _grid, piece, card));
            var b = a1.SelectMany(m => m.Positions(_board, _grid, piece, card, position));
            var c = b.ToList();
            return c;

        }
        public void PerformAction(TPiece piece, TCard card, Position position)
        {
             _validActions[card.Type]
                .Where(m => m.CanExecute(_board, _grid, piece, card))
                .First(m => m.Positions(_board, _grid, piece, card, position).Contains(position))
                .Execute(_board, _grid, piece, card, position);
        }

        private void InitializeActions()
        {               
            _actions.Add(CardType.Laser, new ConfigurableAction<TPiece, TCard>(
                (b,g,p, c, cp) => new ActionHelper<TPiece, TCard>(b,g,p, c, cp)
                .TopRight()
                .Right()
                .BottemRight()
                .BottemLeft()
                .Left()
                .TopLeft()
                .Collect()));

            _actions.Add(CardType.Slash, new ConfigurableAction<TPiece, TCard>(
                (b,g,p,c, cp) => new ActionHelper<TPiece, TCard>(b,g,p, c, cp)
                .TopRight(1)
                .Right(1)
                .BottemRight(1)
                .BottemLeft(1)
                .Left(1)
                .TopLeft(1)
                .Collect()));

            _actions.Add(CardType.Push, new ConfigurableAction<TPiece, TCard>(
                (b, g, p, c, cp) => new ActionHelper<TPiece, TCard>(b, g, p, c, cp)
                .TopRight(1)
                .Right(1)
                .BottemRight(1)
                .BottemLeft(1)
                .Left(1)
                .TopLeft(1)
                .Collect()));

            _actions.Add(CardType.Teleport, new ConfigurableAction<TPiece, TCard>(
                (b, g, p, c, cp) => new ActionHelper<TPiece, TCard>(b, g, p, c, cp)
                .Everywhere(_grid.Q, false)
                .Collect()));

            _actions.Add(CardType.Bomb, new ConfigurableAction<TPiece, TCard>(
                (b,g,p,c,cp) => new ActionHelper<TPiece, TCard>(b,g, p, c,cp)
                .Everywhere(_grid.Q, true)
                .Collect()));

        }

        private void InitialiseValidActions()
        {
            _validActions.Add(CardType.Laser, new Kill<TPiece, TCard>(
                (b, g, p, c, cp) => new ActionHelper<TPiece, TCard>(b, g, p, c, cp)
                .TempTopRight()
                .TempRight()
                .TempBottemRight()
                .TempBottemLeft()
                .TempLeft()
                .TempTopLeft()
                .Collect()));




            _validActions.Add(CardType.Slash, new Kill<TPiece, TCard>(
                (b, g, p, c, cp) => new ActionHelper<TPiece, TCard>(b, g, p, c, cp)
                .HalfCircle()
                .Collect()));

            _validActions.Add(CardType.Bomb, new Destruction<TPiece, TCard>(
                (b, g, p, c, cp) => new ActionHelper<TPiece, TCard>(b, g, p, c, cp)
                .Area()
                .Collect()));
            
            _validActions.Add(CardType.Push, new Push<TPiece, TCard>(
                (b, g, p, c, cp) => new ActionHelper<TPiece, TCard>(b, g, p, c, cp)
                .HalfCircle()
                .Collect()));

            _validActions.Add(CardType.Teleport, new Moving<TPiece, TCard>(
                (b, g, p, c, cp) => new ActionHelper<TPiece, TCard>(b, g, p, c, cp)
                .OnPosition()
                .Collect()));


        }


    }
}
