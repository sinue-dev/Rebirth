using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Rebirth.Prototype
{
    public class ItemDropHandler : MonoBehaviour, IDropHandler
    {
        private Bag bag;

        void Start()
        {
            bag = GameManager.singleton.LocalPlayer.Bag;
        }

        public void OnDrop(PointerEventData eventData)
        {
            RectTransform invPanel = transform as RectTransform;
			//if (!RectTransformUtility.RectangleContainsScreenPoint(invPanel, Input.mousePosition))
			if(!EventSystem.current.IsPointerOverGameObject())
			{
                Item item = eventData.pointerDrag.gameObject.GetComponent<ItemDragHandler>().Item;
                if (item != null)
                {
                    bag.RemoveItem(item);
                    item.OnDrop();
                }
            }
			else if(eventData.pointerCurrentRaycast.gameObject.transform.parent.GetComponent<BagSlot>() != null)
			{
				Item item = eventData.pointerDrag.gameObject.GetComponent<ItemDragHandler>().Item;

				BagSlot toSlot = null;
				foreach (GameObject go in eventData.hovered)
				{
					if(go.CompareTag("BagSlot"))
					{
						toSlot = go.GetComponent<BagSlot>();
					}
				}

				BagSlot fromSlot = null;
				if (eventData.pointerDrag.transform.parent.CompareTag("BagSlot"))
				{
					fromSlot = eventData.pointerDrag.transform.parent.GetComponent<BagSlot>();
				}
				else if(eventData.pointerDrag.transform.parent.parent.CompareTag("BagSlot"))
				{
					fromSlot = eventData.pointerDrag.transform.parent.parent.GetComponent<BagSlot>();
				}

				if(fromSlot != null && toSlot != null && item != null)
				{
					bag.MoveItem(item, fromSlot, toSlot);
				}				
			}
        }
    }
}