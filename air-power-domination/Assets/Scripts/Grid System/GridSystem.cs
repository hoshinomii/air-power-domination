using System;
using System.Collections.Generic;
using RDP.Building;
using RDP.Common.Utils;
using RDP.Multiplayer;
using UnityEngine;
using UnityEngine.Serialization;

namespace RDP.Grid_System {
	public class GridSystem : MonoBehaviour, ITeamReference {
		public int gridWidth;
		public int gridHeight;
		public float cellSize;
		[FormerlySerializedAs("OriginPoint")] public Transform originPoint;
		public List<PlacedObjectTypeSO> placedObjectTypeSOList = null;
		public PlacedObjectTypeSO placedObjectTypeSO;
		public PlacedObjectTypeSO.Dir dir;

		private BuildingManager _buildingManager;
		private Grid<Node> _grid;

		[Header("Visualization")] [SerializeField]
		private GameObject visualizationParent;

		[SerializeField] private GameObject visualizationNodePrefab;

		private Team _teamReference;
		
		public Mouse3D Mouse3D { get; set; }

		private void Awake() {
			// Instance = this;

			_grid = new Grid<Node>(gridWidth, gridHeight, cellSize, originPoint.position,
				(Grid<Node> g, int x, int y) => new Node(g, x, y));
			placedObjectTypeSO = null; // placedObjectTypeSOList[0];

			ConfigureVisualization();
		}

		private void ConfigureVisualization() {
			Node[,] nodes = _grid.GetNodeArray();
			for (int i = 0; i < nodes.GetLength(0); i++) {
				for (int l = 0; l < nodes.GetLength(1); l++) {
					GameObject nodePrefab = Instantiate(visualizationNodePrefab, visualizationParent.transform);
					nodePrefab.GetComponent<VisualizedNode>()
						.Configure(nodes[i, l], _grid.GetWorldPosition(i, l), _grid.GetCellSize());
				}
			}
		}

		private void Start() {
			_buildingManager = GetTeam().BuildingManager;
		}

		private void LateUpdate() {
			Visualize(placedObjectTypeSO);
		}

		private void Visualize(bool b) {
			visualizationParent.SetActive(b);
		}

		private void Update() {
			// Debug.Log(_grid);
		}

		public void SetTeam(Team team) {
			_teamReference = team;
		}

		public Team GetTeam() {
			return _teamReference;
		}

		public event EventHandler OnSelectedChanged;
		public event EventHandler OnObjectPlaced;

		public void Build() {
			Vector3 mousePosition = Mouse3D.GetMouseWorldPosition();
			_grid.GetXZ(mousePosition, out int x, out int z);

			Vector2Int placedObjectOrigin = new Vector2Int(x, z);
			placedObjectOrigin = _grid.ValidateGridPosition(placedObjectOrigin);
			// Debug.Log("Placed Origin " + placedObjectOrigin);

			// Check whether can build
			List<Vector2Int> gridPositionList = placedObjectTypeSO.GetGridPositionList(placedObjectOrigin, dir);
			bool canBuild = true;

			foreach (Vector2Int gridPosition in gridPositionList) {
				if (_grid.GetGridObject(gridPosition.x, gridPosition.y) == null) {
					Debug.Log($"Illegal position {gridPosition.x} {gridPosition.y}");
					canBuild = false;
					break;
				}

				if (!_grid.GetGridObject(gridPosition.x, gridPosition.y).CanBuild()) {
					canBuild = false;
					break;
				}
			}

			if (canBuild) {
				Vector2Int rotationOffset = placedObjectTypeSO.GetRotationOffset(dir);
				// Debug.Log("rotationOffset: " + rotationOffset + " " + placedObjectOrigin.x + " " + placedObjectOrigin.y + " " + grid.GetCellSize());
				Vector3 placedObjectWorldPosition = _grid.GetWorldPosition(placedObjectOrigin.x, placedObjectOrigin.y) +
				                                    new Vector3(rotationOffset.x, 0, rotationOffset.y) *
				                                    _grid.GetCellSize();
				Debug.Log(placedObjectWorldPosition);
				PlacedObjectDone placedObject = PlacedObjectDone.Create(placedObjectWorldPosition, placedObjectOrigin,
					dir,
					placedObjectTypeSO, GetTeam().BuildingManager, false);

				if (!placedObject) {
					Utils.CreateWorldTextPopup("Building Not Permitted!", mousePosition);
					return;
				}
				
				foreach (Vector2Int gridPosition in gridPositionList)
					_grid.GetGridObject(gridPosition.x, gridPosition.y).SetPlacedObject(placedObject);

				OnObjectPlaced?.Invoke(this, EventArgs.Empty);

				//DeselectObjectType();
			} else {
				// Cannot build here
				Utils.CreateWorldTextPopup("Building Not Permitted!", mousePosition);
			}
		}

