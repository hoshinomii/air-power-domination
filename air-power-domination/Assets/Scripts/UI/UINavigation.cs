using RDP.Common.Audio;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using AudioType = RDP.Common.Audio.Enums.AudioType;

namespace RDP.UI {
	public class UINavigation : MonoBehaviour {
		public Button creditsButton;
		public Button mainMenuButton;

		private void Start() {
			if (!creditsButton || !mainMenuButton) return;

			creditsButton.onClick.AddListener(() => {
				UISoundEffects.PlayButtonClick();
				AudioManager.Instance.StopAll(true, 1, 0, .15f);
				AudioManager.Instance.PlayAudio(AudioType.BGMCredits, true, 1, 0, .15f, true);
			});

			mainMenuButton.onClick.AddListener(() => {
				UISoundEffects.PlayButtonClick();
				AudioManager.Instance.StopAll(true, 1, 0, .15f);
				AudioManager.Instance.PlayAudio(AudioType.BGMMenu, true, 1, 0, .15f, true);
			});
		}

		public void StartScene() {
			SceneManager.LoadScene("StartScene");
		}

		public void QuitButton() {
			UISoundEffects.PlayButtonClick();
			Application.Quit();
			Debug.Log("Quited the Game.");
		}

		//Back
		public void MainMenu() {
			SceneManager.LoadScene("NetworkingMainMenu");
		}
	}
}