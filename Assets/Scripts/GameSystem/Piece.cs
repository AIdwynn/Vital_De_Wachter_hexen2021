using DAE.HexSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


class Piece : MonoBehaviour, IPiece
{
    [SerializeField]
    private int _playerID;
    [SerializeField]
    private CardType _cardType;

    public int PlayerID => _playerID;

    public bool Moved { get; set; }

    public CardType Type => _cardType;

    internal void MoveTo(Vector3 worldPosition)
    {
        this.gameObject.transform.position = worldPosition;
    }
}