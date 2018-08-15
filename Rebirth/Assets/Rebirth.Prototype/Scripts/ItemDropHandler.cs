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
            bag = GameManager.singleton.Hud.Bag;
        }

        public void OnDrop(PointerEventData eventData)
        {
            RectTransform invPanel = transform as RectTransform;

            if (!RectTransformUtility.RectangleContainsScreenPoint(invPanel, Input.mousePosition))
            {

                Item item = eventData.pointerDrag.gameObject.GetComponent<ItemDragHandler>().Item;
                if (item != null)
                {
                    bag.RemoveItem(item);
                    item.OnDrop();
                }

            }
        }
    }
}