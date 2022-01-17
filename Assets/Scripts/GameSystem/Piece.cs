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
        gameObject.SetActive(false);
    }
}