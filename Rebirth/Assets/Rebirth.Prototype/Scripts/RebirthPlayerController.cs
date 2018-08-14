using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Rebirth.Prototype
{
    #region Nutrition State Properties
    public enum HungerStates_e
    {
        Gluttony,
        Satiated,
        Peckish,
        Hungry,
        VeryHungry,
        Starving
    }

    public enum ThirstStates_e
    {
        Quenched,
        SlightlyThirsty,
        Thirsty,
        VeryThirsty,
        Dehydrated
    }

    public enum SleepStates_e
    {
        Rested,
        SlightlyTired,
        Tired,
        VeryTired,
        Exhausted
    }
    #endregion

    public class RebirthPlayerController : Entity
    {
        private CharacterControllerCustom controller;
        public CharacterControllerCustom Controller
        {
            get { return controller; }
        }

        #region Unity Methods

        protected override void UpdateClient() // wird in Entity in Update() ausgeführt
        {
            controller.UpdateClient();            
        }

        void FixedUpdate()
        {
            controller.FixedUpdateClient();            
        }

        void LateUpdate()
        {
             controller.LatedUpdateClient();
        }

        public void Init(GameObject planet)
        {
            if (planet != null)
            {
                GetComponent<GravityReceiver>().gravity = planet.GetComponent<Gravity>();
            }

            GameManager.singleton.WorldCamera.gameObject.SetActive(true);
            GameManager.singleton.WorldCamera.GetComponent<CameraController>().target = transform;

            controller = GetComponent<CharacterControllerCustom>();

            GameManager.singleton.Hud.UIActionPanel.Show();
            GameManager.singleton.Hud.UIBagPanel.Show();

            GameManager.singleton.Hud.Bag = GetComponent<Bag>();

            GameManager.singleton.Hud.Bag.ItemUsed += Bag_ItemUsed;
            GameManager.singleton.Hud.Bag.ItemAdded += Bag_ItemAdded;
            GameManager.singleton.Hud.Bag.ItemRemoved += Bag_ItemRemoved;
        }

        private void OnTriggerEnter(Collider other)
        {
            InteractableItemBase item = other.GetComponent<InteractableItemBase>();
            if (item != null)
            {
                InteractableItem = item;

                GameManager.singleton.Hud.OpenMessagePanel(InteractableItem);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            InteractableItemBase item = other.GetComponent<InteractableItemBase>();
            if (item != null)
            {
                GameManager.singleton.Hud.CloseMessagePanel();
                InteractableItem = null;
            }
        }
        #endregion

        #region Bag

        private void Bag_ItemUsed(object sender, BagEventArgs e)
        {
            if (!e.Item.IsConsumable)
            {
                // If the player carries an item, un-use it (remove from player's hand)
                if (LeftHandItem != null)
                {
                    SetItemActive(LeftHandItem, false);
                }

                ItemBase item = e.Item;

                // Use item (put it to hand of the player)
                SetItemActive(item, true);

                LeftHandItem = e.Item;
            }
        }

        private void Bag_ItemAdded(object sender, BagEventArgs e)
        {
            int index = -1;
            foreach (Transform slot in GameManager.singleton.Hud.UIBagPanel.transform)
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

        //private void Bag_ItemRemoved(object sender, BagEventArgs e)
        //{
        //    ItemBase item = e.Item;

        //    GameObject goItem = (item as MonoBehaviour).gameObject;
        //    goItem.SetActive(true);
        //    goItem.transform.parent = null;

        //}

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


        private void SetItemActive(ItemBase item, bool active)
        {
            item.Toggle(active);
            //GameObject currentItem = (item as MonoBehaviour).gameObject;
            //currentItem.SetActive(active);
            //currentItem.transform.parent = active ? PlayerLeftHand.transform : null;
        }

        private void DropCurrentItem()
        {
           // _animator.SetTrigger("tr_drop");

            GameObject goItem = (LeftHandItem as MonoBehaviour).gameObject;

            GameManager.singleton.Hud.Bag.RemoveItem(LeftHandItem);

            // Throw animation
            Rigidbody rbItem = goItem.AddComponent<Rigidbody>();
            if (rbItem != null)
            {
                rbItem.AddForce(transform.forward * 2.0f, ForceMode.Impulse);

                Invoke("DoDropItem", 0.25f);
            }
        }

        public void DoDropItem()
        {
            // Remove Rigidbody
            Destroy((LeftHandItem as MonoBehaviour).GetComponent<Rigidbody>());

            LeftHandItem = null;
        }
        #endregion  
    }
}