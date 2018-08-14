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

        private void Bag_ItemRemoved(object sender, BagEventArgs e)
        {
            ItemBase item = e.Item;

            GameObject goItem = (item as MonoBehaviour).gameObject;
            goItem.SetActive(true);
            goItem.transform.parent = null;

        }

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