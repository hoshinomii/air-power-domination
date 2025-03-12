using System;
using RDP.Networking.Shared.Game.Data;
using RDP.Scenario;
using RDP.Tasks;
using RDP.Tasks.TaskTypes;
using RDP.Unit_Controls;
using UnityEngine;

namespace RDP.Building.Building_Specific {
	public class Watchtower : MonoBehaviour, ITaskExecutableRecursive {
		[SerializeField] private UnitRole roleNeeded;
		[SerializeField] private BuildingOperatingState state;
		[SerializeField] private float range = 40;
		[SerializeField] private LayerMask targetLayer;
		[SerializeField] private int unitsOperating = 0;
		[SerializeField] private Building building;
		[SerializeField] private Task task;
		[SerializeField] private string targetTag;

		private BuildingOperatingState State {
			get => state;
			set => state = value;
		}

		private void Awake() {
			building = GetComponentInParent<Building>();
			State = BuildingOperatingState.Unmanned;
		}

		private bool HasRoles => building.UnitsCurrentlyInteracting.Count > 0 &&
		                         building.UnitsCurrentlyInteracting.FindAll(x => x.GetComponent<Unit>().role == roleNeeded).Count > 0;


		private void FixedUpdate() {
			switch (State) {
				case BuildingOperatingState.Unmanned:
					Idle();
					break;
				case BuildingOperatingState.Operated:
					Operate();
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			if (!HasRoles) State = BuildingOperatingState.Unmanned;
		}

		public void Execute() {
			Debug.Log($"Delegate Call for Watch Tower");
			State = BuildingOperatingState.Operated;
		}


		// Use this to look out for targets;
		private void Operate() {
			// using the targetLayer and range values, do a overlap sphere to find all the targets
			Collider[] targets = Physics.OverlapSphere(transform.position, range, targetLayer);
			foreach (Collider c in targets) {
				if (!c.CompareTag(targetTag)) continue;
				Debug.Log($"{c.name} Found, Shooting It Down");
				c.GetComponent<Recon>().Spotted();
			}
		}

		private void Idle() {
			// Debug.Log($"Idle Mode");
		}

		private void OnDrawGizmos() {
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, range);
		}
	}
}