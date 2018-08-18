using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rebirth.Prototype
{
    public class Weapon_Twohand : Weapon
    {
        public override void OnUse()
        {
            GameManager.singleton.LocalPlayer.Controller.StartCoroutine(GameManager.singleton.LocalPlayer.Controller._SwitchWeapon(this));
        }

        public override void OnAction()
        {
            GameManager.singleton.LocalPlayer.Controller.Attack(this);
        }
    }
}