using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
	public static InputManager singleton = null;



	[Header("Action")]
	public KeyCode keyInteract;
	public KeyCode keySpecialAction;
	public KeyCode keyToggleWalk;
	public KeyCode keyRun;
	// STRG

	[Header("Combat")]
	public KeyCode keyToggleCombat;
	public KeyCode keyBagSlot1;
	public KeyCode keyBagSlot2;
	public KeyCode keyBagSlot3;
	public KeyCode keyBagSlot4;
	public KeyCode keyBagSlot5;
	public KeyCode keyBagSlot6;
	public KeyCode keyBagSlot7;
	public KeyCode keyBagSlot8;
	public KeyCode keyJump;
	public KeyCode keyDodge;
	public KeyCode keyLockOnTarget;
	public KeyCode keyLeftLightAttack;
	public KeyCode keyLeftStrongAttack;
	public KeyCode keyRightLightAttack;
	public KeyCode keyRightStrongAttack;
	// Q
	// R
	// T
	// V
	// X

	// Bag Container Window wie das neue bei Conan mit Vorschau


	[Header("Panels")]
	public KeyCode keyCharacter;
	public KeyCode keyMap;

	[Header("Other")]
	public KeyCode keyMainMenu;
	public KeyCode keyToggleHud;

	void Awake()
	{
		if (singleton == null)
		{
			singleton = this;
		}

		else if (singleton != this)
		{
			Destroy(gameObject);
		}

		// Dont destroy on reloading the scene
		DontDestroyOnLoad(gameObject);
	}

	public InputManager()
	{
		keyInteract = KeyCode.E;
		keySpecialAction = KeyCode.F;
		keyToggleWalk = KeyCode.KeypadMinus;
		keyRun = KeyCode.LeftShift;

		keyToggleCombat = KeyCode.Tab;
		keyBagSlot1 = KeyCode.Alpha1;
		keyBagSlot2 = KeyCode.Alpha2;
		keyBagSlot3 = KeyCode.Alpha3;
		keyBagSlot4 = KeyCode.Alpha4;
		keyBagSlot5 = KeyCode.Alpha5;
		keyBagSlot6 = KeyCode.Alpha6;
		keyBagSlot7 = KeyCode.Alpha7;
		keyBagSlot8 = KeyCode.Alpha8;
		keyJump = KeyCode.Space;
		keyDodge = KeyCode.LeftAlt;
		keyLockOnTarget = KeyCode.Z;
		keyLeftLightAttack = KeyCode.Mouse0;
		keyRightLightAttack = KeyCode.Mouse1;

		keyCharacter = KeyCode.C;
		keyMap = KeyCode.M;

		keyMainMenu = KeyCode.Escape;
		keyToggleHud = KeyCode.H;
	}

	private static float lookAngle = 0f;
	private static float tiltAngle = 0f;
	public static Quaternion GetMouseRotationInput(float mouseSensitivity = 3f, float minTiltAngle = -75f, float maxTiltAngle = 45f)
	{
		float mouseX = Input.GetAxis("Mouse X");
		float mouseY = Input.GetAxis("Mouse Y");

		// Adjust the look angle (Y Rotation)
		lookAngle += mouseX * mouseSensitivity;
		lookAngle %= 360f;

		// Adjust the tilt angle (X Rotation)
		tiltAngle += mouseY * mouseSensitivity;
		tiltAngle %= 360f;
		tiltAngle = MathfExtensions.ClampAngle(tiltAngle, minTiltAngle, maxTiltAngle);

		var controlRotation = Quaternion.Euler(-tiltAngle, lookAngle, 0f);
		return controlRotation;
	}

	#region Action

	public bool Interact()
	{
		return Input.GetKeyDown(keyInteract);
	}

	public bool SpecialAction()
	{
		return Input.GetKeyDown(keySpecialAction);
	}

	public bool Run()
	{
		return Input.GetKeyDown(keyRun);
	}

	public bool ToggleWalk()
	{
		return Input.GetKeyDown(keyToggleWalk);
	}

	public float MoveHorizontal()
	{
		return Input.GetAxis("Horizontal");
	}

	public bool MoveForward()
	{
		return Input.GetKey(KeyCode.W);
	}

	public bool MoveBackward()
	{
		return Input.GetKey(KeyCode.S);
	}

	public bool MoveLeft()
	{
		return Input.GetKey(KeyCode.A);
	}

	public bool MoveRight()
	{
		return Input.GetKey(KeyCode.D);
	}

	public float MoveVertical()
	{
		return Input.GetAxis("Vertical");
	}

	public bool BagSlot1()
	{
		return Input.GetKey(keyBagSlot1);
	}

	public bool BagSlot2()
	{
		return Input.GetKey(keyBagSlot2);
	}

	public bool BagSlot3()
	{
		return Input.GetKey(keyBagSlot3);
	}

	public bool BagSlot4()
	{
		return Input.GetKey(keyBagSlot4);
	}

	public bool BagSlot5()
	{
		return Input.GetKey(keyBagSlot5);
	}

	public bool BagSlot6()
	{
		return Input.GetKey(keyBagSlot6);
	}

	public bool BagSlot7()
	{
		return Input.GetKey(keyBagSlot7);
	}

	public bool BagSlot8()
	{
		return Input.GetKey(keyBagSlot8);
	}
	#endregion

	#region Combat
	public bool ToggleCombat()
	{
		return Input.GetKeyDown(keyToggleCombat);
	}

	public bool Jump()
	{
		return Input.GetKey(keyJump);
	}

	public bool Dodge()
	{
		return Input.GetKeyDown(keyDodge);
	}

	public bool LockOnTarget()
	{
		return Input.GetKeyDown(keyLockOnTarget);
	}

	public bool LeftLightAttack()
	{
		return Input.GetMouseButtonDown(0);
	}

	public bool RightLightAttack()
	{
		return Input.GetMouseButtonDown(1);
	}

	private bool StrongAttackModifier()
	{
		return Input.GetKey(KeyCode.LeftShift);
	}

	public bool LeftStrongAttack()
	{
		return StrongAttackModifier() && Input.GetMouseButtonDown(0);
	}

	public bool RightStrongAttack()
	{
		return StrongAttackModifier() && Input.GetMouseButtonDown(1);
	}
	#endregion

	#region Panels
	public bool Character()
	{
		return Input.GetKeyDown(keyCharacter);
	}

	public bool Map()
	{
		return Input.GetKeyDown(keyMap);
	}
	#endregion

	#region Other
	public bool MainMenu()
	{
		return Input.GetKeyDown(keyMainMenu);
	}

	public bool ToggleHud()
	{
		return Input.GetKeyDown(keyToggleHud);
	}
	#endregion
}
