//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;

//namespace Rebirth.Prototype
//{
//    public enum WeaponStances_e
//    {
//        UNARMED = 0,
//        TWOHANDSWORD = 1,
//        TWOHANDSPEAR = 2,
//        TWOHANDAXE = 3,
//        TWOHANDBOW = 4,
//        TWOHANDCROSSBOW = 5,
//        STAFF = 6,
//        ARMED = 7,
//        RELAX = 8
//    }

//    //public enum Weapons_e
//    //{
//    //    HAND = 0,
//    //    TWOHANDSWORD = 1,
//    //    TWOHANDSPEAR = 2,
//    //    TWOHANDAXE = 3,
//    //    TWOHANDBOW = 4,
//    //    TWOHANDCROSSBOW = 5,
//    //    STAFF = 6,
//    //    SHIELD = 7,
//    //    LEFT_SWORD = 8,
//    //    RIGHT_SWORD = 9,
//    //    LEFT_MACE = 10,
//    //    RIGHT_MACE = 11,
//    //    LEFT_DAGGER = 12,
//    //    RIGHT_DAGGER = 13,
//    //    LEFT_ITEM = 14,
//    //    RIGHT_ITEM = 16,
//    //    LEFT_PISTOL = 16,
//    //    RIGHT_PISTOL = 17
//    //}

//    public class CharacterController : MonoBehaviour
//    {
//        #region "Control Settings"
//        [Header("Jumping Settings")]
//        public float gravity = -9.8f;
//        bool canJump;
//        public bool isJumping = false;
//        public bool isGrounded;
//        public float jumpSpeed = 12;
//        public float doublejumpSpeed = 12;
//        bool doJump = false;
//        bool doublejumping = true;
//        bool canDoubleJump = false;
//        public bool isDoubleJumping = false;
//        bool doublejumped = false;
//        public bool isFalling;
//        bool startFall;
//        float fallingVelocity = -1f;

//        [Header("InAir Settings")]
//        public float inAirSpeed = 8f;
//        float maxVelocity = 2f;
//        float minVelocity = -2f;

//        [Header("Rolling Settings")]
//        public float rollSpeed = 8;
//        public bool isRolling = false;
//        public float rollduration;

//        [Header("Movement Settings")]
//        public bool canMove = true;
//        public float walkSpeed = 1.35f;
//        float moveSpeed;
//        public float runSpeed = 6f;
//        float rotationSpeed = 40f;

//        float x;
//        float z;
//        float dv;
//        float dh;
//        Vector3 inputVec;
//        Vector3 newVelocity;

//        [Header("Weapon and Shield")]
//        //private Weapons_e weapon;
//        private WeaponStances_e weaponStance;
//        Weapons_e rightWeapon = 0;
//        Weapons_e leftWeapon = 0;
//        public bool isRelax = false;

//        [Header("Action Settings")]
//        public bool canAction = true;
//        public bool isStrafing = false;
//        public bool isSprinting = false;
//        public bool isDead = false;
//        public bool isBlocking = false;
//        public bool blockGui;
//        public float knockbackMultiplier = 1f;
//        public bool isKnockback;

//        [Header("Swimming Settings")]
//        public bool isSwimming = false;
//        public float inWaterSpeed = 8f;

//        [Header("Weapon Models")]
//        public GameObject twohandaxe;
//        public GameObject twohandsword;
//        public GameObject twohandspear;
//        public GameObject twohandbow;
//        public GameObject twohandcrossbow;
//        public GameObject staff;
//        public GameObject swordL;
//        public GameObject swordR;
//        public GameObject maceL;
//        public GameObject maceR;
//        public GameObject daggerL;
//        public GameObject daggerR;
//        public GameObject itemL;
//        public GameObject itemR;
//        public GameObject shield;
//        public GameObject pistolL;
//        public GameObject pistolR;
//        #endregion

//        float inputHorizontal;
//        float inputVertical;
//        bool inputDash;

//        private RebirthPlayerController character;

//        [HideInInspector]
//        public Animator animator;
//        Rigidbody rb;
//        Vector3 targetDashDirection;
//        CapsuleCollider capCollider;
//        ParticleSystem FXSplash;

//        void Awake()
//        {
//            //hide all weapons
//            if (twohandaxe != null)
//            {
//                twohandaxe.SetActive(false);
//            }
//            if (twohandbow != null)
//            {
//                twohandbow.SetActive(false);
//            }
//            if (twohandcrossbow != null)
//            {
//                twohandcrossbow.SetActive(false);
//            }
//            if (twohandspear != null)
//            {
//                twohandspear.SetActive(false);
//            }
//            if (twohandsword != null)
//            {
//                twohandsword.SetActive(false);
//            }
//            if (staff != null)
//            {
//                staff.SetActive(false);
//            }
//            if (swordL != null)
//            {
//                swordL.SetActive(false);
//            }
//            if (swordR != null)
//            {
//                swordR.SetActive(false);
//            }
//            if (maceL != null)
//            {
//                maceL.SetActive(false);
//            }
//            if (maceR != null)
//            {
//                maceR.SetActive(false);
//            }
//            if (daggerL != null)
//            {
//                daggerL.SetActive(false);
//            }
//            if (daggerR != null)
//            {
//                daggerR.SetActive(false);
//            }
//            if (itemL != null)
//            {
//                itemL.SetActive(false);
//            }
//            if (itemR != null)
//            {
//                itemR.SetActive(false);
//            }
//            if (shield != null)
//            {
//                shield.SetActive(false);
//            }
//            if (pistolL != null)
//            {
//                pistolL.SetActive(false);
//            }
//            if (pistolR != null)
//            {
//                pistolR.SetActive(false);
//            }

//            character = GetComponent<RebirthPlayerController>();
//            animator = GetComponent<Animator>();
//            rb = GetComponent<Rigidbody>();
//            capCollider = GetComponent<CapsuleCollider>();
//            FXSplash = transform.GetChild(2).GetComponent<ParticleSystem>();
//        }

//        public void Start()
//        {
//            if (animator.layerCount >= 2)
//            {
//                animator.SetLayerWeight(1, 1);
//            }

//            //hide all weapons
//            if (twohandaxe != null)
//            {
//                twohandaxe.SetActive(false);
//            }
//            if (twohandbow != null)
//            {
//                twohandbow.SetActive(false);
//            }
//            if (twohandcrossbow != null)
//            {
//                twohandcrossbow.SetActive(false);
//            }
//            if (twohandspear != null)
//            {
//                twohandspear.SetActive(false);
//            }
//            if (twohandsword != null)
//            {
//                twohandsword.SetActive(false);
//            }
//            if (staff != null)
//            {
//                staff.SetActive(false);
//            }
//            if (swordL != null)
//            {
//                swordL.SetActive(false);
//            }
//            if (swordR != null)
//            {
//                swordR.SetActive(false);
//            }
//            if (maceL != null)
//            {
//                maceL.SetActive(false);
//            }
//            if (maceR != null)
//            {
//                maceR.SetActive(false);
//            }
//            if (daggerL != null)
//            {
//                daggerL.SetActive(false);
//            }
//            if (daggerR != null)
//            {
//                daggerR.SetActive(false);
//            }
//            if (itemL != null)
//            {
//                itemL.SetActive(false);
//            }
//            if (itemR != null)
//            {
//                itemR.SetActive(false);
//            }
//            if (shield != null)
//            {
//                shield.SetActive(false);
//            }
//            if (pistolL != null)
//            {
//                pistolL.SetActive(false);
//            }
//            if (pistolR != null)
//            {
//                pistolR.SetActive(false);
//            }
//        }

//        public void OnAttackLeft()
//        {
//            if (animator)
//            {
//                if (PlayerInput.GetAttackLeft() && canAction && isGrounded && !isBlocking)
//                {
//                    Attack(1);
//                }
//                if (PlayerInput.GetAttackLeft() && canAction && isGrounded && isBlocking)
//                {
//                    StartCoroutine(_BlockHitReact());
//                }
//            }
//        }

//        public void OnAttackRight()
//        {
//            if(animator)
//            {
//                if (PlayerInput.GetAttackRight() && canAction && isGrounded && !isBlocking)
//                {
//                    Attack(2);
//                }
//                if (PlayerInput.GetAttackRight() && canAction && isGrounded && isBlocking)
//                {
//                    StartCoroutine(_BlockHitReact());
//                }
//            }
//        }

