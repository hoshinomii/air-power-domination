using System;
using System.Collections.Generic;
using RDP.Common.Audio.SubClass;
using RDP.Common.Utils.Debugger;
using RDP.Multiplayer;
using UnityEngine;
using AudioType = RDP.Common.Audio.Enums.AudioType;


namespace RDP.Common.Audio {
	public class SfxManager : MonoBehaviour, IDebuger, ITeamReference {
		[SerializeField] private bool requireTeam;
		[SerializeField] private bool debug;
		private DebugTool _debugger;
		private Team team;
		[SerializeField] private List<SoundEffect> soundEffects = new List<SoundEffect>();


		// This will be called and setup by team Manager!
		public void Configure(Team teamReference) {
			SetTeam(teamReference);
			if (debug) _debugger = new DebugTool($"{name}");
			foreach (SoundEffect s in soundEffects) {
				s.source = gameObject.AddComponent<AudioSource>();
				s.source.clip = s.clip;
				s.source.volume = s.volume;
				s.source.pitch = s.pitch;
			}
		}

		// Non Team Reliant Variant
		public void Configure() {
			if (debug) _debugger = new DebugTool($"{name}");
			foreach (SoundEffect s in soundEffects) {
				s.source = gameObject.AddComponent<AudioSource>();
				s.source.clip = s.clip;
				s.source.volume = s.volume;
				s.source.pitch = s.pitch;
			}
		}

		// Play Sound Effect
		// TODO : Update to RPC When using Unity Netcode!
		public void Play(AudioType name) {
			SoundEffect s = soundEffects.Find(x => x.name == name);
			if (s == null)
				Log($"AudioType {name} not found inside SFX Manager");
			else
				s.source.PlayOneShot(s.source.clip);
		}


		public Team GetTeam() {
			return team;
		}

		public void SetTeam(Team team) {
			this.team = team;
		}

		public void Log(string message) {
			if (debug) Debug.Log(message);
		}

		public void LogWarning(string message) {
			throw new NotImplementedException();
		}

		private void Start() {
			if (!requireTeam) Configure();
		}
	}
}