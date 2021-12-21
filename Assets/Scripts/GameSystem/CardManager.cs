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

        public void GenerateDeck(int decksize)
        {
            //Debug.Log(System.Enum.GetValues(typeof(CardType)).Length);
        }
          Random random = new Random();
        public Card CardDraw()
        {
  
            int r = random.Next(0, _cardPrefabs.Count - 1);
            Card card = GenerateCard(r);
            card.gameObject.transform.SetParent(_cardHand.transform);
            card.gameObject.transform.SetAsLastSibling();
            return card;

        }

        private Card GenerateCard(int r)
        {
            var card = new GameObject();
            for (int i = 0; i < _cardPrefabs.Count; i++)
            {
                if (r == i)
                {
                    card = Instantiate(_cardPrefabs[i]);
                    i = _cardPrefabs.Count;
                }
            }
            return card.GetComponent<Card>();

        }
    }
}