//        public void UpdateClient()
//        {
//            //if (GameManager.singleton.WorldCamera.GetComponent<CameraController>().m_cursorIsLocked)
//            //{
//                //Cmd_Movement();

//                //input abstraction for easier asset updates using outside control schemes
//            //bool inputJump = Input.GetButtonDown("Jump");
//            bool inputLightHit = false; // Input.GetButtonDown("LightHit");
//            bool inputDeath = false; // Input.GetButtonDown("Death");
//            bool inputUnarmed = false; // Input.GetButtonDown("Unarmed");
//            bool inputShield = false; // Input.GetButtonDown("Shield");
//            //bool inputAttackL = Input.GetButtonDown("AttackL");
//            //bool inputAttackR = Input.GetButtonDown("AttackR");
//            //bool inputCastL = Input.GetButtonDown("CastL");
//            //bool inputCastR = Input.GetButtonDown("CastR");
//            float inputSwitchUpDown = 0; //Input.GetAxisRaw("SwitchUpDown");
//            float inputSwitchLeftRight = 0; // Input.GetAxisRaw("SwitchLeftRight");
//            //bool inputStrafe = Input.GetKey(KeyCode.LeftShift);
//           // bool inputSprint = Input.GetKey(KeyCode.LeftShift);
//            float inputTargetBlock = Input.GetAxisRaw("TargetBlock");
//            inputDash = Input.GetButtonDown("Dash");

//            inputHorizontal = Input.GetAxisRaw("Horizontal");
//            inputVertical = Input.GetAxisRaw("Vertical");

//            //converts control input vectors into camera facing vectors
//            Transform cameraTransform = GameManager.singleton.WorldCamera.transform; // other camera
//                                                                                        //Forward vector relative to the camera along the x-z plane   
//            Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
//            forward.y = 0;
//            forward = forward.normalized;
//            //Right vector relative to the camera always orthogonal to the forward vector
//            Vector3 right = new Vector3(forward.z, 0, -forward.x);
//            //directional inputs
//            dv = inputVertical; // inputDashVertical;
//            dh = inputHorizontal; // inputDashHorizontal;
//            if (!isRolling)
//            {
//                targetDashDirection = dh * right + -dv * -forward;
//            }

//            x = inputHorizontal;
//            z = inputVertical;
                
//            inputVec = x * right + z * forward;
//            //make sure there is animator on character
//            if (animator)
//            {
//                if (canMove && !isBlocking && !isDead)
//                {
//                    if (x != 0 || z < 0)
//                    {
//                        isStrafing = true;
//                        animator.SetBool("Strafing", true);
//                    }
//                    else
//                    {
//                        isStrafing = false;
//                        animator.SetBool("Strafing", false);
//                    }
//                }
//                else
//                {
//                    inputVec = new Vector3(0, 0, 0);
//                }

//                if (PlayerInput.GetJump())
//                {
//                    doJump = true;
//                }
//                else
//                {
//                    doJump = false;
//                }

//                if (!isSwimming)
//                {
//                    Rolling();
//                    Jumping();
//                    Blocking();
//                }

//                if (inputLightHit && canAction && isGrounded && !isBlocking)
//                {
//                    GetHit();
//                }

//                if (inputDeath && canAction && isGrounded && !isBlocking)
//                {
//                    if (!isDead)
//                    {
//                        StartCoroutine(_Death());
//                    }
//                    else
//                    {
//                        StartCoroutine(_Revive());
//                    }
//                }

//                if (PlayerInput.GetInteract() && canAction && !isBlocking && weaponStance == WeaponStances_e.UNARMED)
//                {
//                    Collect();
//                }

//                if (inputUnarmed && canAction && isGrounded && !isBlocking && weaponStance != WeaponStances_e.UNARMED)
//                {
//                    StartCoroutine(_SwitchWeapon(0));
//                }
//                if (inputShield && canAction && isGrounded && !isBlocking && leftWeapon != Weapons_e.SHIELD)
//                {
//                    StartCoroutine(_SwitchWeapon(Weapons_e.SHIELD));
//                }

                
               
//                if (PlayerInput.GetCastLeft() && canAction && isGrounded && !isBlocking && !isStrafing)
//                {
//                    AttackKick(1);
//                }
//                if (PlayerInput.GetCastLeft() && canAction && isGrounded && isBlocking)
//                {
//                    StartCoroutine(_BlockBreak());
//                }
//                if (PlayerInput.GetCastRight() && canAction && isGrounded && !isBlocking && !isStrafing)
//                {
//                    AttackKick(2);
//                }
//                if (PlayerInput.GetCastRight() && canAction && isGrounded && isBlocking)
//                {
//                    StartCoroutine(_BlockBreak());
//                }
//                if (inputSwitchUpDown < -.1 && canAction && !isBlocking && isGrounded)
//                {
//                    SwitchWeaponTwoHand(0);
//                }
//                else if (inputSwitchUpDown > .1 && canAction && !isBlocking && isGrounded)
//                {
//                    SwitchWeaponTwoHand(1);
//                }
//                if (inputSwitchLeftRight < -.1 && canAction && !isBlocking && isGrounded)
//                {
//                    SwitchWeaponLeftRight(0);
//                }
//                else if (inputSwitchLeftRight > .1 && canAction && !isBlocking && isGrounded)
//                {
//                    SwitchWeaponLeftRight(1);
//                }

//                if (PlayerInput.GetSprint())
//                {
//                    isSprinting = true;
//                }
//                else
//                {
//                    isSprinting = false;
//                }

//                //if strafing
//                //if (inputStrafe || inputTargetBlock > .1 && canAction)
//                //{
//                //    isStrafing = true;
//                //    animator.SetBool("Strafing", true);
//                //    if (inputCastL && canAction && isGrounded && !isBlocking)
//                //    {
//                //        CastAttack(1);
//                //    }
//                //    if (inputCastR && canAction && isGrounded && !isBlocking)
//                //    {
//                //        CastAttack(2);
//                //    }
//                //}
//                //else
//                //{
//                //    isStrafing = false;
//                //    animator.SetBool("Strafing", false);
//                //}
//            }
//            else
//            {
//                Debug.Log("ERROR: There is no animator for character.");
//            }
//            //}
//        }

//        public void FixedUpdateClient()
//        {
//            if (!isSwimming)
//            {
//                CheckForGrounded();
//                //apply gravity force
//                rb.AddForce(0, gravity, 0, ForceMode.Acceleration);
//                AirControl();
//                //check if character can move
//                if (canMove && !isBlocking)
//                {
//                    moveSpeed = UpdateMovement();
//                }
//                //check if falling
//                if (rb.velocity.y < fallingVelocity)
//                {
//                    isFalling = true;
//                    animator.SetInteger("Jumping", 2);
//                    canJump = false;
//                }
//                else
//                {
//                    isFalling = false;
//                }
//            }
//            else
//            {
//                WaterControl();
//                moveSpeed = UpdateMovement();
//            }            
//        }

//        public void LatedUpdateClient()
//        {
//            //Get local velocity of charcter
//            float velocityXel = transform.InverseTransformDirection(rb.velocity).x;
//            float velocityZel = transform.InverseTransformDirection(rb.velocity).z;
//            //Update animator with movement values
//            animator.SetFloat("Velocity X", velocityXel / runSpeed);
//            animator.SetFloat("Velocity Z", velocityZel / runSpeed);
//            //if character is alive and can move, set our animator
//            if (!isDead && canMove)
//            {
//                if (moveSpeed > 0)
//                {
//                    animator.SetBool("Moving", true);
//                }
//                else
//                {
//                    animator.SetBool("Moving", false);
//                }
//            }
//        }

//        public void Cmd_Movement()
//        {
//            RpcMovement();
//        }

//        public void RpcMovement()
//        {
            
//        }

//        void Collect()
//        {
//            RaycastHit hit;
//            int rayLength = 10;

//            //Debug.DrawLine(, , Color.red);

//            //Debug.DrawLine(Camera.transform.position, Camera.transform.forward * rayLength, Color.red);

//            //Debug.DrawRay(character.PlayerHead.transform.position, GameManager.singleton.WorldCamera.transform.GetComponent<CameraController>().AimVector, Color.blue);

//            //if (Physics.Raycast(character.PlayerHead.transform.position, GameManager.singleton.WorldCamera.transform.GetComponent<CameraController>().AimVector * rayLength, out hit, rayLength) && (hit.collider.gameObject.tag == "HarvestableResource"))
//            //{

//            //    // Harvestable harvestable = hit.collider.gameObject.GetComponent<Harvestable>();
//            //    //if (harvestable == null) return;

