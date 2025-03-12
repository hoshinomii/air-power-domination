using RDP.Multiplayer;
using RDP.Networking.Shared.Game.Data;
using RDP.UI.CommanderTools;
using UnityEngine;

namespace RDP.Grid_System {
	public class GridInputManager : MonoBehaviour, ITeamReference {
		public GridSystem gridSystem;
		private KeyCode _previousPressedKey;


		private Team _teamReference;
		private Player.PlayerManager _linkedPlayerManager;

		public Player.PlayerManager LinkedPlayerManager {
			get => _linkedPlayerManager;
			set => _linkedPlayerManager = value;
		}

		private void Start() { }

		// Update is called once per frame
		// TODO: [RDP-6] [For Haoting] Disable Inputs when commander tools UI is active
		private void Update() {
			if (LinkedPlayerManager.UI.GetComponentInChildren<CommandTool>().Enabled) return;

			if (Input.GetMouseButtonDown(0) && gridSystem.placedObjectTypeSO != null) gridSystem.Build();
			if (Input.GetKeyUp(KeyCode.C) || Input.GetKeyUp(KeyCode.Escape)) {
				if (gridSystem.placedObjectTypeSO == null) return;
				LinkedPlayerManager.UI.UISelection.ResetAllNodes(Vocation.Commander);
				gridSystem.placedObjectTypeSO = null;
				gridSystem.RefreshSelectedObjectType();
			}

			if (Input.GetKeyDown(KeyCode.R)) gridSystem.dir = PlacedObjectTypeSO.GetNextDir(gridSystem.dir);

			// if (Input.GetKeyDown(KeyCode.Alpha1))
			// {
			//     if (_previousPressedKey == KeyCode.Alpha1)
			//     {
			//         gridSystem.placedObjectTypeSO = null;
			//         _previousPressedKey = KeyCode.None;
			//     }
			//     else
			//     {
			//         gridSystem.placedObjectTypeSO = gridSystem.placedObjectTypeSOList[0];
			//         _previousPressedKey = KeyCode.Alpha1;
			//     }
			//
			//     gridSystem.RefreshSelectedObjectType();
			// }
			//
			// if (Input.GetKeyDown(KeyCode.Alpha2))
			// {
			//     if (_previousPressedKey == KeyCode.Alpha2)
			//     {
			//         gridSystem.placedObjectTypeSO = null;
			//         _previousPressedKey = KeyCode.None;
			//     }
			//     else
			//     {
			//         gridSystem.placedObjectTypeSO = gridSystem.placedObjectTypeSOList[1];
			//         _previousPressedKey = KeyCode.Alpha2;
			//     }
			//
			//     gridSystem.RefreshSelectedObjectType();
			// }
			//
			// if (Input.GetKeyDown(KeyCode.Alpha3))
			// {
			//     if (_previousPressedKey == KeyCode.Alpha3)
			//     {
			//         gridSystem.placedObjectTypeSO = null;
			//         _previousPressedKey = KeyCode.None;
			//     }
			//     else
			//     {
			//         gridSystem.placedObjectTypeSO = gridSystem.placedObjectTypeSOList[2];
			//         _previousPressedKey = KeyCode.Alpha3;
			//     }
			//
			//     gridSystem.RefreshSelectedObjectType();
			// }
			//
			// if (Input.GetKeyDown(KeyCode.Alpha4))
			// {
			//     if (_previousPressedKey == KeyCode.Alpha4)
			//     {
			//         gridSystem.placedObjectTypeSO = null;
			//         _previousPressedKey = KeyCode.None;
			//     }
			//     else
			//     {
			//         gridSystem.placedObjectTypeSO = gridSystem.placedObjectTypeSOList[3];
			//         _previousPressedKey = KeyCode.Alpha3;
			//     }
			//
			//     gridSystem.RefreshSelectedObjectType();
			// }
			//
			// if (Input.GetKeyDown(KeyCode.Alpha5))
			// {
			//     if (_previousPressedKey == KeyCode.Alpha5)
			//     {
			//         gridSystem.placedObjectTypeSO = null;
			//         _previousPressedKey = KeyCode.None;
			//     }
			//     else
			//     {
			//         gridSystem.placedObjectTypeSO = gridSystem.placedObjectTypeSOList[4];
			//         _previousPressedKey = KeyCode.Alpha3;
			//     }
			//
			//     gridSystem.RefreshSelectedObjectType();
			// }
		}

		public void SetTeam(Team team) {
			_teamReference = team;
		}

		public Team GetTeam() {
			return _teamReference;
		}

		public void SetGridInputInstance(GridSystem gridSystem) {
			this.gridSystem = gridSystem;
		}
	}
}