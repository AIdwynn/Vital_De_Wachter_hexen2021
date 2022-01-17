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
        [SerializeField]
        private int _startingHandSize;
        [SerializeField]
        private int _deckSize;

        Random random = new Random();

        public List<Card> GenerateDeck()
        {
            List<Card> deck = new List<Card>();
            for (int i = 0; i < _deckSize; i++)
            {
                int r = random.Next(0, _cardPrefabs.Count);
                Card card = GenerateCard(r);
                deck.Add(card);
                card.DisableView();
            }
            return deck;

        }

        public void GenerateStartHand()
        {
            for (int i = 0; i < _startingHandSize; i++)
            {
                CardDraw();
            }
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

        public void RemoveCard(Card card)
        {
            Destroy(card.gameObject);
        }

        public void ShowHand()
        => _cardHand.SetActive(true);

        public void HideHand()
        => _cardHand.SetActive(false);
    }
}