//            //    //if (Input.GetKeyUp(KeyCode.E) && !harvestable.bIsHarvesting)
//            //    //{
//            //    //    HarvestResources(harvestable);
//            //    //}
//            //}
//        }

//        #region UpdateMovement

//        //rotate character towards direction moved
//        void RotateTowardsMovementDir()
//        {
//            if (inputVec != Vector3.zero && !isStrafing && !isRolling && !isBlocking)
//            {
//                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(inputVec), Time.deltaTime * rotationSpeed);
//            }
//        }

//        public void RotatePlayerToCameraDir(Quaternion dir)
//        {
//            //if(inputVec != Vector3.zero && !isRolling)
//            //{
//            transform.rotation = Quaternion.Slerp(transform.rotation, dir, Time.deltaTime * rotationSpeed);
//            //}
//        }

//        float UpdateMovement()
//        {
//            Vector3 motion = inputVec;
//            if (isGrounded)
//            {
//                //reduce input for diagonal movement
//                if (motion.magnitude > 1)
//                {
//                    motion.Normalize();
//                }
//                if (canMove && !isBlocking)
//                {
//                    //set speed by walking / running
//                    if (isStrafing)
//                    {
//                        newVelocity = motion * walkSpeed;
//                    }
//                    else if (isSprinting)
//                    {
//                        newVelocity = motion * runSpeed;
//                    }
//                    else
//                    {
//                        newVelocity = motion * walkSpeed;
//                    }

//                    //if rolling use rolling speed and direction
//                    if (isRolling)
//                    {
//                        //force the dash movement to 1
//                        targetDashDirection.Normalize();
//                        newVelocity = rollSpeed * targetDashDirection;
//                    }
//                }
//            }
//            else
//            {
//                if (!isSwimming)
//                {
//                    //if we are falling use momentum
//                    newVelocity = rb.velocity;
//                }
//                else
//                {
//                    newVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
//                }
//            }
//            if (!isStrafing || !canMove)
//            {
//                //RotateTowardsMovementDir();
//                //Debug.Log("Rotate");
//            }
//            if (isStrafing && !isRelax)
//            {
//                ////make character point at target
//                //if (target != null)
//                //{
//                //    Quaternion targetRotation;
//                //    Vector3 targetPos = target.transform.position;
//                //    targetRotation = Quaternion.LookRotation(targetPos - new Vector3(transform.position.x, 0, transform.position.z));
//                //    transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y, (rotationSpeed * Time.deltaTime) * rotationSpeed);
//                //}
//            }

//            //Debug.DrawRay(character.PlayerHead.transform.position, GameManager.singleton.WorldCamera.transform.GetComponent<CameraController>().AimVector, Color.white);

//            //if we are falling use momentum
//            newVelocity.y = rb.velocity.y;
//            rb.velocity = newVelocity;
//            //return a movement value for the animator
//            return inputVec.magnitude;
//        }

//        #endregion

//        #region Swimming

//        void OnTriggerEnter(Collider collide)
//        {
//            //If entering a water volume
//            if (collide.gameObject.layer == 4)
//            {
//                isSwimming = true;
//                canAction = false;
//                rb.useGravity = false;
//                animator.SetTrigger("SwimTrigger");
//                animator.SetBool("Swimming", true);
//                animator.SetInteger("Weapon", 0);
//                StartCoroutine(_WeaponVisibility(leftWeapon, 0, false));
//                StartCoroutine(_WeaponVisibility(rightWeapon, 0, false));
//                animator.SetInteger("RightWeapon", 0);
//                animator.SetInteger("LeftWeapon", 0);
//                animator.SetInteger("LeftRight", 0);
//                FXSplash.Emit(30);
//            }
//        }

//        void OnTriggerExit(Collider collide)
//        {
//            //If leaving a water volume
//            if (collide.gameObject.layer == 4)
//            {
//                isSwimming = false;
//                canAction = true;
//                rb.useGravity = true;
//                animator.SetInteger("Jumping", 2);
//                animator.SetBool("Swimming", false);
//                capCollider.radius = .5f;
//            }
//        }

//        void WaterControl()
//        {
//            AscendDescend();
//            Vector3 motion = inputVec;
//            //dampen vertical water movement
//            Vector3 dampenVertical = new Vector3(rb.velocity.x, (rb.velocity.y * .985f), rb.velocity.z);
//            rb.velocity = dampenVertical;
//            Vector3 waterDampen = new Vector3((rb.velocity.x * .98f), rb.velocity.y, (rb.velocity.z * .98f));
//            //If swimming, don't dampen movement, and scale capsule collider
//            if (moveSpeed < .1f)
//            {
//                rb.velocity = waterDampen;
//                capCollider.radius = .5f;
//            }
//            else
//            {
//                capCollider.radius = 1.5f;
//            }
//            rb.velocity = waterDampen;
//            //clamp diagonal movement so its not faster
//            motion *= (Mathf.Abs(inputVec.x) == 1 && Mathf.Abs(inputVec.z) == 1) ? 0.7f : 1;
//            rb.AddForce(motion * inWaterSpeed, ForceMode.Acceleration);
//            //limit the amount of velocity we can achieve to water speed
//            float velocityX = 0;
//            float velocityZ = 0;
//            if (rb.velocity.x > inWaterSpeed)
//            {
//                velocityX = GetComponent<Rigidbody>().velocity.x - inWaterSpeed;
//                if (velocityX < 0)
//                {
//                    velocityX = 0;
//                }
//                rb.AddForce(new Vector3(-velocityX, 0, 0), ForceMode.Acceleration);
//            }
//            if (rb.velocity.x < minVelocity)
//            {
//                velocityX = rb.velocity.x - minVelocity;
//                if (velocityX > 0)
//                {
//                    velocityX = 0;
//                }
//                rb.AddForce(new Vector3(-velocityX, 0, 0), ForceMode.Acceleration);
//            }
//            if (rb.velocity.z > inWaterSpeed)
//            {
//                velocityZ = rb.velocity.z - maxVelocity;
//                if (velocityZ < 0)
//                {
//                    velocityZ = 0;
//                }
//                rb.AddForce(new Vector3(0, 0, -velocityZ), ForceMode.Acceleration);
//            }
//            if (rb.velocity.z < minVelocity)
//            {
//                velocityZ = rb.velocity.z - minVelocity;
//                if (velocityZ > 0)
//                {
//                    velocityZ = 0;
//                }
//                rb.AddForce(new Vector3(0, 0, -velocityZ), ForceMode.Acceleration);
//            }
//        }

//        void AscendDescend()
//        {
//            if (doJump)
//            {
//                //swim down with left control
//                if (isStrafing)
//                {
//                    animator.SetBool("Strafing", true);
//                    animator.SetTrigger("JumpTrigger");
//                    rb.velocity -= inWaterSpeed * Vector3.up;
//                }
//                else
//                {
//                    animator.SetTrigger("JumpTrigger");
//                    rb.velocity += inWaterSpeed * Vector3.up;
//                }
//            }
//        }

//        #endregion

//        #region Jumping

//        //checks if character is within a certain distance from the ground, and markes it IsGrounded
//        void CheckForGrounded()
//        {
//            float distanceToGround;
//            float threshold = .45f;
//            RaycastHit hit;
//            Vector3 offset = new Vector3(0, .4f, 0);
//            if (Physics.Raycast((transform.position + offset), -Vector3.up, out hit, 100f))
//            {
//                distanceToGround = hit.distance;
//                if (distanceToGround < threshold)
//                {
//                    isGrounded = true;
//                    canJump = true;
//                    startFall = false;
//                    doublejumped = false;
//                    canDoubleJump = false;
//                    isFalling = false;
//                    if (!isJumping)
//                    {
//                        animator.SetInteger("Jumping", 0);
//                    }
//                }
//                else
//                {
//                    isGrounded = false;
//                }
//            }
//        }

//        void Jumping()
//        {
//            if (isGrounded)
//            {
//                if (canJump && doJump)
//                {
//                    StartCoroutine(_Jump());
//                }
//            }
//            else
//            {
//                canDoubleJump = true;
//                canJump = false;
//                if (isFalling)
//                {
//                    //set the animation back to falling
//                    animator.SetInteger("Jumping", 2);
//                    //prevent from going into land animation while in air
//                    if (!startFall)
//                    {
//                        animator.SetTrigger("JumpTrigger");
//                        startFall = true;
//                    }
//                }
//                if (canDoubleJump && doublejumping && Input.GetButtonDown("Jump") && !doublejumped && isFalling)
//                {
//                    // Apply the current movement to launch velocity
//                    rb.velocity += doublejumpSpeed * Vector3.up;
//                    animator.SetInteger("Jumping", 3);
//                    doublejumped = true;
//                }
//            }
//        }

