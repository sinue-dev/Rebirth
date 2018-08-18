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
		public Bag Bag;

        private CharacterControllerCustom controller;
        public CharacterControllerCustom Controller
        {
            get { return controller; }
        }

        #region Unity Methods

        protected override void UpdateClient() // wird in Entity in Update() ausgeführt
        {
            controller.UpdateClient();

			Debug.DrawRay(EntityHead.transform.position, GameManager.singleton.WorldCamera.transform.GetComponent<CameraController>().AimVector, Color.red);

			RaycastHit hit;
			int rayLength = 10;
			if (Physics.Raycast(EntityHead.transform.position, GameManager.singleton.WorldCamera.transform.GetComponent<CameraController>().AimVector * rayLength, out hit, rayLength) && (hit.collider.gameObject.GetComponent<Container>() != null))
			{
				if(InputManager.singleton.Interact())
				{
					Container container = hit.collider.gameObject.GetComponent<Container>();
                    if (!GameManager.singleton.Hud.UIContainerPanel.State()) container.Init(); else container.Dispose();
					GameManager.singleton.Hud.UIContainerPanel.Toggle();

				}

				// Harvestable harvestable = hit.collider.gameObject.GetComponent<Harvestable>();
				//if (harvestable == null) return;

				//if (Input.GetKeyUp(KeyCode.E) && !harvestable.bIsHarvesting)
				//{
				//    HarvestResources(harvestable);
				//}
			}
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

            Bag = GetComponent<Bag>();
        }

        private void OnTriggerEnter(Collider other)
        {
            Item item = other.GetComponent<Item>();
            if (item != null)
            {
                InteractableItem = item;

                GameManager.singleton.Hud.OpenMessagePanel(item);
            }
        }

        private void OnTriggerExit(Collider other)
        {
			Item item = other.GetComponent<Item>();
            if (item != null)
            {
                GameManager.singleton.Hud.CloseMessagePanel();
                InteractableItem = null;
            }
        }
        #endregion

        


       
    }
}