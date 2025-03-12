using System;
using UnityEngine;

namespace RDP.Building {
	[Serializable]
	public class BuildingUnitData {
		public GameObject unit;
		public Transform node;
		public BuildingInteractor linkedBuilding;

		public BuildingUnitData(GameObject unit, Transform node, BuildingInteractor building) {
			this.unit = unit;
			this.node = node;
			linkedBuilding = building;
		}

		public bool IsEmpty() {
			bool res = unit == null && node == null && linkedBuilding == null;
			// Debug.Log($"Name: {this} this: {unit} {node} {linkedBuilding} {res}");
			return res;
		}
	}
}