using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rebirth.Prototype
{
	public class InteractableItem : MonoBehaviour
    {
		public BagSlot Slot
		{
			get; set;
		}

		public RebirthPlayerController character
		{
			get { return GameManager.singleton.LocalPlayer; }
		}		

		public SphereCollider pickupCol;

		public virtual void OnInteract()
		{
			OnPickup();
		}

		public virtual void OnPickup()
		{
			Destroy(gameObject.GetComponent<Rigidbody>());
			Hide();
		}

		public virtual void OnUse() // override this
		{

		}

		public virtual void OnAction() // overide this
		{

		}

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

	public class Item : InteractableItem
	{
		public CItem ItemInfo;

		public virtual void OnHoldLeft()
		{
			transform.localPosition = ItemInfo.LeftHandPosition;
			transform.localEulerAngles = ItemInfo.LeftHandRotation;
		}

		public virtual void OnHoldRight()
		{
			transform.localPosition = ItemInfo.RightHandPosition;
			transform.localEulerAngles = ItemInfo.RightHandRotation;
		}

		public virtual void OnDrop()
		{
			RaycastHit hit = new RaycastHit();
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit, 1000))
			{
				gameObject.SetActive(true);
				gameObject.transform.position = hit.point;
				gameObject.transform.eulerAngles = ItemInfo.DropRotation;
			}
		}
	}

	public class Weapon : Item
	{
		public Collider damageCol;

		private void Start()
		{
			gameObject.name = ItemInfo.Name;
		}

		public virtual void OnActivateWeaponDamage()
		{
			damageCol.enabled = true;
		}

		public virtual void OnDeactivateWeaponDamage()
		{
			damageCol.enabled = false;
		}
	}	

	[CreateAssetMenu(fileName = "New Item", menuName = "Rebirth/Bag/Item")]
	public class CItem : ScriptableObject
	{
		[Header("Basic Item Info")]
		public string Name;
		public Sprite Image;
		public string InteractText = "Press E to pickup the item";
		public ItemTypes_e ItemType;
		public HoldingHands_e HoldingHand;

		[Header("Character Equip/Un-equip")]		
		public Vector3 LeftHandPosition;
		public Vector3 LeftHandRotation;

		public Vector3 RightHandPosition;
		public Vector3 RightHandRotation;

		public Vector3 DropRotation;
	}

	[CreateAssetMenu(fileName = "New Item", menuName = "Rebirth/Bag/Weapon")]
	public class CItemWeapon : CItem
	{
		public Weapons_e weapon;		

		public CItemWeapon()
		{
			ItemType = ItemTypes_e.WEAPON;
		}
	}
}