//        IEnumerator _Jump()
//        {
//            isJumping = true;
//            animator.SetInteger("Jumping", 1);
//            animator.SetTrigger("JumpTrigger");
//            // Apply the current movement to launch velocity
//            rb.velocity += jumpSpeed * Vector3.up;
//            canJump = false;
//            yield return new WaitForSeconds(.5f);
//            isJumping = false;
//        }

//        void AirControl()
//        {
//            if (!isGrounded)
//            {
//                Vector3 motion = inputVec;
//                motion *= (Mathf.Abs(inputVec.x) == 1 && Mathf.Abs(inputVec.z) == 1) ? 0.7f : 1;
//                rb.AddForce(motion * inAirSpeed, ForceMode.Acceleration);
//                //limit the amount of velocity we can achieve
//                float velocityX = 0;
//                float velocityZ = 0;
//                if (rb.velocity.x > maxVelocity)
//                {
//                    velocityX = GetComponent<Rigidbody>().velocity.x - maxVelocity;
//                    if (velocityX < 0)
//                    {
//                        velocityX = 0;
//                    }
//                    rb.AddForce(new Vector3(-velocityX, 0, 0), ForceMode.Acceleration);
//                }
//                if (rb.velocity.x < minVelocity)
//                {
//                    velocityX = rb.velocity.x - minVelocity;
//                    if (velocityX > 0)
//                    {
//                        velocityX = 0;
//                    }
//                    rb.AddForce(new Vector3(-velocityX, 0, 0), ForceMode.Acceleration);
//                }
//                if (rb.velocity.z > maxVelocity)
//                {
//                    velocityZ = rb.velocity.z - maxVelocity;
//                    if (velocityZ < 0)
//                    {
//                        velocityZ = 0;
//                    }
//                    rb.AddForce(new Vector3(0, 0, -velocityZ), ForceMode.Acceleration);
//                }
//                if (rb.velocity.z < minVelocity)
//                {
//                    velocityZ = rb.velocity.z - minVelocity;
//                    if (velocityZ > 0)
//                    {
//                        velocityZ = 0;
//                    }
//                    rb.AddForce(new Vector3(0, 0, -velocityZ), ForceMode.Acceleration);
//                }
//            }
//        }

//        #endregion

//        #region MiscMethods

//        //0 = No side
//        //1 = Left
//        //2 = Right
//        //3 = Dual
//        public void Attack(int attackSide)
//        {
//            if (canAction)
//            {
//                if (weaponStance == WeaponStances_e.UNARMED || weaponStance == WeaponStances_e.ARMED)
//                {
//                    int maxAttacks = 3;
//                    int attackNumber = 0;
//                    if (attackSide == 1 || attackSide == 3)
//                    {
//                        attackNumber = Random.Range(0, maxAttacks);
//                    }
//                    else if (attackSide == 2)
//                    {
//                        attackNumber = Random.Range(3, maxAttacks + 3);
//                    }
//                    if (isGrounded)
//                    {
//                        if (attackSide != 3)
//                        {
//                            animator.SetTrigger("Attack" + (attackNumber + 1).ToString() + "Trigger");
//                            if (leftWeapon == Weapons_e.LEFT_DAGGER || leftWeapon == Weapons_e.LEFT_ITEM || rightWeapon == Weapons_e.RIGHT_DAGGER || rightWeapon == Weapons_e.RIGHT_ITEM)
//                            {
//                                StartCoroutine(_LockMovementAndAttack(0, .75f));
//                            }
//                            else
//                            {
//                                StartCoroutine(_LockMovementAndAttack(0, .6f));
//                            }
//                        }
//                        else
//                        {
//                            animator.SetTrigger("AttackDual" + (attackNumber + 1).ToString() + "Trigger");
//                            StartCoroutine(_LockMovementAndAttack(0, .75f));
//                        }
//                    }
//                }
//                else
//                {
//                    int maxAttacks = 6;
//                    {
//                        int attackNumber = Random.Range(0, maxAttacks);
//                        if (isGrounded)
//                        {
//                            animator.SetTrigger("Attack" + (attackNumber + 1).ToString() + "Trigger");
//                            if (weaponStance == WeaponStances_e.TWOHANDSWORD)
//                            {
//                                StartCoroutine(_LockMovementAndAttack(0, .85f));
//                            }
//                            else if (weaponStance == WeaponStances_e.TWOHANDSPEAR)
//                            {
//                                StartCoroutine(_LockMovementAndAttack(0, 1.1f));
//                            }
//                            else if (weaponStance == WeaponStances_e.TWOHANDAXE)
//                            {
//                                StartCoroutine(_LockMovementAndAttack(0, 1.5f));
//                            }
//                            else
//                            {
//                                StartCoroutine(_LockMovementAndAttack(0, .75f));
//                            }
//                        }
//                    }
//                }
//            }
//        }

//        void AttackKick(int kickSide)
//        {
//            if (isGrounded)
//            {
//                if (kickSide == 1)
//                {
//                    animator.SetTrigger("AttackKick1Trigger");
//                }
//                else
//                {
//                    animator.SetTrigger("AttackKick2Trigger");
//                }
//                StartCoroutine(_LockMovementAndAttack(0, .8f));
//            }
//        }

//        //0 = No side
//        //1 = Left
//        //2 = Right
//        //3 = Dual
//        void CastAttack(int attackSide)
//        {
//            if (weaponStance == WeaponStances_e.UNARMED || weaponStance == WeaponStances_e.STAFF)
//            {
//                int maxAttacks = 3;
//                if (attackSide == 1)
//                {
//                    int attackNumber = Random.Range(0, maxAttacks);
//                    if (isGrounded)
//                    {
//                        animator.SetTrigger("CastAttack" + (attackNumber + 1).ToString() + "Trigger");
//                        StartCoroutine(_LockMovementAndAttack(0, .8f));
//                    }
//                }
//                if (attackSide == 2)
//                {
//                    int attackNumber = Random.Range(3, maxAttacks + 3);
//                    if (isGrounded)
//                    {
//                        animator.SetTrigger("CastAttack" + (attackNumber + 1).ToString() + "Trigger");
//                        StartCoroutine(_LockMovementAndAttack(0, .8f));
//                    }
//                }
//                if (attackSide == 3)
//                {
//                    int attackNumber = Random.Range(0, maxAttacks);
//                    if (isGrounded)
//                    {
//                        animator.SetTrigger("CastDualAttack" + (attackNumber + 1).ToString() + "Trigger");
//                        StartCoroutine(_LockMovementAndAttack(0, 1f));
//                    }
//                }
//            }
//        }

//        void Blocking()
//        {
//            if (blockGui || Input.GetAxisRaw("TargetBlock") < -.1 && canAction && isGrounded)
//            {
//                if (!isBlocking)
//                {
//                    animator.SetTrigger("BlockTrigger");
//                }
//                isBlocking = true;
//                canJump = false;
//                animator.SetBool("Blocking", true);
//                rb.velocity = Vector3.zero;
//                rb.angularVelocity = Vector3.zero;
//                inputVec = Vector3.zero;
//            }
//            else
//            {
//                isBlocking = false;
//                canJump = true;
//                animator.SetBool("Blocking", false);
//            }
//        }

//        public void GetHit()
//        {
//            int hits = 5;
//            int hitNumber = Random.Range(0, hits);
//            animator.SetTrigger("GetHit" + (hitNumber + 1).ToString() + "Trigger");
//            StartCoroutine(_LockMovementAndAttack(.1f, .4f));
//            //apply directional knockback force
//            if (hitNumber <= 1)
//            {
//                StartCoroutine(_Knockback(-transform.forward, 8, 4));
//            }
//            else if (hitNumber == 2)
//            {
//                StartCoroutine(_Knockback(transform.forward, 8, 4));
//            }
//            else if (hitNumber == 3)
//            {
//                StartCoroutine(_Knockback(transform.right, 8, 4));
//            }
//            else if (hitNumber == 4)
//            {
//                StartCoroutine(_Knockback(-transform.right, 8, 4));
//            }
//        }

