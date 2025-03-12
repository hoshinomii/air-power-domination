using System.Collections.Generic;
using RDP.Building;
using UnityEngine;

namespace RDP.Grid_System {
	public class PlacedObjectDone : MonoBehaviour {
		private PlacedObjectTypeSO.Dir _dir;
		private Vector2Int _origin;

		private PlacedObjectTypeSO placedObjectTypeSO;

		public static PlacedObjectDone Create(Vector3 worldPosition, Vector2Int origin, PlacedObjectTypeSO.Dir dir,
		                                      PlacedObjectTypeSO placedObjectTypeSO, BuildingManager buildingManager,
		                                      bool debug, bool autoBuild = false, bool autoConceal = false) {

			if (!buildingManager.IsWithinLimit(placedObjectTypeSO.type)) {
				Debug.Log("PlacedObjectDone: Create: Too many buildings on the map");
				return null;
			}
			
			PlacedObjectDone placedObjectDone = new GameObject("PlacedObjectDone").AddComponent<PlacedObjectDone>();
			Transform placedObjectTransform = Instantiate(placedObjectTypeSO.prefab, worldPosition,
				Quaternion.Euler(0, placedObjectTypeSO.GetRotationAngle(dir), 0));

			PlacedObjectDone placedObject = placedObjectTransform.GetComponent<PlacedObjectDone>();
			placedObjectTransform.name = placedObjectTypeSO.name + " " + placedObjectTransform.GetInstanceID();

			placedObject.Setup(placedObjectTypeSO, origin, dir, buildingManager, autoBuild, autoConceal, debug);

			return placedObject;
		}

		private void Setup(PlacedObjectTypeSO placedObjectTypeSO, Vector2Int origin, PlacedObjectTypeSO.Dir dir,
		                   BuildingManager buildingManager,
		                   bool autoBuild, bool autoConceal, bool debug) {
			this.placedObjectTypeSO = placedObjectTypeSO;
			_origin = origin;
			_dir = dir;

			// Checking if building reference is in this gameObject
			Building.Building building = gameObject.GetComponent<Building.Building>();
			if (building)
				building.Setup(GetGridPositionList(), buildingManager, placedObjectTypeSO, autoBuild, autoConceal);
		}

		public List<Vector2Int> GetGridPositionList() {
			return placedObjectTypeSO.GetGridPositionList(_origin, _dir);
		}

		public void DestroySelf() {
			Destroy(gameObject);
		}

		public override string ToString() {
			return placedObjectTypeSO.nameString;
		}
	}
}