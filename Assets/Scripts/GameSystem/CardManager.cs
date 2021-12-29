using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAE.HexSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Random = System.Random;

namespace DAE.GameSystem
{
    

    class CardManager : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> _cardPrefabs;
        [SerializeField]
        private GameObject _cardHand;

        Random random = new Random();

        public List<Card> GenerateDeck(int decksize)
        {
            List<Card> deck = new List<Card>();
            for (int i = 0; i < decksize; i++)
            {
                int r = random.Next(0, _cardPrefabs.Count);
                Card card = GenerateCard(r);
                deck.Add(card);
                card.DisableView();
            }
            return deck;

        }

        public void CardDraw()
        {
            var card = this.gameObject.transform.GetChild(0);
            card.SetParent(_cardHand.transform);
            card.transform.SetAsLastSibling();
            card.GetComponent<Card>().EnableView();
        }

        private Card GenerateCard(int r)
        {
            for (int i = 0; i < _cardPrefabs.Count; i++)
            {
                if (r == i)
                {
                    var card = Instantiate(_cardPrefabs[i], this.gameObject.transform);
                    card.transform.SetAsLastSibling();
                    i = _cardPrefabs.Count;
                    return card.GetComponent<Card>();
                }
            }
            return null;

        }
    }
}
