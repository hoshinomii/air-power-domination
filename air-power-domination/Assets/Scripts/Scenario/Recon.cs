using System.Collections;
using RDP.Multiplayer;
using RDP.Networking.Shared.Game.Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RDP.Scenario {
	public class Recon : MonoBehaviour {
		[TabGroup("Prefabs & References")] [ShowInInspector] [TabGroup("Prefabs & References")]
		public GameObject rangeMarker;

		[ShowInInspector] [TabGroup("Stats")] [SerializeField]
		private LayerMask target;

		[TabGroup("Stats")] [ShowInInspector] [TabGroup("Stats")]
		private float _countdown = 5f;

		[ShowInInspector] [TabGroup("Stats")] private float _range = 10f;
		private bool spotted;
		private Team team;
		private int refundAmount;

		private void Start() { }

		public void Setup(Team team, float range, float countdown, Vector3 position, int guardCount) {
			refundAmount = 0;

			this.team = team;
			refundAmount = guardCount;
			StartCoroutine(Execute(range, countdown, position));
		}


		private IEnumerator Execute(float range, float countdown, Vector3 position) {
			transform.position = position;
			_range = range;
			_countdown = countdown;
			rangeMarker.gameObject.SetActive(true);
			rangeMarker.transform.localScale = new Vector3(range / 2, .05f, range / 2);
			yield return new WaitForSeconds(countdown);

			rangeMarker.gameObject.SetActive(false);

			Collider[] targets = Physics.OverlapSphere(transform.position, range, target);
			foreach (Collider target in targets) {
				Debug.Log($"Recon Found {target}");
				target.GetComponent<Building.Building>().UpdateConcealedState(false);
			}
			
			// team.SFXManager.PlayNetworking(AudioType.SfxReconAfter);
			yield return new WaitForSeconds(1.5f);
			Reset();
		}

		private void Reset() {
			_range = 1;
			_countdown = 1;
			transform.position = Vector3.zero;
			rangeMarker.transform.localScale = Vector3.one;

			if (spotted) {
				team.UnitManager.KillRandomUnitFromRole(UnitRole.Guard); //Kill a random guard
			}
			
			// REMOVE THIS ONCE OBJECT POOLING IS IMPLEMENTED
			Destroy(gameObject);
		}

		private void OnDrawGizmos() {
			Gizmos.color = Color.cyan;
			Gizmos.DrawWireSphere(transform.position, _range / 4);
		}

		public void Spotted() {
			Debug.Log($"Uh Oh Got Spotted by a watchtower!");
			spotted = true;
		}
	}
}