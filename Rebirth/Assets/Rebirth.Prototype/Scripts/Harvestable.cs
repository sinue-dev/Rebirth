using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rebirth.Prototype
{
	public class Harvestable : MonoBehaviour
	{
		public enum HarvestMethod_e
		{
			GATHER,
			PICK,
			CHOP,
			CUT
		}
		public HarvestMethod_e harvestMethod = HarvestMethod_e.GATHER;

		public enum HarvestType_e
		{
			BUSH,
			TREE,
			ROCK
		}
		public HarvestType_e harvestType;

		[Range(1, 6)]
		public int HarvestAmount;

		private CItem harvestedItem { get; set; }

		public bool bIsHarvesting = false;
		public bool bTriggered = false;
	}
}
