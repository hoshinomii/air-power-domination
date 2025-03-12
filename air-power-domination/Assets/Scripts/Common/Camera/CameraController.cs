using UnityEngine;

namespace RDP.Common.Camera {
	public class CameraController : MonoBehaviour {
		public float cMovementSpeed;
		public float cMovementTime;

		public Vector3 cNewPos;

		public Vector3 dragStartPos;
		public Vector3 dragCurrentPos;

		private UnityEngine.Camera camera => GetComponentInChildren<UnityEngine.Camera>();

		// Start is called before the first frame update
		private void Start() {
			cNewPos = transform.position;
		}

		// Update is called once per frame
		private void Update() {
			HandleMouseInput();
		}

		private void HandleMouseInput() {
			if (Input.GetMouseButtonDown(2)) {
				Plane plane = new Plane(Vector3.up, Vector3.zero);
				Ray ray = camera.ScreenPointToRay(Input.mousePosition);

				float entry;

				if (plane.Raycast(ray, out entry)) dragStartPos = ray.GetPoint(entry);
			}

			if (Input.GetMouseButton(2)) {
				Plane plane = new Plane(Vector3.up, Vector3.zero);
				Ray ray = camera.ScreenPointToRay(Input.mousePosition);

				float entry;

				if (plane.Raycast(ray, out entry)) {
					dragCurrentPos = ray.GetPoint(entry);
					cNewPos = transform.position + dragStartPos - dragCurrentPos;
				}
			}

			transform.position = Vector3.Lerp(transform.position, cNewPos, Time.deltaTime * cMovementTime);
		}
	}
}