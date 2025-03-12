using System.Collections.Generic;
using RDP.Common.Utils;
using RDP.Grid_System;
using RDP.Multiplayer;
using RDP.Networking.Shared.Game.Data;
using RDP.Networking.Shared.ScriptableObjects;
using RDP.Tasks;
using RDP.Tasks.TaskTypes;
using RDP.Unit_Controls;
using UnityEngine;
using Debug = UnityEngine.Debug;
using AudioType = RDP.Common.Audio.Enums.AudioType;

namespace RDP.Building {
	public class Building : MonoBehaviour {
		public const float MAXProgress = 100;

		public string buildingName;
		public BuildingManager buildingManager;
		public BuildingState state;

		public bool autoBuild;
		public bool isConcealed;
		[SerializeField] private bool canRepair;

		[SerializeField] private BuildingType _type;
		public BuildingType Type => _type;
		[SerializeField] private List<GameObject> unitsCurrentlyInteracting;
		[SerializeField] private Task currentTask;
		[SerializeField] private List<Node> occupiedNodes = new List<Node>();

		[SerializeField] private GameObject unBuiltBuilding;
		[SerializeField] private GameObject builtBuilding;
		[SerializeField] private GameObject destroyedBuilding;


		[SerializeField] private TaskData buildingTaskData;
		[SerializeField] private TaskData RecursiveTaskData;
		[SerializeField] private TaskData RepairTaskData;
		[SerializeField] private bool executeOnce;

		public delegate void ExecuteTask();

		private ExecuteTask _executeTask;


		public Task CurrentTask => currentTask;
		public ExecuteTask executeTask => _executeTask;
		public List<GameObject> UnitsCurrentlyInteracting => unitsCurrentlyInteracting;

		private void Awake() {
			SetBuildingState(BuildingState.Unbuit);
			_executeTask += Built;
		}


		private void Update() {
			ProcessTask();
			UpdateBuildingState();
		}

		private void DisableAllBuildingStates() {
			unBuiltBuilding.SetActive(false);
			builtBuilding.SetActive(false);
			destroyedBuilding.SetActive(false);
		}


		private void ProcessTask() {
			if (currentTask == null) return;

			if (currentTask.State == TaskState.Complete) {
				// If its labeled as recursive it will restart the task
				if (currentTask.IsRecursive) {
					currentTask.Restart();
					currentTask.CurrentProgress = 0;
				}

				ResolveCurrentTask();
				return;
			}

			int unitCount = unitsCurrentlyInteracting.FindAll(x => x.GetComponent<Unit>().role == currentTask.UnitRole && x != null).Count;
			float multiplier = 100 / currentTask.ProcessingRate * Time.deltaTime * unitCount;
			currentTask.SetCurrentProgress(multiplier);
		}

		private void OnDestroy() {
			//Clean up the delegate
			_executeTask = null;
		}

		public void Setup(List<Vector2Int> nodeData, BuildingManager buildingManager,
		                  PlacedObjectTypeSO placedObjectTypeSO, bool autoBuild, bool autoConceal, bool debug = false) {
			isConcealed = autoConceal;
			this.autoBuild = debug ? autoBuild : buildingManager.allowInstantBuilding;

			//Record its own Grid Data Positions.
			foreach (Vector2Int node in nodeData) {
				Debug.Log($"{node.x}, {node.y}");
				Grid<Node> grid = buildingManager.GetTeam().GridSystem.Grid;
				Node nodeObject = grid.GetGridObject(node.x, node.y);
				nodeObject.Concealed = isConcealed;
				nodeObject.Destroyed = false;
				// Debug.Log($"{nodeObject.Concealed},  Grid Position: {nodeObject.X}, {nodeObject.Y}");
				occupiedNodes.Add(nodeObject);
			}

			_type = placedObjectTypeSO.type;
			// Next Add itself into the buildingManager's Dictonary
			buildingManager.AddBuildingToList(this, placedObjectTypeSO.nameString);
			
			this.buildingManager = buildingManager;

			// If AutoBuild is True, Then dont need to create new task, just update state and end of story
			if (this.autoBuild || _type == BuildingType.CommandCenter) {
				Built();
				return;
			}
			// Next, Call onto TeamManager to create a new Task for the Engineer Role Player.

			SendTask(Utils.CreateBuildingTask(buildingTaskData, buildingManager, buildingName));
		}

