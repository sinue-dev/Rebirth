using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rebirth.Prototype
{
	public class Magnetism : MonoBehaviour
	{
		public Transform target;
		public float range = 2;
		public float strength = 5;

		void Update()
		{
			if (target != null)
			{
				if (InRange())
				{
					Attract();
				}
			}
		}

		private bool InRange()
		{
			return Vector3.Distance(transform.position, target.position) <= range;
		}

		private void Attract()
		{
			transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * strength);
		}
	}
}