using System.Collections.Generic;
using RDP.Networking.Shared.Game.Data;
using UnityEngine;

namespace RDP.Networking.Shared.ScriptableObjects {
	[CreateAssetMenu(fileName = "Data", menuName = "Prototype 1/UnitData List", order = 1)]
	public class UnitDataList : ScriptableObject {
		public List<UnitData> unitsToSpawn = new List<UnitData>();
	}
}