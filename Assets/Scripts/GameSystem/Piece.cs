using DAE.HexSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PieceEventArgs : EventArgs
{
    bool isPlayer;

    public PieceEventArgs(bool isPlayer)
    {
        this.isPlayer = isPlayer;
    }
}

class Piece : MonoBehaviour, IPiece
{
    public EventHandler<PieceEventArgs> Killed;
    internal void MoveTo(Vector3 worldPosition)
    {
        this.gameObject.transform.position = worldPosition;
    }

    internal void Taken( bool player)
    {
        if (player)
            Debug.Log("player tasks");
        else
            Debug.Log("Enemy killed");
        OnKilled(new PieceEventArgs(player));
        gameObject.SetActive(false);
    }

    private void OnKilled(PieceEventArgs eventArgs)
    {
        var handler = Killed;
        handler?.Invoke(this, eventArgs);
    }
}