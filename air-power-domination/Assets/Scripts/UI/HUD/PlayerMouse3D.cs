using UnityEngine;

namespace RDP.UI.HUD {
	public class PlayerMouse3D : MonoBehaviour {
		[SerializeField] private Camera cameraRef;

		public Camera Camera {
			get => cameraRef;
			set => cameraRef = value;
		}

		[SerializeField] private LayerMask mouseColliderLayerMask;

		private void Update() {
			Ray ray = cameraRef.ScreenPointToRay(Input.mousePosition);

			if (!Physics.Raycast(ray, out RaycastHit raycastHit, 1000f, mouseColliderLayerMask)) return;
			GameObject target = raycastHit.collider.gameObject;
			target.GetComponentInChildren<Canvas>().worldCamera = Camera;
			// Debug.Log(raycastHit.collider.gameObject.name);
		}
	}
}