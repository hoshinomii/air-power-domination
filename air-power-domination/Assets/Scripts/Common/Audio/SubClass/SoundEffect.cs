using UnityEngine;
using AudioType = RDP.Common.Audio.Enums.AudioType;

namespace RDP.Common.Audio.SubClass {
	[System.Serializable]
	public class SoundEffect {
		public AudioType name;
		public AudioClip clip;

		[Range(0, 1)] public float volume = 1;
		[Range(0, 1)] public float pitch = 1;

		[HideInInspector] public AudioSource source;
	}
}