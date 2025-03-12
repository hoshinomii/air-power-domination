using RDP.Common.Audio.Enums;

namespace RDP.Common.Audio.SubClass {
	public class AudioJob {
		public readonly AudioAction Action;
		public readonly AudioType Type;
		public bool Fade;
		public float Delay;
		public bool Loop;
		public float Volume;
		public float FadeDuration;

		public AudioJob(AudioAction action, AudioType type, bool fadeOut, float fadeDuration, float delay,
		                float volume) {
			Action = action;
			Type = type;
			Fade = fadeOut;
			Delay = delay;
			FadeDuration = fadeDuration;
			Volume = volume;
		}

		public AudioJob(AudioAction action, AudioType type, bool fadeOut, float fadeDuration, float delay, float volume,
		                bool looping) {
			Action = action;
			Type = type;
			Fade = fadeOut;
			Delay = delay;
			Loop = looping;
			Volume = volume;
			FadeDuration = fadeDuration;
		}
	}
}