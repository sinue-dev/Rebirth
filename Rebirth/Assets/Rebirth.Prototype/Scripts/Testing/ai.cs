﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rebirth.Prototype
{
	[RequireComponent(typeof(Rigidbody))]
	public class ai : MonoBehaviour
	{
		public float targetDistance;
		public float enemyLookDistance;
		public float attackDistance;
		public float enemyMovementSpeed;
		public float damping;
		public Transform target;
		Rigidbody rb;
		Renderer render;

		void Start()
		{
			render = GetComponent<Renderer>();
			rb = GetComponent<Rigidbody>();
		}

		private void Update()
		{
			if (target == null)
			{
				target = (GameManager.singleton.LocalPlayer == null) ? null : GameManager.singleton.LocalPlayer.transform;
			}
		}

		void FixedUpdate()
		{
			if (target == null) return;

			targetDistance = Vector3.Distance(target.position, transform.position);
			if (targetDistance < enemyLookDistance)
			{
				render.material.color = Color.yellow;
				lookAtPlayer();
			}

			if (targetDistance < attackDistance)
			{
				render.material.color = Color.red;
				//Attack();
			}
			else
			{
				render.material.color = Color.blue;
			}
		}

		void Attack()
		{
			Vector3 movement = transform.forward * enemyMovementSpeed * Time.deltaTime;
			rb.MovePosition(transform.position + movement);
			//rb.AddForce(transform.forward * enemyMovementSpeed);
		}

		public void DoDamage(Weapon weapon)
		{
			Debug.Log("damaged...");
		}

		void lookAtPlayer()
		{
			Quaternion rotation = Quaternion.LookRotation(target.position - transform.position);
			transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
		}
	}
}
