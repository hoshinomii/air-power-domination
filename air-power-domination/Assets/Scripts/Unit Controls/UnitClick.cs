using RDP.Building;
using UnityEngine;
using UnityEngine.Serialization;

namespace RDP.Unit_Controls {
	public class UnitClick : MonoBehaviour {
		[SerializeField] private UnitSelector unitSelector;

		[SerializeField] public Camera camera;

		[SerializeField] private GameObject groundMarkerPrefab;

		[FormerlySerializedAs("Units")] public LayerMask units;
		public LayerMask ground;
		public LayerMask building;
		private GameObject _groundMarker;

		private void Awake() {
			if (!_groundMarker) _groundMarker = Instantiate(groundMarkerPrefab);
		}

		private void Start() {
			unitSelector = unitSelector ?? GetComponent<UnitSelector>();
			camera = camera ?? Camera.main;
		}

		private void Update() {
			if (Input.GetMouseButtonDown(0)) SelectUnit();

			if (Input.GetMouseButtonDown(1)) SetMarker();
		}

		private void SelectUnit() {
			RaycastHit hit;
			Ray ray = camera.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out hit, Mathf.Infinity, units)) {
				if (Input.GetKey(KeyCode.LeftShift)) {
					// Shift key clicked
					unitSelector.ShiftClickSelect(hit.collider.gameObject);
				} else {
					// Normal clicked
					Debug.Log(hit.collider.gameObject);
					unitSelector.ClickSelect(hit.collider.gameObject);
				}
			} else {
				// Not shift clicking
				if (!Input.GetKey(KeyCode.LeftShift)) unitSelector.DeselectAll();
			}
		}

		private void SetMarker() {
			RaycastHit hit;
			Ray ray = camera.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out hit, Mathf.Infinity, building)) {
				Debug.Log("Building");
				unitSelector.SetLocationForAll(hit.point, hit.transform.gameObject.GetComponent<BuildingInteractor>());
				return;
			}

			if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground)) {
				Debug.Log("Ground");
				_groundMarker.transform.position = hit.point;
				//groundMarker.SetActive(false);
				_groundMarker.SetActive(true);
				unitSelector.SetLocationForAll(hit.point);
			}
		}
	}
}