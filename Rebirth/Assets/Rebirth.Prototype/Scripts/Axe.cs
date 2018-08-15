using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rebirth.Prototype
{
    public class Axe : ItemBase
    {

        public override void OnUse()
        {
			character.Controller.StartCoroutine(character.Controller._SwitchWeapon(this));
        }

        public override void ItemAction()
        {
			character.Controller.Attack(this);
        }
    }
}