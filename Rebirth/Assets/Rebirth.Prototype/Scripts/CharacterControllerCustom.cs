using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Rebirth.Prototype
{
	public enum Hands_e
	{
		BOTH = 0,
		LEFT = 1,
		RIGHT = 2
	}

    public enum Weapons_e
    {
        HAND = 0,
        TWOHANDSWORD = 1,
        TWOHANDSPEAR = 2,
        TWOHANDAXE = 3,
        TWOHANDBOW = 4,
        TWOHANDCROSSBOW = 5,
        STAFF = 6,
        SHIELD = 7,
        LEFT_SWORD = 8,
        RIGHT_SWORD = 9,
        LEFT_MACE = 10,
        RIGHT_MACE = 11,
        LEFT_DAGGER = 12,
        RIGHT_DAGGER = 13,
        LEFT_ITEM = 14,
        RIGHT_ITEM = 16,
        LEFT_PISTOL = 16,
        RIGHT_PISTOL = 17
    }

    public class CharacterControllerCustom : MonoBehaviour
    {
        GameManager GM;
        RebirthPlayerController character;
        Animator anim;

        const float WALK_SPEED = .5f;
        const float RUN_SPEED = 1f;

        public float MovementSpeed = 2f;
	    public float TurnSpeed = 1f;
        float rotationSpeed = 40f;

		public bool isDead = false;
		public bool isBlocking = false;

        public bool IsWalking
        {
            get { return anim.GetBool("IsWalking"); }
            set { anim.SetBool("IsWalking", value); }
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

        void Start()
        {
            GM = GameManager.singleton;
            character = GetComponent<RebirthPlayerController>();
            anim = GetComponent<Animator>();
        }


        public virtual void UpdateClient()
        {
            if (anim == null) return;

            if (Input.GetKeyDown("1"))
            {
                //IsAttacking = true;
                
                ItemBase weapon = character.RightHandItem.GetComponent<ItemBase>();
                IsArmed = !weapon.State();
            }

			if (!isDead)
			{
				if (character.InteractableItem != null && GM.IM.Interact())
				{
					character.InteractableItem.OnInteract();

					if (character.InteractableItem is ItemBase)
					{
						GameManager.singleton.Hud.Bag.AddItem(character.InteractableItem as ItemBase);
						(character.InteractableItem as ItemBase).OnPickup();
					}

					GameManager.singleton.Hud.CloseMessagePanel();

					character.InteractableItem = null;
				}

				if (character.LeftHandItem != null && GM.IM.AttackLeft())
				{
					if (!EventSystem.current.IsPointerOverGameObject())
					{
						character.LeftHandItem.ItemAction();
					}
				}
				if (character.RightHandItem != null && GM.IM.AttackRight())
				{
					if (!EventSystem.current.IsPointerOverGameObject())
					{
						character.RightHandItem.ItemAction();
					}
				}
			}

			Running();
        }

        public virtual void FixedUpdateClient()
        {
			if (!isDead)
			{
				if (character.LeftHandItem != null && Input.GetKeyDown(KeyCode.G))
				{
					//DropCurrentItem();
				}
			}

			Move();
        }

        public virtual void LatedUpdateClient()
        {

        }

		public void Attack(ItemBase weapon)
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
					if(character.LeftHandItem == null && GM.IM.AttackLeft())
					{
						AttackLeftFist();
					}

					if(character.RightHandItem == null && GM.IM.AttackRight())
					{
						AttackRightFist();
					}
				}
			}
		}

        public void AttackLeftWeapon(ItemBase weaponItem)
        {
			if (!IsAttacking)
            {
                if (IsArmed && character.RightHandItem == weaponItem)
                {
                    IsAttacking = true;

                    StartCoroutine(Attacking(anim.GetCurrentAnimatorStateInfo(0).length));
                }
                else if (IsArmed)
                {
                    // In Kampf-Modus aber keine Waffe ausgerüstet = Faustkampf

                }
            }           
        }

		public void AttackRightWeapon(ItemBase weaponItem)
		{
			if (!IsAttacking)
			{
				if (IsArmed && character.RightHandItem == weaponItem)
				{
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

            float x = GM.IM.MoveHorizontal(); // Seitwärts
            float z = GM.IM.MoveVertical(); // Vorwärts

            //float speed = new Vector2(z, x).sqrMagnitude;

            IsWalking = (z != 0 || x != 0);

            if (IsWalking && IsRunning)
            {
                MoveZ = Mathf.Clamp(z, -RUN_SPEED, RUN_SPEED);
                MoveX = Mathf.Clamp(x, -RUN_SPEED, RUN_SPEED);
            }
            else if (IsWalking && !IsRunning)
            {
                MoveZ = Mathf.Clamp(z, -WALK_SPEED, WALK_SPEED);
                MoveX = Mathf.Clamp(x, -WALK_SPEED, WALK_SPEED);
            }
            else
            {
                MoveZ = z;
                MoveX = x;
            }

            Vector3 movement = (MoveX * transform.right * MovementSpeed * Time.deltaTime) + (MoveZ * transform.forward * MovementSpeed * Time.deltaTime); 
            character.rb.MovePosition(character.rb.position + movement);
        }

        public void RotatePlayerToCameraDir(Quaternion dir)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, dir, Time.deltaTime * rotationSpeed);
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

        public void EventActivateWeaponDamage(int i)
        {
            if (!IsArmed) return;

            ItemBase item = character.RightHandItem.GetComponent<ItemBase>();
            if (item != null)
            {
                item.col.enabled = true;
            }
        }

        public void EventDeactivateWeaponDamage(int i)
        {
            if (!IsArmed) return;

            ItemBase item = character.RightHandItem.GetComponent<ItemBase>();
            if (item != null)
            {
                item.col.enabled = true;
            }
        }

        public void EventShowWeapon(int i)
        {
            ItemBase item = character.RightHandItem.GetComponent<ItemBase>();
            if (item != null)
            {
                item.Toggle(IsArmed);
            }
        }

        public void EventHideWeapon(int i)
        {
            ItemBase item = character.RightHandItem.GetComponent<ItemBase>();
            if (item != null)
            {
                item.Toggle(IsArmed);
            }
        }
        #endregion
    }
}