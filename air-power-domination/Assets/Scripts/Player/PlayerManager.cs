using System;
using RDP.Multiplayer;
using RDP.Networking.Shared.ScriptableObjects;
using RDP.UI;
using UnityEngine;

namespace RDP.Player {
	[Serializable]
	public class PlayerManager {
		[SerializeField] private PlayerController playerController;
		[SerializeField] private PlayerDataSO playerDataSo;
		[SerializeField] private UIManager uiManager;

		public PlayerManager(PlayerController playerController, PlayerDataSO playerDataSo) {
			this.playerController = playerController;
			playerController.PlayerManager = this;
			this.playerDataSo = playerDataSo;
		}

		public UIManager UI {
			get => uiManager;
			set => uiManager = value;
		}

		public PlayerController PlayerController => playerController;

		public PlayerDataSO PlayerDataSo => playerDataSo;

		public UIManager UIManager => uiManager;

		public void SetTeam(Team team) {
			throw new NotImplementedException();
		}
	}
}