//        IEnumerator _Knockback(Vector3 knockDirection, int knockBackAmount, int variableAmount)
//        {
//            isKnockback = true;
//            StartCoroutine(_KnockbackForce(knockDirection, knockBackAmount, variableAmount));
//            yield return new WaitForSeconds(.1f);
//            isKnockback = false;
//        }

//        IEnumerator _KnockbackForce(Vector3 knockDirection, int knockBackAmount, int variableAmount)
//        {
//            while (isKnockback)
//            {
//                rb.AddForce(knockDirection * ((knockBackAmount + Random.Range(-variableAmount, variableAmount)) * (knockbackMultiplier * 10)), ForceMode.Impulse);
//                yield return null;
//            }
//        }

//        public IEnumerator _Death()
//        {
//            animator.SetTrigger("Death1Trigger");
//            StartCoroutine(_LockMovementAndAttack(.1f, 1.5f));
//            isDead = true;
//            animator.SetBool("Moving", false);
//            inputVec = new Vector3(0, 0, 0);
//            yield return null;
//        }

//        public IEnumerator _Revive()
//        {
//            animator.SetTrigger("Revive1Trigger");
//            isDead = false;
//            yield return null;
//        }

//        #endregion

//        #region Rolling

//        void Rolling()
//        {
//            if (!isRolling && isGrounded)
//            {
//                if(inputHorizontal != 0 && inputDash)
//                {
//                    StartCoroutine(_DirectionalRoll(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal")));
//                }

//                if(inputVertical != 0 && inputDash)
//                {
//                    StartCoroutine(_DirectionalRoll(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal")));
//                }

//                //if (Input.GetAxis("DashVertical") > .5 || Input.GetAxis("DashVertical") < -.5 || Input.GetAxis("DashHorizontal") > .5 || Input.GetAxis("DashHorizontal") < -.5)
//                //{
//                //    StartCoroutine(_DirectionalRoll(Input.GetAxis("DashVertical"), Input.GetAxis("DashHorizontal")));
//                //}
//            }
//        }

//        public IEnumerator _DirectionalRoll(float x, float v)
//        {
//            //check which way the dash is pressed relative to the character facing
//            float angle = Vector3.Angle(targetDashDirection, -transform.forward);
//            float sign = Mathf.Sign(Vector3.Dot(transform.up, Vector3.Cross(targetDashDirection, transform.forward)));
//            // angle in [-179,180]
//            float signed_angle = angle * sign;
//            //angle in 0-360
//            float angle360 = (signed_angle + 180) % 360;
//            //deternime the animation to play based on the angle
//            if (angle360 > 315 || angle360 < 45)
//            {
//                StartCoroutine(_Roll(1));
//            }
//            if (angle360 > 45 && angle360 < 135)
//            {
//                StartCoroutine(_Roll(2));
//            }
//            if (angle360 > 135 && angle360 < 225)
//            {
//                StartCoroutine(_Roll(3));
//            }
//            if (angle360 > 225 && angle360 < 315)
//            {
//                StartCoroutine(_Roll(4));
//            }
//            yield return null;
//        }

//        IEnumerator _Roll(int rollNumber)
//        {
//            if (rollNumber == 1)
//            {
//                animator.SetTrigger("RollForwardTrigger");
//            }
//            if (rollNumber == 2)
//            {
//                animator.SetTrigger("RollRightTrigger");
//            }
//            if (rollNumber == 3)
//            {
//                animator.SetTrigger("RollBackwardTrigger");
//            }
//            if (rollNumber == 4)
//            {
//                animator.SetTrigger("RollLeftTrigger");
//            }
//            isRolling = true;
//            yield return new WaitForSeconds(rollduration);
//            isRolling = false;
//        }

//        #endregion

//        #region _Coroutines

//        //method to keep character from moveing while attacking, etc
//        public IEnumerator _LockMovementAndAttack(float delayTime, float lockTime)
//        {
//            yield return new WaitForSeconds(delayTime);
//            canAction = false;
//            canMove = false;
//            animator.SetBool("Moving", false);
//            rb.velocity = Vector3.zero;
//            rb.angularVelocity = Vector3.zero;
//            inputVec = new Vector3(0, 0, 0);
//            animator.applyRootMotion = true;
//            yield return new WaitForSeconds(lockTime);
//            canAction = true;
//            canMove = true;
//            animator.applyRootMotion = false;
//        }

//        //for controller weapon switching
//        void SwitchWeaponTwoHand(int upDown)
//        {
//            canAction = false;
//            int weaponSwitch = (int)weaponStance;
//            if (upDown == 0)
//            {
//                weaponSwitch--;
//                if (weaponSwitch < 1)
//                {
//                    StartCoroutine(_SwitchWeapon(Weapons_e.STAFF));
//                }
//                else
//                {
//                    StartCoroutine(_SwitchWeapon((Weapons_e)weaponSwitch));
//                }
//            }
//            if (upDown == 1)
//            {
//                weaponSwitch++;
//                if (weaponSwitch > 6)
//                {
//                    StartCoroutine(_SwitchWeapon((Weapons_e)1));
//                }
//                else
//                {
//                    StartCoroutine(_SwitchWeapon((Weapons_e)weaponSwitch));
//                }
//            }
//        }

//        //for controller weapon switching
//        void SwitchWeaponLeftRight(int upDown)
//        {
//            int weaponSwitch = 0;
//            canAction = false;
//            if (upDown == 0)
//            {
//                weaponSwitch = (int)leftWeapon;
//                if (weaponSwitch < 16 && weaponSwitch != 0 && (int)leftWeapon != 7)
//                {
//                    weaponSwitch += 2;
//                }
//                else
//                {
//                    weaponSwitch = 8;
//                }
//            }
//            if (upDown == 1)
//            {
//                weaponSwitch = (int)rightWeapon;
//                if (weaponSwitch < 17 && weaponSwitch != 0)
//                {
//                    weaponSwitch += 2;
//                }
//                else
//                {
//                    weaponSwitch = 9;
//                }
//            }
//            StartCoroutine(_SwitchWeapon((Weapons_e)weaponSwitch));
//        }

//        //function to switch weapons
//        public IEnumerator _SwitchWeapon(Weapons_e weaponNumber)
//        {
//            //character is unarmed
//            if (weaponStance == WeaponStances_e.UNARMED)
//            {
//                StartCoroutine(_UnSheathWeapon(weaponNumber));
//            }
//            //character has 2 handed weapon
//            else if (weaponStance == WeaponStances_e.STAFF || weaponStance == WeaponStances_e.TWOHANDAXE || weaponStance == WeaponStances_e.TWOHANDBOW || weaponStance == WeaponStances_e.TWOHANDCROSSBOW || weaponStance == WeaponStances_e.TWOHANDSPEAR || weaponStance == WeaponStances_e.TWOHANDSWORD)
//            {
//                StartCoroutine(_SheathWeapon((int)leftWeapon, (int)weaponNumber));
//                yield return new WaitForSeconds(1.1f);
//                if (weaponNumber > 0)
//                {
//                    StartCoroutine(_UnSheathWeapon(weaponNumber));
//                }
//                //switch to unarmed
//                else
//                {
//                    weaponStance = WeaponStances_e.UNARMED;
//                    animator.SetInteger("Weapon", 0);
//                }
//            }
//            //character has 1 or 2 1hand weapons and/or shield
//            else if (weaponStance == WeaponStances_e.ARMED)
//            {
//                //character is switching to 2 hand weapon or unarmed, put put away all weapons
//                if ((int)weaponNumber < 7)
//                {
//                    //check left hand for weapon
//                    if (leftWeapon != 0)
//                    {
//                        StartCoroutine(_SheathWeapon((int)leftWeapon, (int)weaponNumber));
//                        yield return new WaitForSeconds(1.05f);
//                        if (rightWeapon != 0)
//                        {
//                            StartCoroutine(_SheathWeapon((int)rightWeapon, (int)weaponNumber));
//                            yield return new WaitForSeconds(1.05f);
//                            //and right hand weapon
//                            if (weaponNumber != 0)
//                            {
//                                StartCoroutine(_UnSheathWeapon(weaponNumber));
//                            }
//                        }
//                        if (weaponNumber != 0)
//                        {
//                            StartCoroutine(_UnSheathWeapon(weaponNumber));
//                        }
//                    }
//                    //check right hand for weapon if no left hand weapon
//                    if (rightWeapon != 0)
//                    {
//                        StartCoroutine(_SheathWeapon((int)rightWeapon, (int)weaponNumber));
//                        yield return new WaitForSeconds(1.05f);
//                        if (weaponNumber != 0)
//                        {
//                            StartCoroutine(_UnSheathWeapon(weaponNumber));
//                        }
//                    }
//                }
//                //using 1 handed weapon(s)
//                else if ((int)weaponNumber == 7)
//                {
//                    if (leftWeapon > 0)
//                    {
//                        StartCoroutine(_SheathWeapon((int)leftWeapon, (int)weaponNumber));
//                        yield return new WaitForSeconds(1.05f);
//                    }
//                    StartCoroutine(_UnSheathWeapon(weaponNumber));
//                }
//                //switching left weapon, put away left weapon if equipped
//                else if (((int)weaponNumber == 8 || (int)weaponNumber == 10 || (int)weaponNumber == 12 || (int)weaponNumber == 14 || (int)weaponNumber == 16))
//                {
//                    if (leftWeapon > 0)
//                    {
//                        StartCoroutine(_SheathWeapon((int)leftWeapon, (int)weaponNumber));
//                        yield return new WaitForSeconds(1.05f);
//                    }
//                    StartCoroutine(_UnSheathWeapon(weaponNumber));
//                }
//                //switching right weapon, put away right weapon if equipped
//                else if (((int)weaponNumber == 9 || (int)weaponNumber == 11 || (int)weaponNumber == 13 || (int)weaponNumber == 15 || (int)weaponNumber == 17))
//                {
//                    if (rightWeapon > 0)
//                    {
//                        StartCoroutine(_SheathWeapon((int)rightWeapon, (int)weaponNumber));
//                        yield return new WaitForSeconds(1.05f);
//                    }
//                    StartCoroutine(_UnSheathWeapon(weaponNumber));
//                }
//            }
//            yield return null;
//        }

