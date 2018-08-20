using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Rebirth.Prototype
{
    public enum Weapons_e
    {
        FIST = 0,
		ONEHAND_SWORD = 1,
		ONEHAND_SHIELD = 2,
		ONEHAND_AXE = 3,
		ONEHAND_DAGGER = 4,
		ONEHAND_MACE = 5,
		ONEHAND_ITEM = 6,
		TWOHAND_SWORD = 7,
		TWOHAND_SPEAR = 8,
		TWOHAND_AXE = 9,
		TWOHAND_BOW = 10,
		TWOHAND_CROSSBOW = 11,
		TWOHAND_STAFF = 12,
		TWOHAND_ITEM = 13
	}

	public enum ItemTypes_e
	{
		ITEM,
		CONSUMABLE,
		WEAPON,
		EQUIPMENT
	}

	public enum HoldingHands_e
	{
		ONE,
		LEFT,
		RIGHT,
		BOTH
	}

	public class CharacterControllerCustom : MonoBehaviour
    {
        GameManager GM;
        RebirthPlayerController character;
        Animator anim;

        const float WALK_SPEED = .5f;
        const float RUN_SPEED = 1f;

		public float Gravity = 9.81f;
        public float MovementSpeed = 2f;
	    public float TurnSpeed = 1f;
		public float JumpTime = 2f;
		public float FallingVelocity;
        float rotationSpeed = 40f;

		public bool isDead = false;
		public bool isBlocking = false;
		public bool isGrounded = false;
		public bool isFalling = false;

		public int Jumping
		{
			get { return anim.GetInteger("Jumping"); }
			set { anim.SetInteger("Jumping", value); }
		}

        public bool IsWalking
        {
            get { return anim.GetBool("IsWalking"); }
            set { anim.SetBool("IsWalking", value); }
        }

		public bool IsMoving
		{
			get { return anim.GetBool("IsMoving"); }
			set { anim.SetBool("IsMoving", value); }
		}

		public bool IsRunning
        {
            get { return anim.GetBool("IsRunning"); }
            set { anim.SetBool("IsRunning", value); }
        }

        public bool IsArmed
        {
            get { return anim.GetBool("IsArmed"); }
            set { anim.SetBool("IsArmed", value); }
        }

		public bool IsCombat
		{
			get { return anim.GetBool("IsCombat"); }
			set { anim.SetBool("IsCombat", value); }
		}

		public bool IsAttacking
        {
            get { return anim.GetBool("IsAttacking"); }
            set { anim.SetBool("IsAttacking", value); }
        }

        public float MoveZ
        {
            get { return anim.GetFloat("v"); }
            set { anim.SetFloat("v", value); }
        }

        public float MoveX
        {
            get { return anim.GetFloat("h"); }
            set { anim.SetFloat("h", value); }
        }

		public int LeftWeapon
		{
			get { return anim.GetInteger("LeftWeapon"); }
			set { anim.SetInteger("LeftWeapon", value); }
		}

		public int RightWeapon
		{
			get { return anim.GetInteger("RightWeapon"); }
			set { anim.SetInteger("RightWeapon", value); }
		}

		public void Trigger(string name)
		{
			anim.SetTrigger(name);
		}

		void Start()
        {
            GM = GameManager.singleton;
            character = GetComponent<RebirthPlayerController>();
            anim = GetComponent<Animator>();
        }


        public virtual void UpdateClient()
        {
            if (anim == null) return;

			if (!isDead)
			{
				if (character.InteractableItem != null && InputManager.singleton.Interact())
				{
					character.InteractableItem.OnInteract();

					if (character.InteractableItem is Item)
					{
						character.Bag.AddItem(character.InteractableItem as Item);
						(character.InteractableItem as Item).OnPickup();
					}

					GameManager.singleton.Hud.CloseMessagePanel();

					character.InteractableItem = null;
				}

				if (character.LeftHandItem != null && InputManager.singleton.LeftLightAttack())
				{
					if (!EventSystem.current.IsPointerOverGameObject())
					{
						character.LeftHandItem.OnAction();
					}
				}
				if (character.RightHandItem != null && InputManager.singleton.RightLightAttack())
				{
					if (!EventSystem.current.IsPointerOverGameObject())
					{
						character.RightHandItem.OnAction();
					}
				}

				if(InputManager.singleton.ToggleCombat())
				{
					IsCombat = !IsCombat;

					if (IsArmed)
					{
						if (character.LeftHandItem != null) StartCoroutine(_SwitchWeapon(character.LeftHandItem));
						if (character.RightHandItem != null) StartCoroutine(_SwitchWeapon(character.RightHandItem));
					}
				}


				//if (Input.GetButtonDown("Dash"))
				//{
				//	Vector3 dashVelocity = Vector3.Scale(transform.forward, DashDistance * new Vector3((Mathf.Log(1f / (Time.deltaTime * _body.drag + 1)) / -Time.deltaTime), 0, (Mathf.Log(1f / (Time.deltaTime * _body.drag + 1)) / -Time.deltaTime)));
				//	_body.AddForce(dashVelocity, ForceMode.VelocityChange);
				//}

				if (InputManager.singleton.Jump() && Jumping == 0)
				{
					if (isGrounded)
					{						
						StartCoroutine(_Jump());
						//character.rb.AddForce(transform.up * Mathf.Sqrt(2f * -2f * Physics.gravity.y), ForceMode.VelocityChange); // 
					}
					else
					{
						if(isFalling)
						{
							Jumping = 2;
						}
					}
				}

				if (InputManager.singleton.Dodge())
				{
					if (!IsMoving)
						Trigger("RollForwardT");
					else
					{
						if (InputManager.singleton.MoveLeft())
							Trigger("RollLeftT");
						else if (InputManager.singleton.MoveRight())
							Trigger("RollRightT");
						else if (InputManager.singleton.MoveForward())
							Trigger("RollForwardT");
						else if (InputManager.singleton.MoveBackward())
							Trigger("RollBackwardT");
						else
							Trigger("RollForwardT");
					}
				}
			}

			Running();
        }

		IEnumerator _Jump()
		{
			Jumping = 1;
			Trigger("JumpT");

			character.rb.velocity = Vector3.zero;
			float timer = 0;

			while (InputManager.singleton.Jump() && timer < JumpTime)
			{
				float proportionCompleted = timer / JumpTime;
				Vector3 thisFrameJumpVector = Vector3.Lerp(transform.up * Mathf.Sqrt(2f * -2f * Physics.gravity.y), Vector3.zero, proportionCompleted);
				character.rb.AddForce(thisFrameJumpVector);
				timer += Time.deltaTime;
				yield return null;
			}

			if (true) // IsGrounded
				Jumping = 0;
			else
				Jumping = 2; // Falling
		}

		public virtual void FixedUpdateClient()
        {
			if (!isDead)
			{
				if (character.LeftHandItem != null && Input.GetKeyDown(KeyCode.G))
				{
					//DropItem();
				}

				Move();
			}
		}

        public virtual void LatedUpdateClient()
        {

        }

		void CheckForGrounded()
		{
			float distanceToGround;
			float threshold = .45f;
			RaycastHit hit;
			Vector3 offset = new Vector3(0, .4f, 0);
			if (Physics.Raycast((transform.position + offset), -Vector3.up, out hit, 100f))
			{
				distanceToGround = hit.distance;
				if (distanceToGround < threshold)
				{
					isGrounded = true;
				}
				else
				{
					isGrounded = false;
				}
			}
		}

		public void Attack(Weapon weapon)
		{
			if(!IsAttacking)
			{
				if (IsArmed)
				{
					if (character.LeftHandItem == weapon)
						AttackLeftWeapon(weapon);

					if (character.RightHandItem == weapon)
						AttackRightWeapon(weapon);
				}
				else
				{
					if(character.LeftHandItem == null && InputManager.singleton.LeftLightAttack())
					{
						AttackLeftFist();
					}

					if(character.RightHandItem == null && InputManager.singleton.RightLightAttack())
					{
						AttackRightFist();
					}
				}
			}
		}

        public void AttackLeftWeapon(Weapon weaponItem)
        {
			if (!IsAttacking)
            {
                if (IsArmed && (int)((CItemWeapon)weaponItem.ItemInfo).weapon == character.Controller.LeftWeapon)
                {
					character.animator.SetTrigger("AttackLMB");
					IsAttacking = true;

                    StartCoroutine(Attacking(anim.GetCurrentAnimatorStateInfo(0).length));
				}
                else if (IsArmed)
                {
                    // In Kampf-Modus aber keine Waffe ausgerüstet = Faustkampf

                }
            }           
        }

		public void AttackRightWeapon(Weapon weaponItem)
		{
			if (!IsAttacking)
			{
				if (IsArmed && (int)((CItemWeapon)weaponItem.ItemInfo).weapon == character.Controller.RightWeapon)
				{
					character.animator.SetTrigger("AttackRMB");
					IsAttacking = true;

					StartCoroutine(Attacking(anim.GetCurrentAnimatorStateInfo(0).length));
				}
			}
		}

		public void AttackRightFist()
		{

		}

		public void AttackLeftFist()
		{

		}

        IEnumerator Attacking(float length)
        {
            yield return new WaitForSeconds(length);
            IsAttacking = false;
        }

        private void Running()
        {
            if (!IsWalking || !CanWalk())
                IsRunning = false;
            else
                IsRunning = Input.GetKey(KeyCode.LeftShift);
        }

        private bool CanWalk()
        {
            return (!IsAttacking);
        }

        private void Move()
        {
            if (!CanWalk()) return;

            float x = InputManager.singleton.MoveHorizontal(); // Seitwärts
            float z = InputManager.singleton.MoveVertical(); // Vorwärts

            //float speed = new Vector2(z, x).sqrMagnitude;

            IsMoving = (z != 0 || x != 0);

            if (IsMoving && IsRunning)
            {
                MoveZ = Mathf.Clamp(z, -RUN_SPEED, RUN_SPEED);
                MoveX = Mathf.Clamp(x, -RUN_SPEED, RUN_SPEED);
            }
            else if (IsMoving && !IsRunning)
            {
                MoveZ = Mathf.Clamp(z, -WALK_SPEED, WALK_SPEED);
                MoveX = Mathf.Clamp(x, -WALK_SPEED, WALK_SPEED);
            }
            else
            {
                MoveZ = z;
                MoveX = x;
            }			

			Vector3 movement = (MoveX * transform.right * MovementSpeed * Time.fixedDeltaTime) + (MoveZ * transform.forward * MovementSpeed * Time.fixedDeltaTime);
			character.rb.MovePosition(character.rb.position + movement);

			
		}

        public void RotatePlayerToCameraDir(Quaternion dir)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, dir, Time.deltaTime * rotationSpeed);
        }

		public IEnumerator _SwitchWeapon(Weapon weaponItem)
		{
			if (IsArmed)
			{
				StartCoroutine(_UnSheathWeapon(weaponItem));
			}
			else
			{
				StartCoroutine(_SheathWeapon(weaponItem));
			}
			yield return null;
		}

		public IEnumerator _SheathWeapon(Weapon weaponItem)
		{
			anim.SetTrigger("SheathWeapon");
			// Waffen müssen ausgerüstet sein und dürfen nicht erst hier ausgerüstet werden => für später
			// Rundes Bag Menü wenn man B hält ploppt es auf und man kann was auswählen und dann geht es wieder zu
			// 
			switch (weaponItem.ItemInfo.HoldingHand)
			{
				case HoldingHands_e.ONE:



					break;
				case HoldingHands_e.LEFT:
					character.Bag.SetItemActive(weaponItem, true, character.EntityLeftHand);
					character.LeftHandItem = weaponItem;
					weaponItem.OnHoldLeft();
					LeftWeapon = (int)((CItemWeapon)weaponItem.ItemInfo).weapon;
					IsArmed = true;
					
					break;
				case HoldingHands_e.RIGHT:
					character.Bag.SetItemActive(weaponItem, true, character.EntityRightHand);
					character.RightHandItem = weaponItem;
					weaponItem.OnHoldRight();
					RightWeapon = (int)((CItemWeapon)weaponItem.ItemInfo).weapon;
					IsArmed = true;

					break;
				case HoldingHands_e.BOTH:
                    character.Bag.SetItemActive(weaponItem, true, character.EntityRightHand);
                    character.LeftHandItem = weaponItem;
                    character.RightHandItem = weaponItem;                    
                    weaponItem.OnHoldRight();
                    LeftWeapon = (int)((CItemWeapon)weaponItem.ItemInfo).weapon;
                    RightWeapon = (int)((CItemWeapon)weaponItem.ItemInfo).weapon;
                    IsArmed = true;
					break;
			}

			yield return null;
		}

		public IEnumerator _UnSheathWeapon(Weapon weaponItem)
		{
			anim.SetTrigger("UnSheathWeapon");

			switch (weaponItem.ItemInfo.HoldingHand)
			{
				case HoldingHands_e.ONE:

					break;
				case HoldingHands_e.LEFT:
					character.Bag.SetItemActive(weaponItem, false, character.EntityLeftHand);
					character.LeftHandItem = null;
					LeftWeapon = 0;
					IsArmed = false;
					break;
				case HoldingHands_e.RIGHT:					
					character.Bag.SetItemActive(weaponItem, false, character.EntityRightHand);
					character.RightHandItem = null;
					RightWeapon = 0;
					IsArmed = false;
					break;
				case HoldingHands_e.BOTH:
                    character.Bag.SetItemActive(weaponItem, false, character.EntityRightHand);
                    character.LeftHandItem = null;
                    character.RightHandItem = null;
                    LeftWeapon = 0;
                    RightWeapon = 0;
                    IsArmed = false;
					break;
			}

			yield return null;
		}

		public IEnumerator _Death()
		{
			//animator.SetTrigger("Death1Trigger");
			//StartCoroutine(_LockMovementAndAttack(.1f, 1.5f));
			//isDead = true;
			//animator.SetBool("Moving", false);
			//inputVec = new Vector3(0, 0, 0);
			yield return null;
		}

		public IEnumerator _Revive()
		{
			//animator.SetTrigger("Revive1Trigger");
			//isDead = false;
			yield return null;
		}

		#region Animation Events
		// Called by Animation Events

		public void AttackEnd(int i)
        {
            IsAttacking = false;
        }

        public void EventActivateWeaponDamage()
        {
            if (!IsArmed) return;

            Weapon weapon = character.RightHandItem.GetComponent<Weapon>();
            if (weapon != null && weapon.ItemInfo.ItemType == ItemTypes_e.WEAPON)
            {
				weapon.OnActivateWeaponDamage();
            }
        }

        public void EventDeactivateWeaponDamage()
        {
            if (!IsArmed) return;

			Weapon weapon = character.RightHandItem.GetComponent<Weapon>();
			if (weapon != null && weapon.ItemInfo.ItemType == ItemTypes_e.WEAPON)
			{
				weapon.OnDeactivateWeaponDamage();
            }
        }

        public void EventShowWeapon()
        {
			if(LeftWeapon != 0)
			{
				Weapon weapon = character.LeftHandItem.GetComponent<Weapon>();
				if (weapon != null && weapon.ItemInfo.ItemType == ItemTypes_e.WEAPON)
				{
					weapon.Toggle(IsArmed);
				}
			}

			if(RightWeapon != 0)
			{
				Weapon weapon = character.RightHandItem.GetComponent<Weapon>();
				if (weapon != null && weapon.ItemInfo.ItemType == ItemTypes_e.WEAPON)
				{
					weapon.Toggle(IsArmed);
				}
			}			
        }

        public void EventHideWeapon()
        {
			Weapon weapon = character.RightHandItem.GetComponent<Weapon>();
			if (weapon != null && weapon.ItemInfo.ItemType == ItemTypes_e.WEAPON)
			{
				weapon.Toggle(IsArmed);
            }
        }
        #endregion
    }
}