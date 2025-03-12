using UnityEngine;
using AudioType = RDP.Common.Audio.Enums.AudioType;

namespace RDP.Common.Audio.SubClass {
	[System.Serializable]
	public class AudioObject {
		public AudioType type;
		public AudioClip clip;
	}
}