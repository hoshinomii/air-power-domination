using System.Collections.Generic;
using RDP.Multiplayer;
using RDP.Tasks;
using UnityEngine;

// This is a global class, so there should only be one and not for each Player.
namespace RDP.Building {
	public class BuildingManager : MonoBehaviour, ITeamReference {
		[SerializeField] private List<GameObject> buildings = new List<GameObject>();
		[SerializeField] private List<Task> allBuildingTaskList = new List<Task>();
		[SerializeField] private int planeCount;
		[SerializeField] private int maxPlanes;

		public bool allowInstantBuilding;
		
		// Command Center limit, Cap to only 1
		public const int CommandCenterLimit = 1;
		
		[Space] [Header("Preparation Phase Limitations")]
		public int hangerLimit;
		public int runwayLimit;
		public int samSiteLimit;
		public int watchTowerLimit;
		

		public int PlaneCount {
			get => planeCount;
			set => planeCount = value;
		}
		
		public bool BasicBuildingsMet {
			get {
				bool commandCenterMet = GetBuildings(BuildingType.CommandCenter).Count > 0;
				bool aircraftHangerMet = GetBuildings(BuildingType.AircraftHanger).Count > 1;
				bool aircraftRunway = GetBuildings(BuildingType.AircraftRunway).Count > 1;
				bool samSite = GetBuildings(BuildingType.SamSite).Count > 1;
				bool watchTower = GetBuildings(BuildingType.WatchTower).Count > 1;
				return aircraftRunway && commandCenterMet && aircraftHangerMet && samSite && watchTower;
			}
		}

		public List<Task> GetAllBuildingTaskList => allBuildingTaskList;

		public bool HasPlanes => PlaneCount > 0;

		private bool CanAirstrike {
			get {
				// Checks if there are aircraft runways and if the runway is built
				List<GameObject> building = GetBuildings(BuildingType.AircraftRunway);
				return building.Count > 0 &&
				       buildings.Find(x => x.GetComponent<Building>().state == BuildingState.Built);
			}
		}

		public bool IsWithinLimit(BuildingType type) {
			
			// First pull the building type
			// BuildingType type = building.GetComponent<Building>().Type;
			if (LevelManager.Instance.State == LevelState.GameOver) return false; //Cant Build then Game Over is enforced.
			
			// check if its a command center, if so enforce limits
			if (type == BuildingType.CommandCenter) {
				return buildings.FindAll(x => x.GetComponent<Building>().Type == BuildingType.CommandCenter).Count <
				       CommandCenterLimit;
			}
			
			
			// Then based on the type look for the limit if LevelManager is in Preparation enforce limits on the building
			if (LevelManager.Instance.State == LevelState.Preparation) {
				return type switch {
					BuildingType.AircraftHanger => buildings
						.FindAll(x => x.GetComponent<Building>().Type == BuildingType.AircraftHanger)
						.Count < hangerLimit,
					BuildingType.AircraftRunway => buildings
						.FindAll(x => x.GetComponent<Building>().Type == BuildingType.AircraftRunway)
						.Count < runwayLimit,
					BuildingType.SamSite => buildings
						.FindAll(x => x.GetComponent<Building>().Type == BuildingType.SamSite)
						.Count < samSiteLimit,
					BuildingType.WatchTower => buildings
						.FindAll(x => x.GetComponent<Building>().Type == BuildingType.WatchTower)
						.Count < watchTowerLimit,
					_ => true
				};
			}


			// if somehow u manage to make it here just return true (Unlimited Build)
			return true;
		}

		public bool AirstrikePossible(int planeCount) {
			return CanAirstrike && HasPlanesNeeded(planeCount);
		}

		private bool HasPlanesNeeded(int planesRequired) {
			if (planesRequired <= 0) return false;
			return PlaneCount >= planesRequired;
		}

		private Team _teamReference;


		private void Awake() { }

		// Call this to update the task list every time the building script adds or updates a new task.
		public void UpdateTaskList() {
			foreach (GameObject building in buildings)
				allBuildingTaskList.Add(building.GetComponent<Building>().CurrentTask);
		}

		public void SetTeam(Team team) {
			_teamReference = team;
		}

		public Team GetTeam() {
			return _teamReference;
		}

		//Make a function to return the correct building in buildings based on the key
		// Get building based on type and returns a gameObject
		public GameObject GetBuilding(BuildingType type) {
			return buildings.Find(x => x.GetComponent<Building>().Type == type);
		}

		// Get Building based on type and return it as a list
		public List<GameObject> GetBuildings(BuildingType type) {
			return buildings.FindAll(x => x.GetComponent<Building>().Type == type);
		}

		// This will be used to determine weather or not the player can initiate an airstrike.
		public void AddBuildingToList(Building building, string buildingName) {
			buildings.Add(building.gameObject);
			Debug.Log("Added Building Name: " + name + " and GameObject " + building + " To Dictonary");

			List<GameObject> gameObjects = buildings.FindAll(x =>
				x.GetComponent<Building>().Type == building.GetComponent<Building>().Type);

			Debug.Log($"Found more than one building of type {building.GetComponent<Building>().Type}");
			// now that we know that there is more than 1 of the same building, return the index of the newly added building
			
			Debug.Log($"Found more than one building of type {building.Type}");
			
			// now that we know that there is more than 1 of the same building, add the index prefix to the object's name
			building.buildingName = $"{buildingName} {(gameObjects.IndexOf(building.gameObject) + 1).ToString()}";

		}

		// Simultaneous Animation Triggering
		public void TriggerAnimationsALL(BuildingType type, string animationTrigger) {
			List<GameObject> gameObjects = GetBuildings(type);
			foreach (GameObject building in gameObjects)
				building.GetComponent<Building>().TriggerAnimation(animationTrigger);
		}

		public void AddPlanes(int planesToAddUponCompletion) {
			if (maxPlanes > planeCount) {
				planeCount += planesToAddUponCompletion;
				Debug.Log($"Adding Planes {planesToAddUponCompletion}, Current Plane Count: {planeCount}");
			}
		}

		public void RemovePlanes(int resourceCount) {
			if (planeCount > 0) planeCount -= resourceCount;
		}

		public void Refund(int refundAmount) {
			AddPlanes(refundAmount);
		}
	}
}