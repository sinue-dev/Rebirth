using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rebirth.Prototype
{
	public class InteractableItemBase : MonoBehaviour
	{
		public string Name;

		public Sprite Image;

		public string InteractText = "Press E to pickup the item";

		public bool IsConsumable;
		public bool IsWeapon;
		public Weapons_e weapon;

		public Collider col;

		public enum NeededHandsToHold_e
		{
			ONE,
			LEFT,
			RIGHT,
			BOTH
		}

		public NeededHandsToHold_e neededHandsToHold;

        public virtual void OnInteract()
        {

        }
    }

    public class ItemBase : InteractableItemBase
    {
        public RebirthPlayerController character
		{
			get { return GameManager.singleton.LocalPlayer; }
		}

        public BagSlot Slot
        {
            get; set;
        }

        public virtual void OnUse()
        {
            
        }

        public virtual void OnHoldLeft()
        {
            transform.localPosition = PickPositionLeft;
            transform.localEulerAngles = PickRotationLeft;
        }

        public virtual void OnHoldRight()
        {
            transform.localPosition = PickPositionRight;
            transform.localEulerAngles = PickRotationRight;
        }

        public virtual void OnDrop()
        {
            RaycastHit hit = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000))
            {
                gameObject.SetActive(true);
                gameObject.transform.position = hit.point;
                gameObject.transform.eulerAngles = DropRotation;
            }
        }

        public virtual void OnPickup()
        {
            Destroy(gameObject.GetComponent<Rigidbody>());
			Hide();

        }

        public virtual void ItemAction()
        {

        }

        public Vector3 PickPositionLeft;
        public Vector3 PickRotationLeft;

		public Vector3 PickPositionRight;
		public Vector3 PickRotationRight;

        public Vector3 DropRotation;

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Toggle(bool state)
        {
            gameObject.SetActive(state);
        }

        public bool State()
        {
            return gameObject.activeSelf;
        }
    }
}