//        public IEnumerator _SheathWeapon(int weaponNumber, int weaponDraw)
//        {
//            if ((weaponNumber == 8 || weaponNumber == 10 || weaponNumber == 12 || weaponNumber == 14 || weaponNumber == 16))
//            {
//                animator.SetInteger("LeftRight", 1);
//            }
//            else if ((weaponNumber == 9 || weaponNumber == 11 || weaponNumber == 13 || weaponNumber == 15 || weaponNumber == 17))
//            {
//                animator.SetInteger("LeftRight", 2);
//            }
//            if (weaponDraw == 0)
//            {
//                //if switching to unarmed, don't set "Armed" until after 2nd weapon sheath
//                if (leftWeapon == 0 && rightWeapon != 0)
//                {
//                    animator.SetBool("Armed", false);
//                }
//                if (rightWeapon == 0 && leftWeapon != 0)
//                {
//                    animator.SetBool("Armed", false);
//                }
//            }
//            animator.SetTrigger("WeaponSheathTrigger");
//            yield return new WaitForSeconds(.1f);
//            if (weaponNumber < 7)
//            {
//                leftWeapon = 0;
//                animator.SetInteger("LeftWeapon", 0);
//                rightWeapon = 0;
//                animator.SetInteger("RightWeapon", 0);
//                animator.SetBool("Shield", false);
//                animator.SetBool("Armed", false);
//            }
//            else if (weaponNumber == 7)
//            {
//                leftWeapon = 0;
//                animator.SetInteger("LeftWeapon", 0);
//                animator.SetBool("Shield", false);
//            }
//            else if ((weaponNumber == 8 || weaponNumber == 10 || weaponNumber == 12 || weaponNumber == 14 || weaponNumber == 16))
//            {
//                leftWeapon = 0;
//                animator.SetInteger("LeftWeapon", 0);
//            }
//            else if ((weaponNumber == 9 || weaponNumber == 11 || weaponNumber == 13 || weaponNumber == 15 || weaponNumber == 17))
//            {
//                rightWeapon = 0;
//                animator.SetInteger("RightWeapon", 0);
//            }
//            //if switched to unarmed
//            if (leftWeapon == 0 && rightWeapon == 0)
//            {
//                animator.SetBool("Armed", false);
//            }
//            if (leftWeapon == 0 && rightWeapon == 0)
//            {
//                animator.SetInteger("LeftRight", 0);
//                animator.SetInteger("Weapon", 0);
//                animator.SetBool("Armed", false);
//                weaponStance = WeaponStances_e.UNARMED;
//            }
//            StartCoroutine(_WeaponVisibility((Weapons_e)weaponNumber, .4f, false));
//            StartCoroutine(_LockMovementAndAttack(0, 1));
//            yield return null;
//        }

//        public IEnumerator _UnSheathWeapon(Weapons_e weaponNumber)
//        {
//            animator.SetInteger("Weapon", -1);
//            yield return new WaitForEndOfFrame();
//            yield return new WaitForEndOfFrame();
//            yield return new WaitForEndOfFrame();
//            //two handed weapons
//            if ((int)weaponNumber < 7)
//            {
//                leftWeapon = weaponNumber;
//                animator.SetInteger("LeftRight", 3);
//                if (weaponNumber == 0)
//                {
//                    weaponStance = WeaponStances_e.UNARMED;
//                }
//                if ((int)weaponNumber == 1)
//                {
//                    weaponStance = WeaponStances_e.TWOHANDSWORD;
//                    StartCoroutine(_WeaponVisibility(weaponNumber, .4f, true));
//                }
//                else if ((int)weaponNumber == 2)
//                {
//                    weaponStance = WeaponStances_e.TWOHANDSPEAR;
//                    StartCoroutine(_WeaponVisibility(weaponNumber, .5f, true));
//                }
//                else if ((int)weaponNumber == 3)
//                {
//                    weaponStance = WeaponStances_e.TWOHANDAXE;
//                    StartCoroutine(_WeaponVisibility(weaponNumber, .5f, true));
//                }
//                else if ((int)weaponNumber == 4)
//                {
//                    weaponStance = WeaponStances_e.TWOHANDBOW;
//                    StartCoroutine(_WeaponVisibility(weaponNumber, .55f, true));
//                }
//                else if ((int)weaponNumber == 5)
//                {
//                    weaponStance = WeaponStances_e.TWOHANDCROSSBOW;
//                    StartCoroutine(_WeaponVisibility(weaponNumber, .5f, true));
//                }
//                else
//                {
//                    weaponStance = WeaponStances_e.STAFF;
//                    StartCoroutine(_WeaponVisibility(weaponNumber, .6f, true));
//                }
//            }
//            //one handed weapons
//            else
//            {
//                if ((int)weaponNumber == 7)
//                {
//                    leftWeapon = Weapons_e.SHIELD;
//                    animator.SetInteger("LeftWeapon", (int)Weapons_e.SHIELD);
//                    animator.SetInteger("LeftRight", 1);
//                    StartCoroutine(_WeaponVisibility(weaponNumber, .6f, true));
//                    animator.SetBool("Shield", true);
//                }
//                else if ((int)weaponNumber == 8 || (int)weaponNumber == 10 || (int)weaponNumber == 12 || (int)weaponNumber == 14 || (int)weaponNumber == 16)
//                {
//                    animator.SetInteger("LeftRight", 1);
//                    animator.SetInteger("LeftWeapon", (int)weaponNumber);
//                    StartCoroutine(_WeaponVisibility(weaponNumber, .6f, true));
//                    leftWeapon = weaponNumber;
//                    weaponNumber = Weapons_e.SHIELD;
//                }
//                else if ((int)weaponNumber == 9 || (int)weaponNumber == 11 || (int)weaponNumber == 13 || (int)weaponNumber == 15 || (int)weaponNumber == 17)
//                {
//                    animator.SetInteger("LeftRight", 2);
//                    animator.SetInteger("RightWeapon", (int)weaponNumber);
//                    rightWeapon = weaponNumber;
//                    StartCoroutine(_WeaponVisibility(weaponNumber, .6f, true));
//                    weaponNumber = Weapons_e.SHIELD;
//                    //set shield to false for animator, will reset later
//                    if (leftWeapon == Weapons_e.SHIELD)
//                    {
//                        animator.SetBool("Shield", false);
//                    }
//                }
//            }
//            animator.SetInteger("Weapon", (int)weaponNumber);
//            animator.SetTrigger("WeaponUnsheathTrigger");
//            StartCoroutine(_LockMovementAndAttack(0, 1.1f));
//            yield return new WaitForSeconds(.1f);
//            if (leftWeapon == Weapons_e.SHIELD)
//            {
//                animator.SetBool("Shield", true);
//            }
//            if ((int)leftWeapon > 6 || (int)rightWeapon > 6)
//            {
//                animator.SetBool("Armed", true);
//                weaponStance = WeaponStances_e.ARMED;
//            }
//            //For dual blocking
//            if ((int)rightWeapon == 9 || (int)rightWeapon == 11 || (int)rightWeapon == 13 || (int)rightWeapon == 15 || (int)rightWeapon == 17)
//            {
//                if ((int)leftWeapon == 8 || (int)leftWeapon == 10 || (int)leftWeapon == 12 || (int)leftWeapon == 14 || (int)leftWeapon == 16)
//                {
//                    yield return new WaitForSeconds(.1f);
//                    animator.SetInteger("LeftRight", 3);
//                }
//            }
//            if ((int)leftWeapon == 8 || (int)leftWeapon == 10 || (int)leftWeapon == 12 || (int)leftWeapon == 14 || (int)leftWeapon == 16)
//            {
//                if ((int)rightWeapon == 9 || (int)rightWeapon == 11 || (int)rightWeapon == 13 || (int)rightWeapon == 15 || (int)rightWeapon == 17)
//                {
//                    yield return new WaitForSeconds(.1f);
//                    animator.SetInteger("LeftRight", 3);
//                }
//            }
//            yield return null;
//        }

