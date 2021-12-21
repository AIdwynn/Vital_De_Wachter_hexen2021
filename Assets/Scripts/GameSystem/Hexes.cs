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

    public class Hexes : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private UnityEvent _onActivate;
        [SerializeField]
        private UnityEvent _onDeactivate;
        //[SerializeField]
        //private GameLoop loop;
        public EventHandler<HexEventArgs> StartHover;
        public EventHandler<HexEventArgs> EndHover;


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

        
    }
}

