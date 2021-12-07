using System;
using System.Collections;
using System.Collections.Generic;
using DAE.ChessSystem;
using DAE.GameSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class Hexes : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private UnityEvent _onActivate;
    [SerializeField]
    private UnityEvent _onDeactivate;
    //[SerializeField]
    //private GameLoop loop;

    private Position _model;
    public Position Model { 
        get
        {
            return _model;
        }
        set
        {
            if(_model != null)
            {
                _model.Activated -= PositionActivated;
                _model.Deactivated -= PositionDeactivated;
            }
            _model = value;

            if(_model != null)
            {
                _model.Activated += PositionActivated;
                _model.Deactivated += PositionDeactivated;
            }
        } }

    private void PositionDeactivated(object sender, EventArgs e)
    => _onDeactivate.Invoke();

    private void PositionActivated(object sender, EventArgs e)
    => _onActivate.Invoke();

    public void OnPointerClick(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
    }
}
