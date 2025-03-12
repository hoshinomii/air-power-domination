using System.Collections;
using RDP.Building;
using RDP.Multiplayer;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RDP.Scenario {
	public class Airstrike : MonoBehaviour {
		[TabGroup("Prefabs & References")] [ShowInInspector] [TabGroup("Prefabs & References")]
		public GameObject rangeMarker;

		[ShowInInspector] [TabGroup("Prefabs & References")]
		public GameObject explosion;

		[ShowInInspector] [TabGroup("Stats")] [SerializeField]
		private LayerMask target;

		[TabGroup("Stats")] [ShowInInspector] [TabGroup("Stats")]
		private float _countdown = 5f;

		[ShowInInspector] [TabGroup("Stats")] private float _range = 10f;


		[TabGroup("Animations")]
		[ShowInInspector]
		[TabGroup("Animations")]
		public Animator ExplosionAnimator => explosion.GetComponent<Animator>();

		private int refundAmount;
		private bool spotted;
		private Team team;

		private void Start() {
			spotted = false;
			explosion.SetActive(false);
		}

		public void Setup(Team team, float range, float countdown, Vector3 position, int planeCount) {
			refundAmount = 0;


			this.team = team;
			refundAmount = planeCount;
			Debug.Log($"{team} {refundAmount}");
			StartCoroutine(Execute(range, countdown, position));
		}

		private IEnumerator Execute(float range, float countdown, Vector3 position) {
			transform.position = position;
			_range = range;
			_countdown = countdown;
			rangeMarker.gameObject.SetActive(true);
			rangeMarker.transform.localScale = new Vector3(range / 2, .05f, range / 2);
			explosion.transform.localScale = new Vector3(range / 10, range / 10, range / 10);
			yield return new WaitForSeconds(countdown);

			rangeMarker.gameObject.SetActive(false);
			// CameraShake.Instance.Shake();
			explosion.SetActive(true);

			Collider[] targets = Physics.OverlapSphere(transform.position, range / 4, target);
			foreach (Collider target in targets) {
				Building.Building building = target.GetComponent<Building.Building>();
				if (building != null && building.state != BuildingState.Unbuit) {
					building.SetBuildingState(BuildingState.Destroyed);
					Debug.Log("Destroyed Building");
				}
			}

			yield return new WaitForSeconds(3f);
			Reset();
		}

		private void Reset() {
			_range = 1;
			_countdown = 1;
			transform.position = Vector3.zero;
			explosion.transform.localScale = Vector3.one;
			rangeMarker.transform.localScale = Vector3.one;

			if (!spotted) { // Airstrike was successful without being spotted refund the planes
				Debug.Log(refundAmount);
				team.BuildingManager.Refund(refundAmount);
			}

			// REMOVE THIS ONCE OBJECT POOLING IS IMPLEMENTED
			Destroy(gameObject);
		}

		private void OnDrawGizmos() {
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, _range / 4);
		}

		public void Spotted() {
			Debug.Log($"Uh Oh Got Spotted by a SAM Site!");
			spotted = true;
		}
	}
}