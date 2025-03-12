using DG.Tweening;
using UnityEngine;

namespace RDP.Common.Camera {
	public class CameraShake : MonoBehaviour {
		public static CameraShake Instance;

		public void Shake(UnityEngine.Camera camera, float duration, float strength) {
			camera.DOShakePosition(duration, strength);
		}
	}
}