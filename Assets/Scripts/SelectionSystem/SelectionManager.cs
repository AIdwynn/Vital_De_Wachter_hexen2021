using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DAE.SelectionSystem
{
    public class SelectableItemEventArgs<TItem> : EventArgs
    {
        public TItem SelectableItem { get; }
        

        public SelectableItemEventArgs(TItem selectableItem)
        {
            SelectableItem = selectableItem;
        }
    }
    public class SelectionManager<TSelectableItem>
 
    {
        public event EventHandler<SelectableItemEventArgs<TSelectableItem>> Selected;
        public event EventHandler<SelectableItemEventArgs<TSelectableItem>> Deselected;

        private HashSet<TSelectableItem> _selectableItems = new HashSet<TSelectableItem>();

        public IReadOnlyCollection<TSelectableItem> SelectableItems => _selectableItems;

        public bool IsSelected(TSelectableItem selectableItem)
           => _selectableItems.Contains(selectableItem);

        public bool Deselect(TSelectableItem selectableItem)
        {
            if (_selectableItems.Remove(selectableItem))
            {
                OnDeselected(new SelectableItemEventArgs<TSelectableItem>(selectableItem));
                return true;
            }
            return false;
        }

        public bool Select(TSelectableItem selectableItem)
        {
            if( _selectableItems.Add(selectableItem))
            {
                OnSelected(new SelectableItemEventArgs<TSelectableItem>(selectableItem));
                return true;
            }
            return false;
        }

        public bool Toggle(TSelectableItem selectableItem)
        {
            if (_selectableItems.Contains(selectableItem))
                return !Deselect(selectableItem);
            else
                return Select(selectableItem);
        }

        public void DeselectAll()
        {
            foreach(var selectableItem in _selectableItems.ToList())
            {
                Deselect(selectableItem);
            }
        }

        protected virtual void OnSelected( SelectableItemEventArgs<TSelectableItem> eventArgs)
        {
            var handler = Selected;
            handler?.Invoke(this, eventArgs);
        }
        protected virtual void OnDeselected(SelectableItemEventArgs<TSelectableItem> eventArgs)
        {
            var handler = Deselected;
            handler?.Invoke(this, eventArgs);
        }
    }
}


