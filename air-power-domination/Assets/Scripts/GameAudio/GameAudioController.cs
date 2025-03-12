using RDP.Common.Audio;
using RDP.Common.Utils.Debugger;
using UnityEngine;

namespace RDP.GameAudio {
	public class GameAudioController : MonoBehaviour, IDebuger {
		public bool debug;
		private DebugTool debugger;

		[Header("This will play on scene load")] [SerializeField]
		private AudioClipInfo onLoadMusic;

		public void Log(string message) {
			if (debug) debugger.Log(message);
		}

		public void LogWarning(string message) {
			if (debug) debugger.LogWarning(message);
		}


		private void Configure() {
			if (debug) debugger = new DebugTool($"this");
			if (AudioManager.Instance == null) {
				LogWarning("AudioManager is null!");
				return;
			}

			AudioManager.Instance.StopAll(true, 1f, 0f, .15f);
			AudioManager.Instance.PlayAudio(
				onLoadMusic.clip,
				onLoadMusic.fade,
				onLoadMusic.fadeTime,
				onLoadMusic.delay, onLoadMusic.volume,
				onLoadMusic.loop);
		}

		private void Start() {
			Configure();
		}
	}
}