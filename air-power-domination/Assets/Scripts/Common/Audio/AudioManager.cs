using System.Collections;
using System.Collections.Generic;
using RDP.Common.Audio.Enums;
using RDP.Common.Audio.SubClass;
using RDP.Common.Utils.Debugger;
using UnityEngine;
using AudioType = RDP.Common.Audio.Enums.AudioType;

namespace RDP.Common.Audio {
	public class AudioManager : MonoBehaviour, IDebuger {
		public static AudioManager Instance; //Singleton TODO: remove this if using for multiplayer maybe.
		public bool debug;
		public bool dontDestroy;
		private Hashtable _audioTable;
		private Hashtable _jobTable;
		[SerializeField] private List<AudioTrack> tracks = new List<AudioTrack>();
		private List<AudioType> _audioTypesCurrentlyPlaying = new List<AudioType>();
		private DebugTool debugger;

		//Project Specific


		private void Awake() {
			if (!Instance) {
				Instance = this;
				if (dontDestroy) DontDestroyOnLoad(gameObject);
			} else {
				Destroy(gameObject);
			}

			if (debug) debugger = new DebugTool(name);
			Setup();
		}

		private void Setup() {
			Instance = this;
			_audioTable = new Hashtable();
			_jobTable = new Hashtable();
			GenerateAudioTable();
		}

		private void GenerateAudioTable() {
			foreach (AudioTrack track in tracks) {
				foreach (AudioObject audioObj in track.audio)
					// TODO: Dont duplicate keys
					if (_audioTable.Contains(audioObj.type)) {
						LogWarning(
							$"You are trying to register [{audioObj.type.ToString()}] more than once. This will cause issues.");
					} else {
						_audioTable.Add(audioObj.type, track);
						Log($"Registering audio [{audioObj.type.ToString()}].");
					}
			}
		}

		private void Dispose() {
			foreach (DictionaryEntry entry in _jobTable) {
				Coroutine job = (Coroutine) entry.Value;
				StopCoroutine(job);
			}
		}

		private void OnDisable() {
			Dispose();
		}

		public void Log(string message) {
			if (!debug) return;
			debugger.Log(message);
		}

		public void LogWarning(string message) {
			if (!debug) return;
			debugger.LogWarning(message);
		}

		private IEnumerator RunAudioJob(AudioJob job) {
			AudioTrack track = (AudioTrack) _audioTable[job.Type];
			track.source.clip = GetAudioClipFromTrack(job.Type, track);
			track.source.loop = job.Loop;
			track.source.volume = job.Volume;

			switch (job.Action) {
				case AudioAction.Start:
					track.source.Play();
					break;
				case AudioAction.Stop:
					if (!job.Fade) track.source.Stop();
					break;
				case AudioAction.Restart:
					track.source.Stop();
					track.source.Play();
					break;
			}

			if (job.Fade) {
				float initial = job.Action == AudioAction.Start || job.Action == AudioAction.Restart ? 0 : job.Volume;
				float target = initial == 0 ? job.Volume : 0;
				float duration = job.FadeDuration;
				float timer = 0;


				while (timer <= duration) {
					// Log($"{initial} {target} {duration} {timer}");;
					track.source.volume = Mathf.Lerp(initial, target, timer / duration);
					timer += Time.deltaTime;
					yield return null;
				}

				if (job.Action == AudioAction.Stop) track.source.Stop();
			}

			_jobTable.Remove(job.Type);
			Log($"Job Count : {_jobTable.Count}");

			yield return null;
		}

		private AudioClip GetAudioClipFromTrack(AudioType type, AudioTrack track) {
			foreach (AudioObject obj in track.audio)
				if (obj.type == type)
					return obj.clip;
			return null;
		}


		private void ResolveConflictingJob(AudioType type) {
			// Conflict 1 : Job is already running
			if (_jobTable.ContainsKey(type)) RemoveJob(type);

			AudioType conflictingAudioType = AudioType.None;
			foreach (DictionaryEntry entry in _jobTable) {
				AudioType audioType = (AudioType) entry.Key;
				AudioTrack trackInUse = (AudioTrack) _audioTable[audioType];
				AudioTrack trackNeeded = (AudioTrack) _audioTable[type];
				if (trackNeeded.source == trackInUse.source) {
					conflictingAudioType = audioType;
					break;
				}
			}

			if (conflictingAudioType != AudioType.None) RemoveJob(conflictingAudioType);
		}


		private void AddJob(AudioJob _job) {
			// cancel any job that might be using this job's audio source
			ResolveConflictingJob(_job.Type);

			Coroutine _jobRunner = StartCoroutine(RunAudioJob(_job));
			_jobTable.Add(_job.Type, _jobRunner);
			_audioTypesCurrentlyPlaying.Add(_job.Type);
			Log($"Starting job on [{_job.Type}] with operation: {_job.Action}");
		}

		private void RemoveJob(AudioType _type) {
			if (!_jobTable.ContainsKey(_type)) {
				Log($"Trying to stop a job [{_type}] that is not running.");
				return;
			}

			Coroutine _runningJob = (Coroutine) _jobTable[_type];
			StopCoroutine(_runningJob);
			_audioTypesCurrentlyPlaying.Remove(_type);
			_jobTable.Remove(_type);
		}

		#region Public Methods Use this to execute the audio

		public void PlayAudio(AudioType type, bool fadeOut = false, float fadeDuration = 0, float delay = 0,
		                      float volume = 1, bool looping = false) {
			AddJob(new AudioJob(AudioAction.Start, type, fadeOut, fadeDuration, delay, volume, looping));
		}

		public void StopAudio(AudioType type, bool fadeOut = false, float fadeDuration = 0, float delay = 0,
		                      float volume = 1) {
			AddJob(new AudioJob(AudioAction.Stop, type, fadeOut, fadeDuration, delay, volume));
		}

		public void RestartAudio(AudioType type, bool fadeOut = false, float fadeDuration = 0, float delay = 0,
		                         float volume = 1, bool looping = false) {
			AddJob(new AudioJob(AudioAction.Restart, type, fadeOut, fadeDuration, delay, volume, looping));
		}

		public void StopAll(bool fadeOut = false, float fadeDuration = 0, float delay = 0, float volume = 1) {
			foreach (AudioType type in _audioTypesCurrentlyPlaying.ToArray())
				AddJob(new AudioJob(AudioAction.Stop, type, fadeOut, fadeDuration, delay, volume));
		}

		#endregion
	}
}