		//Build Method, for placing objects through code instead of mouse position and will automatically build and set concealment to false (for testing) 
		public void Build(Vector3 pos, bool autoBuild, bool autoConceal) {
			// Debug.Log(_grid);
			_grid.GetXZ(pos, out int x, out int z);
			Vector2Int placedObjectOrigin = new Vector2Int(x, z);
			placedObjectOrigin = _grid.ValidateGridPosition(placedObjectOrigin);
			// Debug.Log("Placed Origin " + placedObjectOrigin);

			// Check whether can build
			List<Vector2Int> gridPositionList = placedObjectTypeSO.GetGridPositionList(placedObjectOrigin, dir);
			bool canBuild = true;

			foreach (Vector2Int gridPosition in gridPositionList)
				if (!_grid.GetGridObject(gridPosition.x, gridPosition.y).CanBuild()) {
					canBuild = false;
					break;
				}

			if (canBuild) {
				Vector2Int rotationOffset = placedObjectTypeSO.GetRotationOffset(dir);
				// Debug.Log("rotationOffset: " + rotationOffset + " " + placedObjectOrigin.x + " " + placedObjectOrigin.y + " " + grid.GetCellSize());
				Vector3 placedObjectWorldPosition = _grid.GetWorldPosition(placedObjectOrigin.x, placedObjectOrigin.y) +
				                                    new Vector3(rotationOffset.x, 0, rotationOffset.y) *
				                                    _grid.GetCellSize();
				Debug.Log(placedObjectWorldPosition);
				PlacedObjectDone placedObject = PlacedObjectDone.Create(placedObjectWorldPosition, placedObjectOrigin,
					dir,
					placedObjectTypeSO, GetTeam().BuildingManager, true, autoBuild, autoConceal);
				foreach (Vector2Int gridPosition in gridPositionList)
					_grid.GetGridObject(gridPosition.x, gridPosition.y).SetPlacedObject(placedObject);
				OnObjectPlaced?.Invoke(this, EventArgs.Empty);

				//DeselectObjectType();
			} else {
				// Cannot build here
				Utils.CreateWorldTextPopup("Building Not Permitted!", pos);
			}
		}


		private void DeselectObjectType() {
			placedObjectTypeSO = null;
			RefreshSelectedObjectType();
		}

		public void RefreshSelectedObjectType() {
			OnSelectedChanged?.Invoke(this, EventArgs.Empty);
		}

		public Vector2Int GetGridPosition(Vector3 worldPosition) {
			_grid.GetXZ(worldPosition, out int x, out int z);
			return new Vector2Int(x, z);
		}

		public Vector3 GetMouseWorldSnappedPosition() {
			
			if(!Mouse3D) { 
				Debug.LogWarning($"Mouse3D is not located in the GridSystem reference from {gameObject.name}");
				return Vector3.zero;
			}
			
			
			Vector3 mousePosition = Mouse3D.GetMouseWorldPosition();
			_grid.GetXZ(mousePosition, out int x, out int z);
			//Debug.Log("X = " + x + " | " + "Y = " + z);

			if (placedObjectTypeSO != null) {
				Vector2Int rotationOffset = placedObjectTypeSO.GetRotationOffset(dir);
				Vector3 placedObjectWorldPosition = _grid.GetWorldPosition(x, z) +
				                                    new Vector3(rotationOffset.x, 0, rotationOffset.y) *
				                                    _grid.GetCellSize();
				return placedObjectWorldPosition;
			} else {
				return mousePosition;
			}
		}

		public Quaternion GetPlacedObjectRotation() {
			if (placedObjectTypeSO != null)
				return Quaternion.Euler(0, placedObjectTypeSO.GetRotationAngle(dir), 0);
			else
				return Quaternion.identity;
		}

		public PlacedObjectTypeSO GetPlacedObjectTypeSO() {
			return placedObjectTypeSO;
		}

		public Grid<Node> Grid => _grid;
	}
}