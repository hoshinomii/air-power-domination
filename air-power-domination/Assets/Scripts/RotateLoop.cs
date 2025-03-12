using DG.Tweening;
using UnityEngine;

namespace RDP {
	public class RotateLoop : MonoBehaviour {
		public Vector3 rot;

		// Update is called once per frame
		private void Update() {
			transform.DORotate(rot, 1f).SetLoops(-1, LoopType.Incremental);
		}
	}
}