using System;
using System.Collections.Generic;
using System.Linq;
using RDP.Building;
using RDP.Common.Audio;
using RDP.Grid_System;
using RDP.Networking.Shared.Game.Data;
using RDP.Networking.Shared.ScriptableObjects;
using RDP.Player;
using RDP.Unit_Controls;
using UnityEngine;

namespace RDP.Multiplayer {
	[Serializable]
	public class Team {
		[SerializeField] private List<PlayerManager> players = new List<PlayerManager>();
		[SerializeField] private BuildingManager buildingManager;
		[SerializeField] private BuildingGhost buildingGhost;
		[SerializeField] private UnitManager unitManager;
		[SerializeField] private GridSystem grid;
		[SerializeField] private SfxManager sfxManager;
		[SerializeField] private int teamID;

		public Team(List<PlayerManager> players, BuildingManager buildingManager, UnitManager unitManager,
		            GridSystem grid, BuildingGhost buildingGhost,
		            SfxManager sfxManager, int teamID) {
			foreach (PlayerManager player in players) this.players.Add(player);
			this.buildingManager = buildingManager;
			this.grid = grid;
			this.unitManager = unitManager;
			this.buildingGhost = buildingGhost;
			this.teamID = teamID;
			this.sfxManager = sfxManager;
		}

		public List<PlayerManager> Players {
			get => players;
			set => players = value;
		}

		public PlayerController GetPlayerController(Vocation role) {
			return players.Find(x => x.PlayerDataSo.CharacterClass == role).PlayerController;
		}

		public PlayerController GetPlayerController(List<Vocation> roles) {
			PlayerController res = null;
			foreach (Vocation role in roles) res = GetPlayerController(role);
			return res;
		}

		public PlayerDataSO GetPlayerData(Vocation role) {
			return players.Find(x => x.PlayerDataSo.CharacterClass == role).PlayerDataSo;
		}

		public Vocation GetPlayerRoles(UnitRole role) {
			Vocation res = Vocation.None;
			foreach (PlayerManager player in players.Where(player => player.PlayerDataSo.UnitRoleExists(role)))
				res = player.PlayerDataSo.GetPlayerRoles();
			return res;
		}

		public int TeamID => teamID;

		public List<PlayerManager> GetPlayers => players;

		public BuildingManager BuildingManager => buildingManager;

		public int GetPlayerCount => players.Count;

		public GridSystem GridSystem => grid;
		public UnitManager UnitManager => unitManager;

		public SfxManager SFXManager => sfxManager;

		// public void DelegateTask(Vocation role, Task task)
		// {
		//     var player = GetPlayerController(role).GetComponent<TaskManager>();
		//     task.TaskManager = player;
		//     player.GetTask(task);
		// }
	}
}