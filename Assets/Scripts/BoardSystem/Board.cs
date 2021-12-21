using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DAE.Commons;

namespace DAE.BoardSystem
{  
    public class PlacedEventArgs<TPosition, TPiece> : EventArgs
    {
        public TPosition ToPosition { get; }

        public TPiece Piece { get; }

        public PlacedEventArgs(TPosition toPosition, TPiece piece)
        {
            ToPosition = toPosition;
            Piece = piece;
        }
    }

    public class MovedEventArgs<TPosition, TPiece> : EventArgs
    {
        public MovedEventArgs(TPosition toPosition, TPosition fromPosition, TPiece piece)
        {
            ToPosition = toPosition;
            FromPosition = fromPosition;
            this.piece = piece;
        }

        public TPosition ToPosition { get; }
        public TPosition FromPosition { get; }
        public TPiece piece { get; }

    }

    public class TakeEventArgs<TPosition, TPiece> : EventArgs
    {
        public TPosition FromPosition { get; }
        public TPiece piece { get; }

        public TakeEventArgs(TPosition fromPosition, TPiece piece)
        {
            FromPosition = fromPosition;
            this.piece = piece;
        }
    }

    public class Board<TPosition, TPiece>
    {
        public event EventHandler<TakeEventArgs<TPosition, TPiece>> Taken;
        
        public event EventHandler<MovedEventArgs<TPosition, TPiece>> Moved;
        
        public event EventHandler<PlacedEventArgs<TPosition, TPiece>> Placed;
        
        private BidirectionalDictionary<TPosition, TPiece> _positionPiece = new BidirectionalDictionary<TPosition, TPiece>();
        public bool Place(TPiece piece, TPosition toPosition)
        {
            if (TryGetPieceAt(toPosition, out _))
                return false;
       
            if (TryGetPositionOf(piece, out _))
                return false;
       
            _positionPiece.Add(toPosition, piece);
       
            return true;
        }
       
        public bool TryMove(TPiece piece ,TPosition toPosition)
        {
            if (TryGetPieceAt(toPosition, out _))
                return false;
       
            if (!TryGetPositionOf(piece, out var fromPosition) || !_positionPiece.Remove(piece))
                return false;
       
            _positionPiece.Add(toPosition, piece);
            OnMoved(new MovedEventArgs<TPosition, TPiece>(toPosition, fromPosition, piece));

            return true;
        }
       
        public bool TryTake(TPiece piece)
        {
            if(!TryGetPositionOf(piece, out var fromPosition))
            {
                return false;               
            }

            if(!_positionPiece.Remove(piece))
            {
                return false;
            }

            OnTaken(new TakeEventArgs<TPosition, TPiece>(fromPosition, piece));
            return true;
        }
       
        public bool TryGetPieceAt(TPosition position, out TPiece piece)
            => _positionPiece.TryGetValue(position, out piece);
       
        public bool TryGetPositionOf(TPiece piece, out TPosition position)
            => _positionPiece.TryGetKey(piece, out position);


        #region EventTriggers
        protected virtual void OnTaken(TakeEventArgs<TPosition, TPiece> eventArgs)
        {
            var handler = Taken;
            handler.Invoke(this, eventArgs);
        }
        protected virtual void OnMoved(MovedEventArgs<TPosition, TPiece> eventArgs)
        {
            var handler = Moved;
            handler.Invoke(this, eventArgs);
        }
        protected virtual void OnPlaced(PlacedEventArgs<TPosition, TPiece> eventArgs)
        {
            var handler = Placed;
            handler.Invoke(this, eventArgs);
        }

        #endregion
    }

}
