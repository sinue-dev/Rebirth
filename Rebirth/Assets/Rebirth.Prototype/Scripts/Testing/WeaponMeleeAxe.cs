using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMeleeAxe : WeaponBase
{
	public override void SwingLeft()
	{
		
	}

	public override void SwingRight()
	{
		
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			movement m = other.GetComponent<movement>();
			if (m == null) return;

			m.DoDamage(this);	
				
		}

		if (other.CompareTag("AI"))
		{
			ai enemy = other.GetComponent<ai>();
			if (enemy == null) return;

			enemy.DoDamage(this);		
		}

		// Collider nach einem Hit wieder deaktivieren
		col.enabled = false;
	}
}
