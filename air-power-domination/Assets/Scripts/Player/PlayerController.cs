using RDP.Grid_System;
using RDP.Multiplayer;
using RDP.Networking.Shared.Game.Data;
using RDP.Networking.Shared.ScriptableObjects;
using RDP.UI;
using RDP.UI.HUD;
using RDP.UI.UISelector;
using RDP.Unit_Controls;
using UnityEngine;
using UnityEngine.Serialization;

namespace RDP.Player {
	public class PlayerController : MonoBehaviour, ITeamReference {
		[Header("Prefabs")] public GameObject uiPrefab;

		[FormerlySerializedAs("CameraPrefab")] public GameObject cameraPrefab;
		public GameObject gridInputManagerPrefab;

		[Tooltip("This is a must or else the entire Player and Units System will break!")]
		public PlayerDataSO playerDataSo;

		[Header("References")] [SerializeField]
		private UIManager uiManager;

		[SerializeField] private GridInputManager gridInputManager;
		[SerializeField] private Mouse3D mouse3D;
		[SerializeField] private Camera camera;

		[FormerlySerializedAs("InsertionPoint")]
		public Transform insertionPoint;

		private Team _teamReference;
		private UIManager _uiManager;
		private PlayerManager _playerManager;


		private void Awake() {
			if (!playerDataSo) Debug.LogError("PlayerData Scriptable Object Not Found!!!");
		}

		private void Start() {
			Init();
		}

		private void Init() {
			//Spawn its own camera and UI
			uiManager = Instantiate(uiPrefab, Vector3.zero, Quaternion.identity).GetComponent<UIManager>();
			uiManager.SetTeam(_teamReference);
			uiManager.Setup(playerDataSo.CharacterClass);
			uiManager.Player = this;
			PlayerManager.UI = uiManager;
			UISelection selector = uiManager.UISelection;
			//First UISelection Configuration Checking if its commander.
			selector.Configure(this);

			camera = Instantiate(cameraPrefab, Vector3.zero + new Vector3(0, 100, -170), Quaternion.identity)
				.GetComponentInChildren<Camera>();
			GetComponent<PlayerMouse3D>().Camera = camera;

			if (playerDataSo.HasUnitsToSpawn()) {
				UnitSelector unitSelector = GetComponent<UnitSelector>();
				UnitClick unitClick = GetComponent<UnitClick>();
				UnitDrag unitDrag = GetComponent<UnitDrag>();

				// If player data have units spawn them.
				UnitInit(unitSelector, unitClick, unitDrag);
				SpawnUnits(unitSelector);

				selector.Configure(this, unitSelector.unitList, playerDataSo.GetRoles());
			}

			if (gridInputManagerPrefab == null) return;
			GridInit();
			if (!playerDataSo.HasUnitsToSpawn()) //Check if its commander and does not have units to work with
				DisableUnitInit();
		}

		private void UnitInit(UnitSelector unitSelector, UnitClick unitClick, UnitDrag unitDrag) {
			unitSelector.role = playerDataSo.CharacterClass;
			unitClick.camera = camera;
			unitDrag.camera = camera;
			unitDrag.boxVisual = uiManager.boxVisual;
		}

		private void DisableUnitInit() {
			GetComponent<UnitSelector>().enabled = false;
			GetComponent<UnitClick>().enabled = false;
			GetComponent<UnitDrag>().enabled = false;
		}

		private void GridInit() {
			GameObject gridManager = Instantiate(gridInputManagerPrefab, Vector3.zero, Quaternion.identity);
			gridInputManager = gridManager.GetComponent<GridInputManager>();
			mouse3D = gridManager.GetComponent<Mouse3D>();
			GetTeam().GridSystem.Mouse3D = mouse3D;
			mouse3D.SetCamera(camera);
			gridInputManager.SetGridInputInstance(GetTeam().GridSystem);
			gridInputManager.LinkedPlayerManager = _playerManager;
		}

		private void SpawnUnits(UnitSelector unitSelector) {
			// TODO: Spawn Units Integrate with networking
			foreach (UnitData unit in playerDataSo.unitDataList.unitsToSpawn) {
				// Instantiate the unit at the insertion point.
				Vector3 spawnPoint = unit.insertionPoint;
				for (int i = 0; i < unit.amount; i++) {
					GameObject gm = Instantiate(unit.unitPrefab, spawnPoint + new Vector3(1.5f * i, 0, 0),
						Quaternion.identity);
					Unit unitObj = gm.GetComponent<Unit>();
					unitObj.state = UnitState.Alive;
					unitObj.SetTeam(GetTeam(), GetTeam().TeamID);
					unitObj.PrimaryColor = unit.primaryColor;
					unitObj.PlayerReference = this;
					unitObj.unitSelector = GetComponent<UnitSelector>();
					unitSelector.unitList.Add(gm);
				}
			}
		}

		public void SetTeam(Team team) {
			_teamReference = team;
		}

		public Team GetTeam() {
			return _teamReference;
		}

		public UIManager UIManager => uiManager;

		public PlayerManager PlayerManager {
			get => _playerManager;
			set => _playerManager = value;
		}
	}
}