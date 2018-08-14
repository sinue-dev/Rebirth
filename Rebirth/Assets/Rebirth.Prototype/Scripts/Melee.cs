using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Rebirth.Prototype
{
    //public class Melee : NetworkBehaviour
    //{
    //    RaycastHit hit;
    //    RebirthPlayerController character;

    //    //private Collider col;

    //    //void Awake()
    //    //{
    //    //    col = GetComponent<Collider>();
    //    //}

    //    public void Cmd_HitTarget(GameObject obj, float damage)
    //    { 
    //        obj.GetComponent<RebirthPlayerController>().Stats.RpcTakeDamage(damage);
    //    }

    //    void DoDamage(Collider col)
    //    {
    //        RebirthPlayerController hitCharacter = col.gameObject.GetComponent<RebirthPlayerController>();
    //        if (hitCharacter != null)
    //        {
    //            if (!hitCharacter.Controller.isBlocking)
    //            {
    //                hitCharacter.Controller.GetHit();
    //                Cmd_HitTarget(hitCharacter.gameObject, character.Stats.cMelee.FinalValue);
    //            }
    //            else
    //            {
    //                if (hitCharacter.Stats.Endurance - 20 >= 0) // Blocken zieht 20 Endurance ab!
    //                    StartCoroutine(hitCharacter.Controller._BlockHitReact());
    //                else
    //                    StartCoroutine(hitCharacter.Controller._BlockBreak());
    //            }
    //        }
    //    }

    //    void OnTriggerEnter(Collider col)
    //    {
    //        if (col.tag == "Player")
    //        {
    //            RebirthPlayerController hitCharacter = col.gameObject.GetComponent<RebirthPlayerController>();
    //            if (hitCharacter.name != character.name)
    //            {
    //                Debug.Log(character.name + " hit: " + hitCharacter.name);
    //                DoDamage(col);
    //            }
    //        }
    //    }


    //}
}
