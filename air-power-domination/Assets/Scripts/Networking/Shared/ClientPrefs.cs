using System;
using UnityEngine;

namespace RDP.Networking.Shared {
	/// <summary>
	/// Singleton class which saves/loads local-client settings
	/// (This is just a wrapper around the PlayerPrefs system so that all the calls are in the same place)
	/// </summary>
	public class ClientPrefs {
		private const float k_DefaultMasterVolume = 1.0f;
		private const float k_DefaultBGMVolume = 1.0f;
		private const float k_DefaultSFXVolume = 1.0f;

		public static float GetMasterVolume() {
			return PlayerPrefs.GetFloat("MasterVolume", k_DefaultMasterVolume);
		}

		public static void SetMasterVolume(float volume) {
			PlayerPrefs.SetFloat("MasterVolume", volume);
		}

		public static float GetBGMVolume() {
			return PlayerPrefs.GetFloat("MusicVolume", k_DefaultBGMVolume);
		}

		public static void SetBGMVolume(float volume) {
			PlayerPrefs.SetFloat("MusicVolume", volume);
		}

		public static float GetSFXVolume() {
			return PlayerPrefs.GetFloat("EffectsVolume", k_DefaultSFXVolume);
		}

		public static void SetSFXVolume(float volume) {
			PlayerPrefs.SetFloat("EffectsVolume", volume);
		}

		/// <summary>
		/// Either loads a Guid string from Unity preferences, or creates one and checkpoints it, then returns it.
		/// </summary>
		/// <returns>The Guid that uniquely identifies this client install, in string form. </returns>
		public static string GetGuid() {
			if (PlayerPrefs.HasKey("client_guid")) return PlayerPrefs.GetString("client_guid");

			Guid guid = System.Guid.NewGuid();
			string guidString = guid.ToString();

			PlayerPrefs.SetString("client_guid", guidString);
			return guidString;
		}
	}
}