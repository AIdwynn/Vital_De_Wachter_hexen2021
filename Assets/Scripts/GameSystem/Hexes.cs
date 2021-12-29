using System;
using System.Collections;
using System.Collections.Generic;
using DAE.HexSystem;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace DAE.GameSystem
{
    public class HexEventArgs : EventArgs
    {
        public Position Position { get; }

        public HexEventArgs(Position position)
        {
            Position = position;
        }
    }

    public class DropEventArgs : EventArgs
    {
        public Card Card { get; }

        public DropEventArgs(Card card)
        {
            Card = card;
        }
    }

    public class Hexes : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
    {
        [SerializeField]
        private UnityEvent _onActivate;
        [SerializeField]
        private UnityEvent _onDeactivate;
        //[SerializeField]
        //private GameLoop loop;
        public EventHandler<HexEventArgs> StartHover;
        public EventHandler<HexEventArgs> EndHover;
        public EventHandler<DropEventArgs> Drop;


        private Position _model;
        public Position Model
        {
            get
            {
                return _model;
            }
            set
            {
                if (_model != null)
                {
                    _model.Activated -= PositionActivated;
                    _model.Deactivated -= PositionDeactivated;
                }
                _model = value;

                if (_model != null)
                {
                    _model.Activated += PositionActivated;
                    _model.Deactivated += PositionDeactivated;
                }
            }
        }

        private void PositionDeactivated(object sender, EventArgs e)
        => _onDeactivate.Invoke();

        private void PositionActivated(object sender, EventArgs e)
        => _onActivate.Invoke();

        public void OnHoverStart(HexEventArgs eventArgs)
        {
            var handler = StartHover;
            handler?.Invoke(this, eventArgs);
        }

        public void OnHoverStop(HexEventArgs eventArgs)
        {
            var handler = EndHover;
            handler?.Invoke(this, eventArgs);
        }

        public void OnPointerEnter(PointerEventData eventData)
        => OnHoverStart(new HexEventArgs(Model));
        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        => OnHoverStop(new HexEventArgs(Model));

        public void OnDrop(PointerEventData eventData)
        => OnDropHandle(new DropEventArgs(eventData.pointerDrag.GetComponent<Card>()));

        private void OnDropHandle(DropEventArgs eventArgs)
        {
            var handler = Drop;
            handler?.Invoke(this, eventArgs);
        }
    }
}

