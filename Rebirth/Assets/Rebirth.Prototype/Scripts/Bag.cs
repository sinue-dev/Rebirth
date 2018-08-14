﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rebirth.Prototype
{
    public class Bag : MonoBehaviour
    {
        private const int SLOTS = 8;

        private IList<BagSlot> mSlots = new List<BagSlot>();

        public event EventHandler<BagEventArgs> ItemAdded;
        public event EventHandler<BagEventArgs> ItemRemoved;
        public event EventHandler<BagEventArgs> ItemUsed;

        public Bag()
        {
            for (int i = 0; i < SLOTS; i++)
            {
                mSlots.Add(new BagSlot(i));
            }
        }

        private BagSlot FindStackableSlot(ItemBase item)
        {
            foreach (BagSlot slot in mSlots)
            {
                if (slot.IsStackable(item))
                    return slot;
            }
            return null;
        }

        private BagSlot FindNextEmptySlot()
        {
            foreach (BagSlot slot in mSlots)
            {
                if (slot.IsEmpty)
                    return slot;
            }
            return null;
        }

        public void AddItem(ItemBase item)
        {
            BagSlot freeSlot = FindStackableSlot(item);
            if (freeSlot == null)
            {
                freeSlot = FindNextEmptySlot();
            }
            if (freeSlot != null)
            {
                freeSlot.AddItem(item);

                if (ItemAdded != null)
                {
                    ItemAdded(this, new BagEventArgs(item));
                }

            }
        }

        internal void UseItem(ItemBase item)
        {
            if (ItemUsed != null)
            {
                ItemUsed(this, new BagEventArgs(item));
            }
        }

        public void RemoveItem(ItemBase item)
        {
            foreach (BagSlot slot in mSlots)
            {
                if (slot.Remove(item))
                {
                    if (ItemRemoved != null)
                    {
                        ItemRemoved(this, new BagEventArgs(item));
                    }
                    break;
                }

            }
        }
    }
}