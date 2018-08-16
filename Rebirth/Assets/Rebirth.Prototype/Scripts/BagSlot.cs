using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rebirth.Prototype
{
    public class BagSlot : MonoBehaviour
    {
        private Stack<Item> mItemStack = new Stack<Item>();
        public int SlotId = 0;

        public void InitSlot(int id)
        {
			SlotId = id;
			mItemStack = new Stack<Item>();

		}

        public int Id
        {
            get { return SlotId; }
			set { SlotId = value; }
        }

        public void AddItem(Item item)
        {
            item.Slot = this;
            mItemStack.Push(item);
        }

        public Item FirstItem
        {
            get
            {
                if (IsEmpty)
                    return null;

                return mItemStack.Peek();
            }
        }

        public bool IsStackable(Item item)
        {
            if (IsEmpty)
                return false;

            Item first = mItemStack.Peek();

            if (first.ItemInfo.Name == item.ItemInfo.Name)
                return true;

            return false;
        }

        public bool IsEmpty
        {
            get { return Count == 0; }
        }

        public int Count
        {
            get { return mItemStack.Count; }
        }

        public bool Remove(Item item)
        {
            if (IsEmpty)
                return false;

            Item first = mItemStack.Peek();
            if (first.ItemInfo.Name == item.ItemInfo.Name)
            {
                mItemStack.Pop();
                return true;
            }
            return false;
        }
    }
}