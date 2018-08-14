using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rebirth.Prototype
{
    public class Axe : ItemBase
    {

        public override void OnUse()
        {
            //base.OnUse();
            player.Controller.StartCoroutine(player.Controller._SwitchWeapon(Weapons_e.LEFT_SWORD));
        }

        public override void ItemAction()
        {
            player.Controller.OnAttackLeft();
        }
    }
}