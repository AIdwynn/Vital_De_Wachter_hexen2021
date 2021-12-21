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

    public class DropEventArgs : EventArgs
    {
        public Transform Transform { get; }

        public DropEventArgs(Transform transform)
        {
            Transform = transform;
        }
    }

    public class Card: MonoBehaviour, IBeginDragHandler, IDragHandler, IDropHandler, IEndDragHandler, ICard
    {
        [SerializeField]
        private CardType cardType;

        private GameObject _cardHand;

        public EventHandler<BeginDragEventArgs> BeginDragging;
        public EventHandler<DropEventArgs> Drop;

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

        public void OnDrop(PointerEventData eventData)
        {
            if (dragIcon != null)
                Destroy(dragIcon);
            canvas.GetComponent<CanvasGroup>().blocksRaycasts = true;
            OnDropTrigger(new DropEventArgs(transform));
        }

        public void OnEndDrag(PointerEventData eventData)
        {

            EnableView();
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

        private void DisableView()
        {
            gameObject.GetComponent<Image>().enabled = false;
        }

        private void EnableView()
        {
            gameObject.GetComponent<Image>().enabled = true;
        }

        private void OnBeginDragTrigger(BeginDragEventArgs dragEventArgs)
        {
            var handler = BeginDragging;
            handler.Invoke(this, dragEventArgs);
        }

        private void OnDropTrigger(DropEventArgs dropEventArgs)
        {
            var handler = Drop;
            handler.Invoke(this, dropEventArgs);
        }
    }
}