//        public IEnumerator _WeaponVisibility(Weapons_e weaponNumber, float delayTime, bool visible)
//        {
//            yield return new WaitForSeconds(delayTime);
//            if (weaponNumber == Weapons_e.TWOHANDSWORD)
//            {
//                twohandsword.SetActive(visible);
//            }
//            if (weaponNumber == Weapons_e.TWOHANDSPEAR)
//            {
//                twohandspear.SetActive(visible);
//            }
//            if (weaponNumber == Weapons_e.TWOHANDAXE)
//            {
//                twohandaxe.SetActive(visible);
//            }
//            if (weaponNumber == Weapons_e.TWOHANDBOW)
//            {
//                twohandbow.SetActive(visible);
//            }
//            if (weaponNumber == Weapons_e.TWOHANDCROSSBOW)
//            {
//                twohandcrossbow.SetActive(visible);
//            }
//            if (weaponNumber == Weapons_e.STAFF)
//            {
//                staff.SetActive(visible);
//            }
//            if (weaponNumber == Weapons_e.SHIELD)
//            {
//                shield.SetActive(visible);
//            }
//            if (weaponNumber == Weapons_e.LEFT_SWORD)
//            {
//                swordL.SetActive(visible);
//            }
//            if (weaponNumber == Weapons_e.RIGHT_SWORD)
//            {
//                swordR.SetActive(visible);
//            }
//            if (weaponNumber == Weapons_e.LEFT_MACE)
//            {
//                maceL.SetActive(visible);
//            }
//            if (weaponNumber == Weapons_e.RIGHT_MACE)
//            {
//                maceR.SetActive(visible);
//            }
//            if (weaponNumber == Weapons_e.LEFT_DAGGER)
//            {
//                daggerL.SetActive(visible);
//            }
//            if (weaponNumber == Weapons_e.RIGHT_DAGGER)
//            {
//                daggerR.SetActive(visible);
//            }
//            if (weaponNumber == Weapons_e.LEFT_ITEM)
//            {
//                itemL.SetActive(visible);
//            }
//            if (weaponNumber == Weapons_e.RIGHT_ITEM)
//            {
//                itemR.SetActive(visible);
//            }
//            if (weaponNumber == Weapons_e.LEFT_PISTOL)
//            {
//                pistolL.SetActive(visible);
//            }
//            if (weaponNumber == Weapons_e.RIGHT_PISTOL)
//            {
//                pistolR.SetActive(visible);
//            }
//            yield return null;
//        }

//        public IEnumerator _BlockHitReact()
//        {
//            int hits = 2;
//            int hitNumber = Random.Range(0, hits);
//            animator.SetTrigger("BlockGetHit" + (hitNumber + 1).ToString() + "Trigger");
//            StartCoroutine(_LockMovementAndAttack(.1f, .4f));
//            StartCoroutine(_Knockback(-transform.forward, 3, 3));
//            yield return null;
//        }

//        public IEnumerator _BlockBreak()
//        {
//            animator.applyRootMotion = true;
//            animator.SetTrigger("BlockBreakTrigger");
//            yield return new WaitForSeconds(1f);
//            animator.applyRootMotion = false;
//        }

//        #endregion

//        #region GUI

//        void OnGUI()
//        {
//            if (!isDead)
//            {
//                if (weaponStance == WeaponStances_e.RELAX || weaponStance != WeaponStances_e.UNARMED)
//                {
//                    if (GUI.Button(new Rect(1115, 310, 100, 30), "Unarmed"))
//                    {
//                        animator.SetBool("Relax", false);
//                        isRelax = false;
//                        StartCoroutine(_SwitchWeapon(0));
//                        weaponStance = WeaponStances_e.UNARMED;
//                        canAction = true;
//                        animator.SetTrigger("RelaxTrigger");
//                    }
//                }
//                if (canAction && !isRelax)
//                {
//                    if (isGrounded)
//                    {
//                        //bow and crossbow can't block
//                        if (weaponStance != WeaponStances_e.TWOHANDBOW && weaponStance != WeaponStances_e.TWOHANDCROSSBOW)
//                        {
//                            //if character is not blocking
//                            blockGui = GUI.Toggle(new Rect(25, 215, 100, 30), blockGui, "Block");
//                        }
//                        //get hit
//                        if (blockGui)
//                        {
//                            if (GUI.Button(new Rect(30, 240, 100, 30), "Get Hit"))
//                            {
//                                StartCoroutine(_BlockHitReact());
//                            }
//                            if (GUI.Button(new Rect(30, 270, 100, 30), "Block Break"))
//                            {
//                                StartCoroutine(_BlockBreak());
//                            }
//                        }
//                        else if (!isBlocking)
//                        {
//                            if (!isBlocking)
//                            {
//                                if (GUI.Button(new Rect(25, 15, 100, 30), "Roll Forward"))
//                                {
//                                    targetDashDirection = transform.forward;
//                                    StartCoroutine(_Roll(1));
//                                }
//                                if (GUI.Button(new Rect(130, 15, 100, 30), "Roll Backward"))
//                                {
//                                    targetDashDirection = -transform.forward;
//                                    StartCoroutine(_Roll(3));
//                                }
//                                if (GUI.Button(new Rect(25, 45, 100, 30), "Roll Left"))
//                                {
//                                    targetDashDirection = -transform.right;
//                                    StartCoroutine(_Roll(4));
//                                }
//                                if (GUI.Button(new Rect(130, 45, 100, 30), "Roll Right"))
//                                {
//                                    targetDashDirection = transform.right;
//                                    StartCoroutine(_Roll(2));
//                                }
//                                //ATTACK LEFT
//                                if (weaponStance != WeaponStances_e.ARMED || (weaponStance == WeaponStances_e.ARMED && (int)leftWeapon != 0) && (int)leftWeapon != 7)
//                                {
//                                    if (GUI.Button(new Rect(25, 85, 100, 30), "Attack L"))
//                                    {
//                                        Attack(1);
//                                    }
//                                }
//                                //ATTACK RIGHT
//                                if (weaponStance != WeaponStances_e.ARMED || (weaponStance == WeaponStances_e.ARMED && (int)rightWeapon != 0))
//                                {
//                                    if (GUI.Button(new Rect(130, 85, 100, 30), "Attack R"))
//                                    {
//                                        Attack(2);
//                                    }
//                                }
//                                //ATTACK DUAL
//                                if ((int)leftWeapon > 7 && (int)rightWeapon > 7 && (int)leftWeapon != 14)
//                                {
//                                    if ((int)rightWeapon != 15)
//                                    {
//                                        if (((int)leftWeapon != 16 && (int)rightWeapon != 17))
//                                        {
//                                            if (GUI.Button(new Rect(235, 85, 100, 30), "Attack Dual"))
//                                            {
//                                                Attack(3);
//                                            }
//                                        }
//                                        else if (((int)leftWeapon == 16 && (int)rightWeapon == 17))
//                                        {
//                                            if (GUI.Button(new Rect(235, 85, 100, 30), "Attack Dual"))
//                                            {
//                                                Attack(3);
//                                            }
//                                        }
//                                    }
//                                }
//                                if (GUI.Button(new Rect(25, 115, 100, 30), "Left Kick"))
//                                {
//                                    AttackKick(1);
//                                }
//                                if (GUI.Button(new Rect(130, 115, 100, 30), "Right Kick"))
//                                {
//                                    AttackKick(2);
//                                }
//                                if (weaponStance == WeaponStances_e.ARMED || weaponStance == WeaponStances_e.UNARMED || weaponStance == WeaponStances_e.STAFF)
//                                {
//                                    if (GUI.Button(new Rect(25, 330, 100, 30), "Cast Atk Left"))
//                                    {
//                                        CastAttack(1);
//                                    }
//                                    if (weaponStance != WeaponStances_e.STAFF)
//                                    {
//                                        if (GUI.Button(new Rect(130, 330, 100, 30), "Cast Atk Right"))
//                                        {
//                                            CastAttack(2);
//                                        }
//                                        if (GUI.Button(new Rect(80, 365, 100, 30), "Cast Dual"))
//                                        {
//                                            CastAttack(3);
//                                        }
//                                    }
//                                }
//                                if (GUI.Button(new Rect(30, 240, 100, 30), "Get Hit"))
//                                {
//                                    //CmdSpawnEnemy();

