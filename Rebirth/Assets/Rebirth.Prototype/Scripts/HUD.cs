using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rebirth.Prototype
{
    public class HUD : MonoBehaviour
    {
        public GameObject UIMainMenu;
        public UIMenu UIMenu;
        public GameObject UILogin;
        public UIActionPanel UIActionPanel;
        public UIBagPanel UIBagPanel;


        public Bag Bag;

        public GameObject MessagePanel;

        void Awake()
        {
            if(UIMenu != null)
            {
                UIMainMenu.SetActive(true);
                UIMenu.Show();
            }
        }

        // Use this for initialization
        void Start()
        {

        }

        private void Bag_ItemAdded(object sender, BagEventArgs e)
        {
            Transform bagPanel = transform.Find("BagPanel");
            int index = -1;
            foreach (Transform slot in bagPanel)
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
                    image.sprite = e.Item.Image;
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

        private void Bag_ItemRemoved(object sender, BagEventArgs e)
        {
            Transform bagPanel = transform.Find("BagPanel");

            int index = -1;
            foreach (Transform slot in bagPanel)
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

        private bool mIsMessagePanelOpened = false;

        public bool IsMessagePanelOpened
        {
            get { return mIsMessagePanelOpened; }
        }

        public void OpenMessagePanel(InteractableItemBase item)
        {
            MessagePanel.SetActive(true);

            Text mpText = MessagePanel.transform.Find("Text").GetComponent<Text>();
            mpText.text = item.InteractText;


            mIsMessagePanelOpened = true;


        }

        public void CloseMessagePanel()
        {
            MessagePanel.SetActive(false);

            mIsMessagePanelOpened = false;
        }
    }
}