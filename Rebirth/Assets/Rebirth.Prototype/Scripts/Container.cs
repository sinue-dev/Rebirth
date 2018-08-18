using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rebirth.Prototype
{
	public class Container : BaseContainer
	{
		public UIContainerPanel UIContainerPanel;
		public List<Item> startingItems;

		private void Start()
		{
			ItemUsed += UIContainer_ItemUsed;
			ItemAdded += UIContainer_ItemAdded;
			ItemRemoved += UIContainer_ItemRemoved;
		}

		public void Init()
		{
			UIContainerPanel = GameManager.singleton.Hud.UIContainerPanel;
			PopulateSlots();
			PopulateItems(startingItems.ToArray());
		}

		public void PopulateSlots()
		{
            if (mSlots.Count > 0) Dispose();            

			for (int i = 0; i < ContainerSlots; i++)
			{
				GameObject slot = Instantiate(UIContainerPanel.emptySlot, UIContainerPanel.transform);
				slot.name = "ContainerSlot_" + i;

				BagSlot bagSlot = slot.AddComponent<BagSlot>();
				bagSlot.InitSlot(i);
				mSlots.Add(bagSlot);
			}
		}

		public void PopulateItems(Item[] items)
		{
			foreach (Item item in items)
			{
				AddItem(item);
			}
		}

        public override void Dispose()
        {
            mSlots.Clear();
            foreach (BagSlot slot in UIContainerPanel.GetComponentsInChildren<BagSlot>())
            {
                Destroy(slot.gameObject);
            }
        }

        #region Bag

        private void UIContainer_ItemUsed(object sender, BagEventArgs e)
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

		private void UIContainer_ItemAdded(object sender, BagEventArgs e)
		{
			int index = -1;
            foreach (BagSlot slot in UIContainerPanel.GetComponentsInChildren<BagSlot>())
			{
				index++;

				// Border... Image
				Transform imageTransform = slot.transform.GetChild(0).GetChild(0);
				Transform textTransform = slot.transform.GetChild(0).GetChild(1);
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

		private void UIContainer_ItemRemoved(object sender, BagEventArgs e)
		{
			int index = -1;
			foreach (Transform slot in UIContainerPanel.transform)
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
		#endregion
	}
}
