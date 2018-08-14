using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
	public movement player;
	public Collider col;

	public void Start()
	{
		this.player = GameObject.FindGameObjectWithTag("Player").GetComponent<movement>();
	}

	public virtual void SwingLeft()
	{

	}

	public virtual void SwingRight()
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