using RDP.Common.Audio;
using RDP.Multiplayer;
using RDP.Networking.Shared.Game.Data;
using RDP.Networking.Shared.Net.ConnectionManagement;
using RDP.Scenario;
using RDP.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RDP.Testing {
	public class TestButtons : MonoBehaviour {
		[SerializeField] private bool enable;
		[SerializeField] private Button airstrikeButton;
		[SerializeField] private Button reconButton;
		[SerializeField] private Button leaveRoom;
		[SerializeField] private Button actionPhase;
		[SerializeField] private Button preparationPhase;
		[SerializeField] private Button KillRandomGuard;
		private Team team;

		private void Configure() {

			Debug.Log($"TEST BUTTONS INIT	");
			gameObject.SetActive(true);
			team = GetComponentInParent<UIManager>().GetTeam();
		
			
			// KillRandomGuard.onClick.AddListener(() => {
			// 	team.UnitManager.KillRandomUnitFromRole(UnitRole.Guard);
			// });

			
			actionPhase.onClick.AddListener(() => {
				Debug.Log("Changing state to action phase");
				LevelManager.Instance.State = LevelState.Action;
			});
			
			preparationPhase.onClick.AddListener(() => {
				Debug.Log("Changing state to Preparation phase");
				LevelManager.Instance.State = LevelState.Preparation;
			});

			
			airstrikeButton.onClick.AddListener(() => {
				Debug.Log("Airstrike button clicked");
				ScenarioEngine.Instance.SetupScenarioTest(ScenarioType.Attack, team, 4, 5, 3);
			});

			reconButton.onClick.AddListener(() => {
				Debug.Log("Airstrike button clicked");
				ScenarioEngine.Instance.SetupScenarioTest(ScenarioType.Recon, team, 4, 5);
			});

			leaveRoom.onClick.AddListener(() => {
				Debug.Log("Leave Room");
				UISoundEffects.PlayButtonClick();
				AudioManager.Instance.StopAll(true, 1, 0, .15f);
				// Player is leaving the group
				// first disconnect then return to menu
				GameNetPortal gameNetPortal =
					GameObject.FindGameObjectWithTag("GameNetPortal").GetComponent<GameNetPortal>();
				gameNetPortal.RequestDisconnect();
				SceneManager.LoadScene("NetworkingMainMenu");
			});
		}

		private void Start() {
			if (enable) Configure();
		}
	}
}