using UnityEngine;

namespace RDP.Grid_System {
	
	public class Mouse3D : MonoBehaviour {
		[SerializeField] private Camera cameraRef;

		[SerializeField] private LayerMask mouseColliderLayerMask;
		public static Mouse3D Instance { get; private set; }

		private void Awake() {
			Instance = this;
		}

		private void Update() {
			Ray ray = cameraRef.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out RaycastHit raycastHit, 1000f, mouseColliderLayerMask))
				transform.position = raycastHit.point;
		}

		public void SetCamera(Camera camera) {
			cameraRef = camera;
		}

		public Vector3 GetMouseWorldPosition() {
			Ray ray = cameraRef.ScreenPointToRay(Input.mousePosition);

			return Physics.Raycast(ray, out RaycastHit raycastHit, 1000f, mouseColliderLayerMask)
				? raycastHit.point
				: Vector3.zero;
		}

		// private Vector3 GetMouseWorldPosition_Instance() {
		// 	
		// }
	}
}