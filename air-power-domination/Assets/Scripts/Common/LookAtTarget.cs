using UnityEngine;

namespace RDP.Common {
	public class LookAtTarget : MonoBehaviour {
		public Transform target;

		private void FixedUpdate() {
			// Rotate the camera every frame so it keeps looking at the target
			// transform.LookAt(target);
			transform.rotation =
				Quaternion.LookRotation(transform.position - (target ? target.transform.position : Vector3.zero));
		}
	}
}