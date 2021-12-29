using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DAE.HexSystem
{
    public class BeginDragEventArgs : EventArgs
    {
        public Card Card { get; }
        public BeginDragEventArgs(Card card)
        {
            Card = card;
        }
    }

    public class EndDragEventArgs : EventArgs
    {

    }

    public class Card: MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, ICard
    {
        [SerializeField]
        private CardType cardType;

        private GameObject _cardHand;

        public EventHandler<BeginDragEventArgs> BeginDragging;
        public EventHandler<EndDragEventArgs> EndDragging;


        private GameObject dragIcon;
        private RectTransform dragPlane;
        private Canvas canvas;

        public CardType Type => cardType;

        public void OnBeginDrag(PointerEventData eventData)
        {
            _cardHand = this.gameObject.transform.parent.gameObject;
            canvas = gameObject.GetComponentInParent<Canvas>(true);
            if (canvas == null)
                return;

            dragIcon = new GameObject("icon");
            dragIcon.transform.SetParent(canvas.transform, false);
            dragIcon.transform.SetAsLastSibling();

            var image = dragIcon.AddComponent<Image>();
            image.color = GetComponent<Image>().color;
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0.5f);

            dragIcon.transform.localScale = this.gameObject.transform.localScale;
            dragIcon.transform.localScale = new Vector3(dragIcon.transform.localScale.x, dragIcon.transform.localScale.y/2.5f, dragIcon.transform.localScale.z);

            dragPlane = canvas.transform as RectTransform;

            SetDraggedPosition(eventData);

            canvas.GetComponent<CanvasGroup>().blocksRaycasts = false;
            DisableView();

            OnBeginDragTrigger(new BeginDragEventArgs(this));
        }


        public void OnDrag(PointerEventData eventData)
        {
            if (dragIcon != null)
                SetDraggedPosition(eventData);
            SetPanelPosition(eventData);
        }     

        public void OnEndDrag(PointerEventData eventData)
        {
            if (dragIcon != null)
                Destroy(dragIcon);
            canvas.GetComponent<CanvasGroup>().blocksRaycasts = true;
            EnableView();
            OnEndDragTrigger(new EndDragEventArgs());
        }



        private void SetDraggedPosition(PointerEventData eventData)
        {
            var rt = dragIcon.GetComponent<RectTransform>();
            Vector3 globalMousePos;
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(dragPlane, eventData.position, eventData.pressEventCamera, out globalMousePos))
            {
                rt.position = globalMousePos;
                rt.rotation = dragPlane.rotation;
            }
        }

        private void SetPanelPosition(PointerEventData eventData)
        {
            
            Vector3 globalMousePos;
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(dragPlane, eventData.position, eventData.pressEventCamera, out globalMousePos))
            {
                int position = 0;
                for (int i = 0; i < _cardHand.transform.childCount; i++)
                {
                    var cardPos = _cardHand.transform.GetChild(i).transform.position;
                    if (globalMousePos.x >= cardPos.x)
                        position++;
                }

                this.gameObject.transform.SetSiblingIndex(position);
            }

        }

        public void DisableView()
        {
            gameObject.GetComponent<Image>().enabled = false;
        }

        public void EnableView()
        {
            gameObject.GetComponent<Image>().enabled = true;
        }

        private void OnBeginDragTrigger(BeginDragEventArgs dragEventArgs)
        {
            var handler = BeginDragging;
            handler.Invoke(this, dragEventArgs);
        }
        private void OnEndDragTrigger(EndDragEventArgs endDragEventArgs)
        {
            var handler = EndDragging;
            handler.Invoke(this, endDragEventArgs);
        }
    }
}
