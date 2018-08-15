using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rebirth.Prototype
{
    public class BagSlot
    {
        private Stack<Item> mItemStack = new Stack<Item>();

        private int mId = 0;

        public BagSlot(int id)
        {
            mId = id;
        }

        public int Id
        {
            get { return mId; }
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