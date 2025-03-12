using RDP.Common.Audio;
using RDP.Player;
using RDP.UI.PhaseUI;
using UnityEngine;
using AudioType = RDP.Common.Audio.Enums.AudioType;

namespace RDP.Multiplayer {
	public class LevelManager : MonoBehaviour {
		public static LevelManager Instance;
		[SerializeField] private LevelState state;

		public LevelState State {
			get => state;
			set {
				state = value;
				UpdateState();
			}
		}

		private void Awake() {
			Configure();
		}

		private void Configure() {
			if (Instance == null)
				Instance = this;
			else
				Destroy(gameObject);

			State = LevelState.Preparation;
		}

		private void UpdateState() {
			switch (State) {
				case LevelState.Preparation:
					PreperationPhase();
					break;
				case LevelState.Action:
					PhaseUI.Instance.ShowPrepMessage("Action Phase", 2f);
					AudioManager.Instance.PlayAudio(AudioType.BGMActionPhase, true, 3f, 3f, .05f, true);
					ActionPhase();
					break;
				case LevelState.GameOver:
					break;
			}
		}
		
		private void PreperationPhase() {
			foreach (Team team in TeamManager.Instance.Teams) {
				team.BuildingManager.allowInstantBuilding = true;
			}
		}
		
		private void ActionPhase() {
			foreach (Team team in TeamManager.Instance.Teams) {
				team.BuildingManager.allowInstantBuilding = false;
			}
		}

		

		/*
		 * <summary>
		 * This is called to end the game and accepts in a Team Object that is the losing team, usually called once Command center is dead
		 * </summary>
		 */
		public void GameOver(Team losingTeam) {
			State = LevelState.GameOver;

			Debug.Log(
				$"Team {losingTeam.TeamID} lost! \nAnd Team {TeamManager.GetOppositeTeam(losingTeam).TeamID} won!");

			//We can handle any game over logic here
			foreach (Team team in TeamManager.Instance.Teams.ToArray()) {
				foreach (PlayerManager player in team.Players) player.UI.GameOverScreen.SetActive(true);
			}
		}
	}
}