		private void SendTask(Task task) {
			Team team = buildingManager.GetTeam();
			Vocation role = team.GetPlayerRoles(task.UnitRole);

			// team.DelegateTask(role, task);

			if (task.State == TaskState.Complete) return;
			currentTask = task;
			buildingManager.UpdateTaskList();
		}

		public UnitRole GetNeededUnitRoles => currentTask.UnitRole;
		
		private void KillAnyUnitInteracting() {
			foreach (GameObject unit in unitsCurrentlyInteracting) {
				unit.GetComponent<Unit>().KillUnit();
			}
		}

		private void Built() {
			foreach (Node node in occupiedNodes) {
				node.BuiltState = true;
				node.Concealed = isConcealed;
				// Debug.Log($"Node Data: {node.Concealed}, {node.BuiltState}, \n Grid Position: {node.X}, {node.Y}");
			}

			SetBuildingState(BuildingState.Built);
			if (buildingManager.GetTeam().TeamID == 0) // TODO Remove this IF Statement when catering for multiplayer!
				buildingManager.GetTeam().SFXManager.Play(AudioType.SfxBuildComplete);
		}


		public void SetBuildingState(BuildingState state) {
			_executeTask = null;
			executeOnce = false;
			this.state = state;
			UpdateBuildingState();
		}

		public void UpdateUnitsCurrentlyInteracting(List<GameObject> units) {
			unitsCurrentlyInteracting = units;
		}

		public void UpdateConcealedState(bool state) {
			foreach (Node node in occupiedNodes) node.Concealed = state;
		}

		private void UpdateBuildingState() {
			// TODO : Update the Building State
			switch (state) {
				case BuildingState.Unbuit:
					UnbuiltState();
					break;
				case BuildingState.Built:
					UpdateBuiltState();
					break;
				case BuildingState.Destroyed:
					Repair();
					break;
			}
		}

		private void UnbuiltState() {
			DisableAllBuildingStates();
			unBuiltBuilding.SetActive(true);
		}

		private void UpdateBuiltState() {
			DisableAllBuildingStates();
			builtBuilding.SetActive(true);


			if (executeOnce) return;
			if (RecursiveTaskData == null) return;
			executeOnce = true;

			ITaskExecutableRecursive taskExecutableRecursive = GetComponentInChildren<ITaskExecutableRecursive>();
			if (taskExecutableRecursive == null) return;

			_executeTask += taskExecutableRecursive.Execute;
			SendTask(Utils.CreateTask(RecursiveTaskData, buildingManager, buildingName));
		}

		private void Repair() {
			DisableAllBuildingStates();
			destroyedBuilding.SetActive(true);

			if (executeOnce) return;

			if (!canRepair) return;
			if (RepairTaskData == null) return;

			executeOnce = true;

			KillAnyUnitInteracting();
			foreach (Node node in occupiedNodes) node.Destroyed = true;

			ITaskExecutableRepair interfaceRepair = GetComponentInChildren<ITaskExecutableRepair>();
			if (interfaceRepair != null) { // check if there is a Repair Interface task located 
				// if so execute it's execute function and return
				interfaceRepair.Execute();
				return;
			}

			_executeTask += OnRepairComplete;
			SendTask(Utils.CreateBuildingTask(RepairTaskData, buildingManager, buildingName));
		}

		private void OnRepairComplete() {
			SetBuildingState(BuildingState.Built);
			foreach (Node node in occupiedNodes) node.Destroyed = false;
		}


		private void ResolveCurrentTask() {
			if (executeTask == null) { // Dont process task if no function is subscribed to the delegate
				Debug.Log($"No Delegates found in executeTask Delegate Returning...");
				return;
			}
			// foreach( Delegate del in _executeTask.GetInvocationList() )
			// {
			//     // Debug.Log( $"Delegate 2 - Method: {del.Method} Target {del.Target}" );
			// }

			// currentTask.CompleteTask();
			_executeTask();
			// _executeTask = null;
		}

		public void TriggerAnimation(string animationTrigger) {
			// Execute any animations here
		}
	}
}