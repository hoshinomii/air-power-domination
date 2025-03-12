using System.Collections.Generic;
using System.Linq;
using RDP.Building;
using RDP.Common.Utils;
using RDP.Networking.Shared.Game.Data;
using RDP.Player;
using RDP.Unit_Controls.Movement;
using UnityEngine;

namespace RDP.Unit_Controls {
	public class UnitSelector : MonoBehaviour {
		//public UnitRole role;
		public Vocation role;
		public List<GameObject> unitList = new List<GameObject>();
		public List<GameObject> unitSelected = new List<GameObject>();
		private PlayerController _player;


		private void Start() {
			_player = GetComponent<PlayerController>();
		}

		public void ClickSelect(GameObject unitToAdd) {
			// Debug.Log(_player + " " + _player.playerDataSo + " " + unitToAdd);
			if (!_player.playerDataSo.UnitRoleExists(unitToAdd.GetComponentInParent<Unit>().role)) return;
			DeselectAll();
			unitSelected.Add(unitToAdd);
			unitToAdd.GetComponentInParent<Unit>().selectedCircle.SetActive(true);
			unitToAdd.GetComponentInParent<UnitMovement>().enabled = true;
		}

		public void ShiftClickSelect(GameObject unitToAdd) {
			if (!unitSelected.Contains(unitToAdd)) {
				if (!_player.playerDataSo.UnitRoleExists(unitToAdd.GetComponentInParent<Unit>().role)) return;
				unitSelected.Add(unitToAdd);
				unitToAdd.GetComponentInParent<Unit>().selectedCircle.SetActive(true);
				unitToAdd.GetComponentInParent<UnitMovement>().enabled = true;
			} else {
				unitToAdd.GetComponentInParent<UnitMovement>().enabled = false;
				unitToAdd.GetComponentInParent<Unit>().selectedCircle.SetActive(false);
				unitSelected.Remove(unitToAdd);
			}
		}

		public void DragSelect(GameObject unitToAdd) {
			if (unitSelected.Contains(unitToAdd)) return;
			if (!_player.playerDataSo.UnitRoleExists(unitToAdd.GetComponentInParent<Unit>().role)) return;
			unitSelected.Add(unitToAdd);
			unitToAdd.GetComponentInParent<Unit>().selectedCircle.SetActive(true);
			unitToAdd.GetComponentInParent<UnitMovement>().enabled = true;
		}

		public void DeselectAll() {
			foreach (GameObject unit in unitSelected.ToArray()) Deselect(unit);
			unitSelected.Clear();
		}

		public void Deselect(GameObject unitToDeselect) {
			unitToDeselect.GetComponent<Unit>().selectedCircle.SetActive(false);
			unitSelected.Remove(unitToDeselect);
		}

		public void SelectAllUnits() {
			foreach (GameObject unit in unitList) ShiftClickSelect(unit);
		} // ReSharper disable Unity.PerformanceAnalysis
		public void SetLocationForAll(Vector3 pos, BuildingInteractor buildingInteractor = null) {
			if (unitSelected.Count <= 0) return; // dont execute if there is no units selected

			List<Transform> usedPoints = new List<Transform>();
			// Debug.Log($"INDEX COUNT {index}");
			List<Vector3> positions = Utils.GetPositionalVectors(pos, 5f, unitSelected.Count);

			if (buildingInteractor) {
				Building.Building building = buildingInteractor.GetComponent<Building.Building>();
				UnitRole roleNeeded = building.GetNeededUnitRoles;
				List<Transform> points = buildingInteractor.GetInsertionPoints;
				List<GameObject> unitsOfRole = GetUnitsOfRole(unitSelected, roleNeeded);


				// Check if A: there is enough space for all units, B: there is enough space for all units of the same role
				int index = unitsOfRole.Count > points.Count ? points.Count : unitsOfRole.Count;

				for (int i = 0; i < index; i++) {
					ClearUnitData(unitSelected[i].transform);
					Unit unitOfRole = unitsOfRole[i].GetComponent<Unit>();
					UnitMovement unitMovementOfRole = unitsOfRole[i].GetComponent<UnitMovement>();
					BuildingUnitData data =
						AssignUnitsToBuilding(unitMovementOfRole, unitOfRole, buildingInteractor, points[i]);
					usedPoints.Add(data.node);
					buildingInteractor.unitBuildingDatas.Add(data);
				}
			} else {
				for (int i = 0; i < unitSelected.Count; i++) {
					ClearUnitData(unitSelected[i].transform);
					
					UnitMovement movement = unitSelected[i].GetComponent<UnitMovement>();
					movement.InteractWithBuilding(false);
					movement.SetLocation(positions[i]);
				}
			}


			if (!buildingInteractor) return;

			foreach (Transform point in usedPoints) {
				buildingInteractor.usedInsertionPoints.Add(point);
				buildingInteractor.availableInsertionPoints.Remove(point);
			}
		}

		private List<GameObject> GetUnitsOfRole(List<GameObject> units, UnitRole role) {
			//Take in a units list and return a list of units that match the role
			return units.Where(unit => unit.GetComponent<Unit>().role == role).ToList();
		}

		private void ClearUnitData(Transform unitToClear) {
			Unit unit = unitToClear.GetComponent<Unit>();
			unitToClear.GetComponent<UnitMovement>().isBuilding = false;
			if (unit.linkedBuildingData != null && unit.linkedBuildingData.IsEmpty()) return;
			// Debug.Log($"{unit.unitData} {unit.unitData != null && unit.unitData.IsEmpty()}");
			BuildingUnitData data = unit.linkedBuildingData;

			// Debug.Log("Removing Unit... SetLocationForAll()");
			data?.linkedBuilding.RemoveUnit(data);
			unitToClear.GetComponent<UnitMovement>().building = null;
			unit.linkedBuildingData = null; // Clear the unitData Class to prevent errors	
		}

		private BuildingUnitData AssignUnitsToBuilding(UnitMovement unitMovement, Unit unit,
		                                               BuildingInteractor buildingInteractor, Transform point) {
			BuildingUnitData data = new BuildingUnitData(unitMovement.gameObject, point, buildingInteractor);
			unitMovement.isBuilding = true;
			unit.linkedBuildingData = data;
			unitMovement.buildingInteractor = buildingInteractor;
			unitMovement.building = buildingInteractor.GetComponent<Building.Building>();
			unitMovement.SetLocation(point.position);
			return data;
		}
		
	}
}