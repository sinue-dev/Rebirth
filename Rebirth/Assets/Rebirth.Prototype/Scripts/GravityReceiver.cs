using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityReceiver : MonoBehaviour
{
    public Gravity gravity;

    private Rigidbody rb;

	void Start ()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.freezeRotation = true;
	}
	
	void FixedUpdate ()
    {
        if (gravity == null) return;
        
        gravity.ApplyGravity(transform);	
	}
}
