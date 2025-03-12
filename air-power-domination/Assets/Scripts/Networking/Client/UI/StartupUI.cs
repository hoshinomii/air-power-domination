using UnityEngine;
using UnityEngine.SceneManagement;

namespace RDP.Networking.Client.UI {
	/// <summary>
	/// Provides backing logic for any UI before MainMenu stage. Mostly we just load main menu
	/// </summary>
	public class StartupUI : MonoBehaviour {
		private void Start() {
			SceneManager.LoadScene("NetworkingMainMenu");
		}
	}
}