using System.Collections.Generic;
using UnityEngine;

namespace RDP.Building {
	public class BuildingInteractor : MonoBehaviour {
		//This will handle how many Units that is currently interacting with the building
		public List<GameObject> unitCurrentlyInteracting = new List<GameObject>();

		//This will handle how many open spots there are.
		public List<Transform> availableInsertionPoints = new List<Transform>();
		public List<Transform> usedInsertionPoints = new List<Transform>();
		public List<BuildingUnitData> unitBuildingDatas = new List<BuildingUnitData>();
		private Building _building;

		public int AvailableInsertionPoints => availableInsertionPoints.Count;
		private void Start() {
			_building = GetComponent<Building>();
		}

		//Call this when Unit reaches insertion point target
		public void Interact(GameObject unit) {
			unitCurrentlyInteracting.Add(unit);
			_building.UpdateUnitsCurrentlyInteracting(unitCurrentlyInteracting);
		}

		public void Remove(GameObject unit) {
			_building.UpdateUnitsCurrentlyInteracting(unitCurrentlyInteracting);
			unitCurrentlyInteracting.Remove(unit);
		}

		public Transform GetInsertionPointOccupiedByUnit(GameObject unit) {
			// search buildingUnitData for unit
			Transform result = null;
			for (int i = 0; i < unitBuildingDatas.Count; i++) {
				if (unitBuildingDatas[i].unit == unit) {
					result = unitBuildingDatas[i].node;
				}
			}

			return result;
		}

		public List<Transform> GetInsertionPoints => availableInsertionPoints;

		public void RemoveUnit(BuildingUnitData unitData) {
			Debug.Log("Removing Unit... RemoveUnit()");
			unitBuildingDatas.Remove(unitData);
			unitCurrentlyInteracting.Remove(unitData.unit);

			usedInsertionPoints.Remove(unitData.node);
			availableInsertionPoints.Add(unitData.node);
		}
	}
}