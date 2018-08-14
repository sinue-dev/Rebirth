using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rebirth.Prototype
{
    public class BagSlot
    {
        private Stack<ItemBase> mItemStack = new Stack<ItemBase>();

        private int mId = 0;

        public BagSlot(int id)
        {
            mId = id;
        }

        public int Id
        {
            get { return mId; }
        }

        public void AddItem(ItemBase item)
        {
            item.Slot = this;
            mItemStack.Push(item);
        }

        public ItemBase FirstItem
        {
            get
            {
                if (IsEmpty)
                    return null;

                return mItemStack.Peek();
            }
        }

        public bool IsStackable(ItemBase item)
        {
            if (IsEmpty)
                return false;

            ItemBase first = mItemStack.Peek();

            if (first.Name == item.Name)
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

        public bool Remove(ItemBase item)
        {
            if (IsEmpty)
                return false;

            ItemBase first = mItemStack.Peek();
            if (first.Name == item.Name)
            {
                mItemStack.Pop();
                return true;
            }
            return false;
        }
    }
}