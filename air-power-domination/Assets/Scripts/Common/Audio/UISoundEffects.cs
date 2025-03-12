using Sirenix.OdinInspector;
using UnityEngine;
using AudioType = RDP.Common.Audio.Enums.AudioType;

namespace RDP.Common.Audio {
	public class UISoundEffects : MonoBehaviour {
		// This is to be used as a Singleton Hookup for sfxManager but for UI Buttons ONLY!
		[SerializeField] [Required] private SfxManager sfxManager;
		public static UISoundEffects Instance;

		// Start is called before the first frame update
		private void Start() {
			if (!Instance) {
				Instance = this;
				DontDestroyOnLoad(this);
			} else {
				Destroy(gameObject);
			}
		}

		public static void PlayButtonClick() {
			if (Instance.sfxManager) Instance.sfxManager.Play(AudioType.SfxUIButtonClick);
		}
	}
}