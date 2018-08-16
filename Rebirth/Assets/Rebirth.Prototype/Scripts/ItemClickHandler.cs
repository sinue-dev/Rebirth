using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rebirth.Prototype
{
    public class ItemClickHandler : MonoBehaviour
    {
        private Bag bag;

        public KeyCode _Key;

        private Button _button;

        void Awake()
        {
            _button = GetComponent<Button>();
        }

        void Start()
        {
            bag = GameManager.singleton.Hud.Bag;
        }

        void Update()
        {
            if (Input.GetKeyDown(_Key))
            {
                FadeToColor(_button.colors.pressedColor);

                // Click the button
                _button.onClick.Invoke();
            }
            else if (Input.GetKeyUp(_Key))
            {
                FadeToColor(_button.colors.normalColor);
            }
        }

        void FadeToColor(Color color)
        {
            Graphic graphic = GetComponent<Graphic>();
            graphic.CrossFadeColor(color, _button.colors.fadeDuration, true, true);
        }

        public Item AttachedItem
        {
            get
            {
                ItemDragHandler dragHandler = gameObject.transform.Find("ItemImage").GetComponent<ItemDragHandler>();
                return dragHandler.Item;
            }
        }

        public void OnItemClicked()
        {
            Item item = AttachedItem;

            if (item != null)
            {
                bag.UseItem(item);
                item.OnUse();
            }
        }

    }
}