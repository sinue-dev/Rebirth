using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rebirth.Prototype
{
	public class BaseContainer : MonoBehaviour
	{
		public int ContainerSlots = 8;
		public List<BagSlot> mSlots = new List<BagSlot>();

		public event EventHandler<BagEventArgs> ItemAdded;
		public event EventHandler<BagEventArgs> ItemRemoved;
		public event EventHandler<BagEventArgs> ItemUsed;

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

					if (ItemAdded != null)
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
