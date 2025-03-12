using System.Collections.Generic;
using RDP.Building;
using RDP.Common.Utils;
using RDP.UI.Buildings;
using UnityEngine;
using UnityEngine.AI; //using MLAPI;
//using MLAPI.Messaging;
//using MLAPI.NetworkVariable;
// using Unity.Netcode;

namespace RDP.Unit_Controls.Movement {
	public class UnitMovement : MonoBehaviour {
		[SerializeField] private AIState state;
		[SerializeField] private NavMeshAgent agent;

		[SerializeField] private LineRenderer line;

		//[SerializeField] private Vector3 destination;
		[SerializeField] private Vector3 destination;
		private UnitSelector _unitSelector;
		public bool isBuilding;
		public BuildingInteractor buildingInteractor;
		public Building.Building building;
		private NavMeshObstacle _obstacle;
		private NavMeshPath _path;
		public Vector3 Position;
		public bool enabled;
		private PathVisualization PathVisualizer => GetComponent<PathVisualization>();
		private Color32 PrimaryColor => GetComponent<Unit>().PrimaryColor;

		private string AIStateString;
		public string GetAIState => AIStateString;
		private bool CallOnce;

		private void Start() {
			CallOnce = false;
			state = AIState.Idle;
			AIStateString = $"{AIState.Idle.ToString()}";

			//check if camera attached, if no use default Camera.main
			agent = GetComponent<NavMeshAgent>();
			_path = new NavMeshPath();
			_obstacle = GetComponent<NavMeshObstacle>();
		}

		private void Update() {
			UpdateVisualization(); // Check 1: Update the visualization of the path
			HandleAI(); // Check 2: Handle the AI
			UpdateState(); // Check 3: Finishing Touches
		}

		private void UpdateState() {
			if (building) return; // if there is no buildings then finally set to idle
			state = AIState.Idle;
			AIStateString = $"{AIState.Idle.ToString()}";
		}

		private void HandleAI() {
			if (agent.pathPending) return;
			if (!(agent.remainingDistance <= agent.stoppingDistance)) return;
			if (agent.hasPath && agent.velocity.sqrMagnitude != 0f) return;
			if (!isBuilding) return;
			GetComponent<Unit>().DeselectSelf();
			InteractWithBuilding();
			AIStateString = $"{building.name}";
			isBuilding = false;
		}

		private void UpdateVisualization() {
			NavMeshPath path = CalculatePath();
			switch (state) {
				case AIState.Moving:
					Vector3[] corners = path.corners;
					for (int i = 0; i < path.corners.Length - 1; i++)
						PathVisualizer.CreateLines(path.corners, PrimaryColor);
					AIStateString = building == null ? $"{AIState.Moving.ToString()}" : $"{building.buildingName}";

					break;
				case AIState.Idle:
					PathVisualizer.ClearLines();
					break;
			}
		}

		public void SetLocation(Vector3 pos) {
			state = AIState.Moving;
			destination = pos;
			agent.SetDestination(pos);
		}

		public Vector3 GetDestination() {
			return destination;
		}

		public void InteractWithBuilding(bool b = true) { // true by default - if false then deselect

			// if (CallOnce) return;
			
			if (b == false) {
				if (buildingInteractor == null) return;
				Transform point = buildingInteractor.GetInsertionPointOccupiedByUnit(gameObject);
				if (!point) return;
				buildingInteractor.Remove(gameObject);
				buildingInteractor.GetComponentInChildren<BuildingInfoUI>().RemoveUnitFromOccupiedIcons(gameObject);
					
				List<Vector3> exitPos = Utils.GetPositionalVectors(point.position, 5, 1); // will always give 1 vector back
				SetLocation(exitPos[0]); // Exit using that vector
				
				CallOnce = false;
			} else {
				
				// CallOnce = true;
				buildingInteractor.Interact(gameObject);
				// buildingInteractor.GetComponentInChildren<BuildingInfoUI>().AssignUnitToInteractableUI(gameObject);

			}
		}
		private NavMeshPath CalculatePath() {
			agent.CalculatePath(destination, _path);
			Vector3[] corners = _path.corners;
			for (int i = 0; i < _path.corners.Length - 1; i++)
				Debug.DrawLine(_path.corners[i], _path.corners[i + 1], Color.red);
			return _path;
		}

		private void ShowPath(Vector3[] positions) {
			line.positionCount = positions.Length;
			for (int i = 0; i < positions.Length; i++) line.SetPosition(i, positions[i]);
		}

		public void SetLeader(Transform leader) { }

		
	}
}