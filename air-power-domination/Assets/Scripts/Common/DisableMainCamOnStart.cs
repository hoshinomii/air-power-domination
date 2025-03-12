using UnityEngine;

namespace RDP.Common {
	public class DisableMainCamOnStart : MonoBehaviour {
		// Start is called before the first frame update
		private void Start() {
			gameObject.SetActive(false);
		}
	}
}