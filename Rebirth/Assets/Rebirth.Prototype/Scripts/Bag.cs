using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rebirth.Prototype
{
    public class Bag : MonoBehaviour
    {
        private const int SLOTS = 8;

        private List<BagSlot> mSlots = new List<BagSlot>();

        public event EventHandler<BagEventArgs> ItemAdded;
        public event EventHandler<BagEventArgs> ItemRemoved;
        public event EventHandler<BagEventArgs> ItemUsed;

        public void Start()
        {
			Transform bagTransform = GameManager.singleton.Hud.UIBagPanel.transform;
			for (int i = 0; i < bagTransform.childCount; i++)
            {
				GameObject slot = bagTransform.GetChild(i).gameObject;
				BagSlot bagSlot = slot.AddComponent<BagSlot>();
				bagSlot.InitSlot(i);
                mSlots.Add(bagSlot);
            }
        }

        private BagSlot FindStackableSlot(Item item)
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

        public void AddItem(Item item)
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

        public void UseItem(Item item)
        {
            if (ItemUsed != null)
            {
                ItemUsed(this, new BagEventArgs(item));
            }
        }

		public void MoveItem(Item item, BagSlot fromSlot, BagSlot toSlot)
		{
			if (toSlot.IsEmpty)
			{
				if (fromSlot.Remove(item))
				{
					if (ItemRemoved != null)
					{
						ItemRemoved(this, new BagEventArgs(item));
					}

					toSlot.AddItem(item);

					if(ItemAdded != null)
					{
						ItemAdded(this, new BagEventArgs(item));
					}
				}
			}
			else
			{

			}
		}

        public void RemoveItem(Item item)
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