using DAE.ChessSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[Serializable]
public class HighlightEvent : UnityEvent<bool>
{

}
class PieceEventArgs : EventArgs
{
    public Piece Piece { get; }

    public PieceEventArgs(Piece piece)
       => Piece = piece;
    
}

class Piece : MonoBehaviour, IPointerClickHandler, IPiece
{
    public event EventHandler<PieceEventArgs> Clicked;

    [SerializeField]
    private HighlightEvent OnHighlight;
    [SerializeField]
    private int _playerID;
    [SerializeField]
    private PieceType _pieceType;
    public bool Highlight 
    {  
        set
        {
            OnHighlight.Invoke(value);
        }
            
    }  

    public int PlayerID => _playerID;

    public bool Moved { get; set; }

    public PieceType PieceType => _pieceType;

  
    public void OnPointerClick(PointerEventData eventData)
        =>  OnClicked(this, new PieceEventArgs(this));

    public override string ToString()
    {
        return gameObject.name;
    }

    protected virtual void OnClicked(object source, PieceEventArgs e)
    {
        var handle = Clicked;
        handle?.Invoke(this, e);
    }
}