using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    float gravityForce = -9.81f;
	public void ApplyGravity(Transform receiver)
    {
        Rigidbody rb = receiver.GetComponent<Rigidbody>();
        Vector3 forceUp = receiver.position - transform.position;
        Vector3 dir = gravityForce * forceUp.normalized;

        rb.AddForce(dir); 
        Vector3 receiverUp = receiver.up;

        receiver.rotation = Quaternion.FromToRotation(receiverUp, forceUp) * receiver.rotation;
    }
}
