using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rebirth.Prototype
{
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


        void Update()
        {
            if (anim == null) return;

            if (Input.GetKeyDown("1"))
            {
                //IsAttacking = true;
                WeaponBase weapon = character.RightHandItem.GetComponent<WeaponBase>();
                IsArmed = !weapon.State();
            }

            Running();
            Attack();
        }

        public void FixedUpdate()
        {
            //Rotate();
            Move();
        }

        public void LateUpdate()
        {

        }

        private void Attack()
        {
            if (Input.GetMouseButtonDown(0) && !IsAttacking)
            {
                if (IsArmed && character.LeftHandItem != null)
                {
                    ItemBase item = character.LeftHandItem.GetComponent<ItemBase>();
                    item.ItemAction();
                    IsAttacking = true;

                    StartCoroutine(Attacking(anim.GetCurrentAnimatorStateInfo(1).length));
                }
                else if (IsArmed)
                {
                    // In Kampf-Modus aber keine Waffe ausgerüstet = Faustkampf

                }
            }

            if (Input.GetMouseButtonDown(1) && !IsAttacking)
            {
                if (IsArmed && character.RightHandItem != null)
                {
                    ItemBase item = character.RightHandItem.GetComponent<ItemBase>();
                    item.ItemAction();
                    IsAttacking = true;

                    StartCoroutine(Attacking(anim.GetCurrentAnimatorStateInfo(1).length));
                }
            }
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

            if (GM.IM.Forward())
            {

            }

            float x = Input.GetAxis("Horizontal"); // Seitwärts
            float z = Input.GetAxis("Vertical"); // Vorwärts

            float speed = new Vector2(z, x).sqrMagnitude;

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

            Vector3 movement = new Vector3(x, 0, z) * MovementSpeed * Time.deltaTime;
            character.rb.MovePosition(transform.position + movement);
            //rb.MovePosition(transform.position + transform.forward * speed * MovementSpeed * Time.deltaTime);
            //transform.position += Vector3.forward * z * MovementSpeed * Time.deltaTime;
            //transform.position += Vector3.right * x * * MovementSpeed * Time.deltaTime;
        }

        public void RotatePlayerToCameraDir(Quaternion dir)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, dir, Time.deltaTime * rotationSpeed);
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