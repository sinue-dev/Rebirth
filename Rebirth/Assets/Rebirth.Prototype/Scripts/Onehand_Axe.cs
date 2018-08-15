using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rebirth.Prototype
{
    public class Onehand_Axe : Weapon
    {
		public override void OnUse()
        {
			character.Controller.StartCoroutine(character.Controller._SwitchWeapon(this));
        }

        public override void OnAction()
        {
			character.Controller.Attack(this);
        }
    }
}