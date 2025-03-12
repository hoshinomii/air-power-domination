using UnityEngine;

namespace RDP.Common.Audio {
	public class AudioTester : MonoBehaviour {
		public AudioManager audioManager;
		public KeyCode testKey1;
		public KeyCode testKey2;
		public KeyCode testKey3;
		public KeyCode testKey4;
		public KeyCode testKey5;
		public KeyCode testKey6;

		#region Unity Methods

#if UNITY_EDITOR
		private void Update() { }
#endif

		#endregion
	}
}