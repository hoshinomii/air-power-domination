using System.Collections.Generic;
using System.Linq;
using RDP.Multiplayer;
using RDP.Networking.Shared.Game.Data;
using UnityEngine;

namespace RDP.Unit_Controls {
	public class UnitManager : MonoBehaviour, ITeamReference {
		//This script's main goal is to regulate the entire team's Units
		private Team team;
		[SerializeField] private List<GameObject> units = new List<GameObject>();

		public List<GameObject> Units {
			get => units;
			set => units = value;
		}

		public bool HasGuards => GetUnits(UnitRole.Guard).Count > 0;
		public bool CanSendScoutingTeam => HasGuards;

		public Team GetTeam() {
			return team;
		}

		public void SetTeam(Team team) {
			this.team = team;
		}

		// Get Units, if get state is true, get ONLY the units that the gameObject is in the given state
		public List<GameObject> GetUnits(UnitRole role, bool getState = false, bool state = false) {
			List<GameObject> gameObjects = new List<GameObject>();
			foreach (GameObject unit in this.units.Where(unit => unit.GetComponent<Unit>().role == role).ToArray()) {
				if (getState) {
					if (unit.activeSelf == state) {
						gameObjects.Add(unit);
					}
				} else {
					gameObjects.Add(unit);
				}
			}

			return gameObjects;
		}
		
		public void KillRandomUnitFromRole(UnitRole role) {
			// find all the guards from Units and put them inside a list
			List<GameObject> units = Units.FindAll(x =>
				x.GetComponent<Unit>().role == role && x.GetComponent<Unit>().state != UnitState.Killed);

			if (units.Count <= 0) {
				Debug.LogWarning($"No {role.ToString()} left to kill!");
				return;
			}
			
			//Then pick a random gameObject from guardUnits
			Unit selectedGuard = units[Random.Range(0, units.Count)].GetComponent<Unit>();
			
			//Kill the guard.
			selectedGuard.KillUnit();
		}
	}
}