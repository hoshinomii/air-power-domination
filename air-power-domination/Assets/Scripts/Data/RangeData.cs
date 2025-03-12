using System.Collections.Generic;
using UnityEngine;

namespace RDP.Data {
	[CreateAssetMenu(fileName = "Data", menuName = "Prototype 1/Range Data", order = 1)]
	public class RangeData : ScriptableObject {
		[SerializeField] private List<RangeLevel> rangeLevels = new List<RangeLevel>();

		public float GetRangeLevel(int PlaneCount) {
			RangeLevel closestLevel = rangeLevels[0];
			foreach (RangeLevel level in rangeLevels)
				if (level.amount <= PlaneCount)
					closestLevel = level;
			return closestLevel.range;
		}
	}
}