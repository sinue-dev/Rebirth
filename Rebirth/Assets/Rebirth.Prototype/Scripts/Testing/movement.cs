using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class movement : MonoBehaviour {

	public Animator _animator;

	public float MovementSpeed = 2f;
	public float TurnSpeed = 1f;

	float currentAngle;
	Vector3 EulerAngleVelocity;

	Quaternion targetRotation;
	Rigidbody rb;

	const float WALK_SPEED = .5f;
	const float RUN_SPEED = 1f;

	public GameObject fistLeft;
	public GameObject fistRight;
	public GameObject equippedWeapon;

	public bool IsWalking
	{
		get { return _animator.GetBool("IsWalking"); }
		set { _animator.SetBool("IsWalking", value); }
	}

	public bool IsRunning
	{
		get { return _animator.GetBool("IsRunning"); }
		set { _animator.SetBool("IsRunning", value); }
	}

	public bool IsArmed
	{
		get { return _animator.GetBool("IsArmed"); }
		set { _animator.SetBool("IsArmed", value); }
	}

	public bool IsAttacking
	{
		get { return _animator.GetBool("IsAttacking"); }
		set { _animator.SetBool("IsAttacking", value); }
	}

	public float MoveZ
	{
		get { return _animator.GetFloat("v"); }
		set { _animator.SetFloat("v", value); }
	}

	public float MoveX
	{
		get { return _animator.GetFloat("h"); }
		set { _animator.SetFloat("h", value); }
	}


	// Use this for initialization
	void Start ()
	{
		_animator = GetComponent<Animator>();
		rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (_animator == null) return;

		if(Input.GetKeyDown("1"))
		{
			//IsAttacking = true;
			WeaponBase weapon = equippedWeapon.GetComponent<WeaponBase>();
			IsArmed = !weapon.State();
		}

		Running();
		Attack();
	}

	void FixedUpdate()
	{
		Rotate();
		Move();		
	}

	public void DoDamage(WeaponBase weapon)
	{
		Debug.Log("damaged...");
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

		WeaponBase weapon = equippedWeapon.GetComponent<WeaponBase>();
		if (weapon != null)
		{
			weapon.col.enabled = true;
		}
	}

	public void EventDeactivateWeaponDamage(int i)
	{
		if (!IsArmed) return;

		WeaponBase weapon = equippedWeapon.GetComponent<WeaponBase>();
		if (weapon != null)
		{
			weapon.col.enabled = false;
		}
	}

	public void EventShowWeapon(int i)
	{
		WeaponBase weapon = equippedWeapon.GetComponent<WeaponBase>();
		if (weapon != null)
		{
			weapon.Toggle(IsArmed);
		}
	}

	public void EventHideWeapon(int i)
	{
		WeaponBase weapon = equippedWeapon.GetComponent<WeaponBase>();
		if (weapon != null)
		{
			weapon.Toggle(IsArmed);
		}
	}
	#endregion	


	private void Attack()
	{
		if(Input.GetMouseButtonDown(0) && !IsAttacking)
		{
			if(IsArmed && equippedWeapon != null)
			{
				WeaponBase wb = equippedWeapon.GetComponent<WeaponBase>();
				wb.SwingLeft();
				IsAttacking = true;

				StartCoroutine(Attacking(_animator.GetCurrentAnimatorStateInfo(1).length));
			}
			else if(IsArmed)
			{
				// In Kampf-Modus aber keine Waffe ausgerüstet = Faustkampf
				
			}
		}

		if(Input.GetMouseButtonDown(1) && !IsAttacking)
		{
			if (IsArmed && equippedWeapon != null)
			{
				WeaponBase wb = equippedWeapon.GetComponent<WeaponBase>();
				wb.SwingRight();
				IsAttacking = true;
				
				StartCoroutine(Attacking(_animator.GetCurrentAnimatorStateInfo(1).length));
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
		rb.MovePosition(transform.position + movement );
		//rb.MovePosition(transform.position + transform.forward * speed * MovementSpeed * Time.deltaTime);
		//transform.position += Vector3.forward * z * MovementSpeed * Time.deltaTime;
		//transform.position += Vector3.right * x * * MovementSpeed * Time.deltaTime;
	}

	private void Rotate()
	{
		targetRotation = Quaternion.Euler(0, -currentAngle, 0);
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, TurnSpeed * Time.deltaTime);
	}

	private void CalculateDirection()
	{
		currentAngle = Mathf.Atan2(MoveX, MoveZ);
		currentAngle = Mathf.Rad2Deg * currentAngle;
		currentAngle += Camera.main.transform.eulerAngles.y;
	}
}
