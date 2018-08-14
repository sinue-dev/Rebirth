using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMeleeFist : WeaponBase
{
	public override void SwingLeft()
	{
		player._animator.SetTrigger("AttackLMB");
	}

	public override void SwingRight()
	{
		player._animator.SetTrigger("AttackRMB");
	}

	void OnTriggerEnter(Collider other)
	{
		if (player.equippedWeapon != null) return;

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
