﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rebirth.Prototype
{
	public class Bag : BaseContainer
	{
		UIBagPanel UIBagPanel;
		RebirthPlayerController character;

		public void Start()
		{
			character = GetComponent<RebirthPlayerController>();
			UIBagPanel = GameManager.singleton.Hud.UIBagPanel;
			for (int i = 0; i < UIBagPanel.transform.childCount; i++)
			{
				GameObject slot = UIBagPanel.transform.GetChild(i).gameObject;
				BagSlot bagSlot = slot.AddComponent<BagSlot>();
				bagSlot.InitSlot(i);
				mSlots.Add(bagSlot);
			}

			ItemUsed += UIBag_ItemUsed;
			ItemAdded += UIBag_ItemAdded;
			ItemRemoved += UIBag_ItemRemoved;
		}


		// Add Left und Right Hand Container !

		#region Bag

		private void UIBag_ItemUsed(object sender, BagEventArgs e)
		{
			//        if (e.Item.ItemInfo.ItemType == ItemTypes_e.WEAPON)
			//        {
			//e.Item.OnUse();

			//            // If the player carries an item, un-use it (remove from player's hand)
			//            //if (LeftHandItem != null)
			//            //{
			//            //    SetItemActive(LeftHandItem, false, EntityLeftHand);
			//            //}

			//            //Item item = e.Item;

			//            //// Use item (put it to hand of the player)
			//            //SetItemActive((Weapon)item, true, EntityLeftHand);
			//            //LeftHandItem = (Weapon)e.Item;
			//        }
		}

		private void UIBag_ItemAdded(object sender, BagEventArgs e)
		{
			int index = -1;
			foreach (Transform slot in UIBagPanel.transform)
			{
				index++;

				// Border... Image
				Transform imageTransform = slot.GetChild(0).GetChild(0);
				Transform textTransform = slot.GetChild(0).GetChild(1);
				Image image = imageTransform.GetComponent<Image>();
				Text txtCount = textTransform.GetComponent<Text>();
				ItemDragHandler itemDragHandler = imageTransform.GetComponent<ItemDragHandler>();

				if (index == e.Item.Slot.Id)
				{
					image.enabled = true;
					image.sprite = e.Item.ItemInfo.Image;
					image.type = Image.Type.Filled;

					int itemCount = e.Item.Slot.Count;
					if (itemCount > 1)
						txtCount.text = itemCount.ToString();
					else
						txtCount.text = "";


					// Store a reference to the item
					itemDragHandler.Item = e.Item;

					break;
				}
			}
		}

		//private void Bag_ItemRemoved(object sender, BagEventArgs e)
		//{
		//    ItemBase item = e.Item;

		//    GameObject goItem = (item as MonoBehaviour).gameObject;
		//    goItem.SetActive(true);
		//    goItem.transform.parent = null;

		//}

		private void UIBag_ItemRemoved(object sender, BagEventArgs e)
		{
			int index = -1;
			foreach (Transform slot in UIBagPanel.transform)
			{
				index++;

				Transform imageTransform = slot.GetChild(0).GetChild(0);
				Transform textTransform = slot.GetChild(0).GetChild(1);

				Image image = imageTransform.GetComponent<Image>();
				Text txtCount = textTransform.GetComponent<Text>();

				ItemDragHandler itemDragHandler = imageTransform.GetComponent<ItemDragHandler>();

				// We found the item in the UI
				if (itemDragHandler.Item == null)
					continue;

				// Found the slot to remove from
				if (e.Item.Slot.Id == index)
				{
					int itemCount = e.Item.Slot.Count;
					itemDragHandler.Item = e.Item.Slot.FirstItem;

					if (itemCount < 2)
					{
						txtCount.text = "";
					}
					else
					{
						txtCount.text = itemCount.ToString();
					}

					if (itemCount == 0)
					{
						image.enabled = false;
						image.sprite = null;
					}
					break;
				}

			}
		}
		public void SetItemActive(Weapon item, bool active, GameObject Hand)
		{
			item.transform.parent = active ? Hand.transform : null;
			item.Toggle(active);

			//GameObject currentItem = (item as MonoBehaviour).gameObject;
			//currentItem.SetActive(active);

		}

		private void DropItem(Item item)
		{
			// _animator.SetTrigger("tr_drop");

			//GameObject goItem = (LeftHandItem as MonoBehaviour).gameObject;

			//GameManager.singleton.Hud.Bag.RemoveItem(item);

			//// Throw animation
			//Rigidbody rbItem = goItem.AddComponent<Rigidbody>();
			//if (rbItem != null)
			//{
			//    rbItem.AddForce(transform.forward * 2.0f, ForceMode.Impulse);

			//    Invoke("DoDropItem", 0.25f);
			//}
		}

		public void DoDropItem()
		{
			// Remove Rigidbody
			Destroy((character.LeftHandItem as MonoBehaviour).GetComponent<Rigidbody>());

			character.LeftHandItem = null;
		}
		#endregion
	}
}