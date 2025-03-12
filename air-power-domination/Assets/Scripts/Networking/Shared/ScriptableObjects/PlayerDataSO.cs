using System.Collections.Generic;
using System.Linq;
using RDP.Networking.Shared.Game.Data;
using Sirenix.OdinInspector;
using Unity.Netcode;
using UnityEngine;

namespace RDP.Networking.Shared.ScriptableObjects {
	[CreateAssetMenu(fileName = "Data", menuName = "Prototype 1/Player Data", order = 1)]

	// TODO: We can add more in the future to suit the needs.
	public class PlayerDataSO : ScriptableObject {
		[Header("Unit System")] public Vocation CharacterClass;
		public UnitDataList unitDataList;

		public string DisplayedName;

		[Required("PlayerController Prefab is a must for the player to load in properly!!")]
		public NetworkObject playerPrefab;

		//Lobby InfoBox
		public string displayedName;

		//Class Logo
		//public Sprite classBannerLit;
		//public Sprite classBannerUnlit;

		//Enemy
		public bool isNpc;

/*        //Health for Units
        public IntVariable baseHp;*/

		// Use this function to determine what this player can control
		public List<UnitRole> GetRoles() {
			return unitDataList.unitsToSpawn.Select(unit => unit.role).ToList();
		}

		public bool UnitRoleExists(UnitRole role) {
			return GetRoles().Contains(role);
		}

		public bool UnitRoleExists(List<UnitRole> role) {
			foreach (UnitRole r in role)
				if (GetRoles().Contains(r))
					return true;
			return false;
		}

		public Vocation GetPlayerRoles() {
			return CharacterClass;
		}

		public bool HasUnitsToSpawn() {
			if (unitDataList)
				return unitDataList.unitsToSpawn.Count > 0;
			else
				return false;
		}
	}
}