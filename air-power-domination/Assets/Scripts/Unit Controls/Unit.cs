using RDP.Building;
using RDP.Multiplayer;
using RDP.Networking.Shared.Game.Data;
using RDP.Player;
using RDP.Unit_Controls.Movement;
using UnityEngine;

namespace RDP.Unit_Controls {

	public enum UnitState {
		Alive,
		Killed,
	}
	
	public class Unit : TeamReference {
		public UnitSelector unitSelector;

		public UnitState state;
		public UnitData data;
		//public UnitRole role;
		public UnitRole role;
		public Color32 PrimaryColor;
		public GameObject selectedCircle;
		public BuildingUnitData linkedBuildingData;
		[SerializeField] private PlayerController playerReference;

		private UnitMovement unitMovement => GetComponent<UnitMovement>();

		public string AIState => unitMovement.GetAIState;
		private bool executeOnce = false;

		public PlayerController PlayerReference {
			get => playerReference;
			set => playerReference = value;
		}

		private void LateUpdate() {
			if (!executeOnce) PlayerReference.GetTeam().UnitManager.Units.Add(gameObject);
			executeOnce = true;
		}

		private void OnDestroy() {
			unitSelector.unitList.Remove(gameObject);
		}

		public void DeselectSelf() {
			unitSelector.Deselect(gameObject);
		}

		public void KillUnit() {
			unitSelector.unitList.Remove(gameObject);
			GetTeam().UnitManager.Units.Remove(gameObject);
			GetComponent<UnitMovement>().InteractWithBuilding(false);
			state = UnitState.Killed;
		}
	}
}