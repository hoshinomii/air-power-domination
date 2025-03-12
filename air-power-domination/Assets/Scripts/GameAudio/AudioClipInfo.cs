using UnityEngine;
using AudioType = RDP.Common.Audio.Enums.AudioType;

namespace RDP.GameAudio {
	[System.Serializable]
	public struct AudioClipInfo {
		public AudioType clip;

		[Range(0, 1)] public float volume;
		public bool fade;
		public float fadeTime;
		public float delay;
		public bool loop;
	}
}