//                                }
//                                if (weaponStance == WeaponStances_e.UNARMED)
//                                {
//                                    if (GUI.Button(new Rect(1115, 310, 100, 30), "Relax"))
//                                    {
//                                        animator.SetBool("Relax", true);
//                                        isRelax = true;
//                                        weaponStance = WeaponStances_e.RELAX;
//                                        canAction = false;
//                                        animator.SetTrigger("RelaxTrigger");
//                                    }
//                                }
//                                if (weaponStance != WeaponStances_e.TWOHANDSWORD)
//                                {
//                                    if (GUI.Button(new Rect(1115, 350, 100, 30), "2 Hand Sword"))
//                                    {
//                                        StartCoroutine(_SwitchWeapon(Weapons_e.TWOHANDSWORD));
//                                    }
//                                }
//                                if (weaponStance != WeaponStances_e.TWOHANDSPEAR)
//                                {
//                                    if (GUI.Button(new Rect(1115, 380, 100, 30), "2 Hand Spear"))
//                                    {
//                                        StartCoroutine(_SwitchWeapon(Weapons_e.TWOHANDSPEAR));
//                                    }
//                                }
//                                if (weaponStance != WeaponStances_e.TWOHANDAXE)
//                                {
//                                    if (GUI.Button(new Rect(1115, 410, 100, 30), "2 Hand Axe"))
//                                    {
//                                        StartCoroutine(_SwitchWeapon(Weapons_e.TWOHANDAXE));
//                                    }
//                                }
//                                if (weaponStance != WeaponStances_e.TWOHANDBOW)
//                                {
//                                    if (GUI.Button(new Rect(1115, 440, 100, 30), "2 Hand Bow"))
//                                    {
//                                        StartCoroutine(_SwitchWeapon(Weapons_e.TWOHANDBOW));
//                                    }
//                                }
//                                if (weaponStance != WeaponStances_e.TWOHANDCROSSBOW)
//                                {
//                                    if (GUI.Button(new Rect(1115, 470, 100, 30), "Crossbow"))
//                                    {
//                                        StartCoroutine(_SwitchWeapon(Weapons_e.TWOHANDCROSSBOW));
//                                    }
//                                }
//                                if (weaponStance != WeaponStances_e.STAFF)
//                                {
//                                    if (GUI.Button(new Rect(1115, 500, 100, 30), "Staff"))
//                                    {
//                                        StartCoroutine(_SwitchWeapon(Weapons_e.STAFF));
//                                    }
//                                }
//                                if ((int)leftWeapon != 7)
//                                {
//                                    if (GUI.Button(new Rect(1115, 700, 100, 30), "Shield"))
//                                    {
//                                        StartCoroutine(_SwitchWeapon(Weapons_e.SHIELD));
//                                    }
//                                }
//                                if ((int)leftWeapon != 8)
//                                {
//                                    if (GUI.Button(new Rect(1065, 540, 100, 30), "Left Sword"))
//                                    {
//                                        StartCoroutine(_SwitchWeapon(Weapons_e.LEFT_SWORD));
//                                    }
//                                }
//                                if ((int)rightWeapon != 9)
//                                {
//                                    if (GUI.Button(new Rect(1165, 540, 100, 30), "Right Sword"))
//                                    {
//                                        StartCoroutine(_SwitchWeapon(Weapons_e.RIGHT_SWORD));
//                                    }
//                                }
//                                if ((int)leftWeapon != 10)
//                                {
//                                    if (GUI.Button(new Rect(1065, 570, 100, 30), "Left Mace"))
//                                    {
//                                        StartCoroutine(_SwitchWeapon(Weapons_e.LEFT_MACE));
//                                    }
//                                }
//                                if ((int)rightWeapon != 11)
//                                {
//                                    if (GUI.Button(new Rect(1165, 570, 100, 30), "Right Mace"))
//                                    {
//                                        StartCoroutine(_SwitchWeapon(Weapons_e.RIGHT_MACE));
//                                    }
//                                }
//                                if ((int)leftWeapon != 12)
//                                {
//                                    if (GUI.Button(new Rect(1065, 600, 100, 30), "Left Dagger"))
//                                    {
//                                        StartCoroutine(_SwitchWeapon(Weapons_e.LEFT_DAGGER));
//                                    }
//                                }
//                                if ((int)leftWeapon != 13)
//                                {
//                                    if (GUI.Button(new Rect(1165, 600, 100, 30), "Right Dagger"))
//                                    {
//                                        StartCoroutine(_SwitchWeapon(Weapons_e.RIGHT_DAGGER));
//                                    }
//                                }
//                                if ((int)leftWeapon != 14)
//                                {
//                                    if (GUI.Button(new Rect(1065, 630, 100, 30), "Left Item"))
//                                    {
//                                        StartCoroutine(_SwitchWeapon(Weapons_e.LEFT_ITEM));
//                                    }
//                                }
//                                if ((int)leftWeapon != 15)
//                                {
//                                    if (GUI.Button(new Rect(1165, 630, 100, 30), "Right Item"))
//                                    {
//                                        StartCoroutine(_SwitchWeapon(Weapons_e.RIGHT_ITEM));
//                                    }
//                                }
//                                if ((int)leftWeapon != 16)
//                                {
//                                    if (GUI.Button(new Rect(1065, 660, 100, 30), "Left Pistol"))
//                                    {
//                                        StartCoroutine(_SwitchWeapon(Weapons_e.LEFT_PISTOL));
//                                    }
//                                }
//                                if ((int)leftWeapon != 17)
//                                {
//                                    if (GUI.Button(new Rect(1165, 660, 100, 30), "Right Pistol"))
//                                    {
//                                        StartCoroutine(_SwitchWeapon(Weapons_e.RIGHT_PISTOL));
//                                    }
//                                }
//                            }
//                        }
//                    }
//                    if (canJump || canDoubleJump)
//                    {
//                        if (isGrounded)
//                        {
//                            if (GUI.Button(new Rect(25, 165, 100, 30), "Jump"))
//                            {
//                                if (canJump && isGrounded)
//                                {
//                                    StartCoroutine(_Jump());
//                                }
//                            }
//                        }
//                        else
//                        {
//                            if (GUI.Button(new Rect(25, 165, 100, 30), "Double Jump"))
//                            {
//                                if (canDoubleJump && !isDoubleJumping)
//                                {
//                                    StartCoroutine(_Jump());
//                                }
//                            }
//                        }
//                    }
//                    if (!blockGui && !isBlocking && isGrounded)
//                    {
//                        if (GUI.Button(new Rect(30, 270, 100, 30), "Death"))
//                        {
//                            StartCoroutine(_Death());
//                        }
//                    }
//                }
//            }
//            if (isDead)
//            {
//                if (GUI.Button(new Rect(30, 270, 100, 30), "Revive"))
//                {
//                    StartCoroutine(_Revive());
//                }
//            }
//        }

//        //[Command]
//        //public void CmdSpawnEnemy()
//        //{
//        //    var prefab = NetworkManager.singleton.spawnPrefabs.Find(p => p.name == "Enemy");
//        //    var spawnRotation = Quaternion.Euler(0f, 0f, 0f);
//        //    var enemy = GameObject.Instantiate(prefab, new Vector3(0f, 3f, 0f), spawnRotation);
//        //    NetworkServer.Spawn(enemy.gameObject);
//        //}

//        #endregion

//    }
//}