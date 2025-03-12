using System.Collections.Generic;
using RDP.Building;
using RDP.Common.Audio;
using RDP.Grid_System;
using RDP.Networking.Shared.Game.Data;
using RDP.Networking.Shared.ScriptableObjects;
using RDP.Player;
using RDP.Unit_Controls;
using UnityEngine;

namespace RDP.Multiplayer {
	public class TeamManager : MonoBehaviour {
		public static readonly Vocation COMMANDER = Vocation.Commander;

		public static TeamManager Instance;

		// public int playercount;
		// public int teamCount;
		[SerializeField] private List<PlayerDataSO> playerData = new List<PlayerDataSO>();
		[SerializeField] private GameObject gridSystem;
		[SerializeField] private List<MapData> maps = new List<MapData>();
		[SerializeField] private GameObject buildingGhost;
		[SerializeField] private GameObject buildingManager;
		[SerializeField] private GameObject unitManager;
		[SerializeField] private GameObject SfxManager;
		public List<Team> teams = new List<Team>();

		private Team teamB;

		private void Awake() {
			Instance = this;
			SetupTeam();
		}

		// This function is mainly for testing purposes only!
		private void SetupTeam() {
			// Go through both teams and link their respective grids
			SetupTeamA();
			// Setup a fake team B
			teamB = SetupTeamB();
		}

		private void Start() {
			GetComponent<PseudoMultiplayer>().Setup(teamB);
		}

		// Simulation function to simulate an enemy team, no players will be loaded..
		private Team SetupTeamB() {
			List<PlayerManager> players = new List<PlayerManager>();

			//Setup the managers
			BuildingGhost ghost = Instantiate(buildingGhost, Vector3.zero, Quaternion.identity)
				.GetComponent<BuildingGhost>();
			BuildingManager buildingManager = Instantiate(this.buildingManager, Vector3.zero, Quaternion.identity)
				.GetComponent<BuildingManager>();
			UnitManager unitManager = Instantiate(this.unitManager, Vector3.zero, Quaternion.identity)
				.GetComponent<UnitManager>();
			SfxManager sfxManager =
				Instantiate(SfxManager, Vector3.zero, Quaternion.identity).GetComponent<SfxManager>();

			ghost.gridSystem = maps[1].GridSystem;
			ghost.DontFollowMouse = false;

			//Renaming
			maps[1].GridSystem.name = $"GridSystem For Team 2";
			ghost.name = $"BuildingGhost For Team 2";
			buildingManager.name = $"BuildingManager For Team 2";
			unitManager.name = $"UnitManager For Team 2";
			sfxManager.name = $"SfxManager For Team 2";

			Team team = new Team(players, buildingManager, unitManager, maps[1].GridSystem, ghost, sfxManager, 1);

			teams.Add(team);
			ghost.SetTeam(team);
			sfxManager.Configure(team);
			maps[1].GridSystem.SetTeam(team);
			buildingManager.SetTeam(team);

			foreach (PlayerManager player in players) player.PlayerController.SetTeam(team);
			players.Clear();

			return team;
		}

		private void SetupTeamA() {
			List<PlayerManager> players = new List<PlayerManager>();

			//Setup the managers
			BuildingGhost ghost = Instantiate(buildingGhost, Vector3.zero, Quaternion.identity)
				.GetComponent<BuildingGhost>();
			BuildingManager buildingManager = Instantiate(this.buildingManager, Vector3.zero, Quaternion.identity)
				.GetComponent<BuildingManager>();
			UnitManager unitManager = Instantiate(this.unitManager, Vector3.zero, Quaternion.identity)
				.GetComponent<UnitManager>();
			SfxManager sfxManager =
				Instantiate(SfxManager, Vector3.zero, Quaternion.identity).GetComponent<SfxManager>();

			ghost.gridSystem = maps[0].GridSystem;
			ghost.DontFollowMouse = false;

			//Renaming
			maps[0].GridSystem.name = $"GridSystem For Team 1";
			ghost.name = $"BuildingGhost For Team 1";
			buildingManager.name = $"BuildingManager For Team 1";
			unitManager.name = $"UnitManager For Team 1";
			sfxManager.name = $"SfxManager For Team 1";

			for (int j = 0; j < 1; j++) {
				PlayerDataSO player = playerData[j];
				PlayerController playerController = Instantiate(player.playerPrefab, Vector3.zero, Quaternion.identity)
					.GetComponent<PlayerController>();
				playerController.name = player.CharacterClass.ToString();
				players.Add(new PlayerManager(playerController, playerController.playerDataSo));
				// Debug.Log(players.Count + " " + players[0]);
			}

			Team team = new Team(players, buildingManager, unitManager, maps[0].GridSystem, ghost, sfxManager, 0);


			sfxManager.Configure(team);
			teams.Add(team);
			ghost.SetTeam(team);
			maps[0].GridSystem.SetTeam(team);
			buildingManager.SetTeam(team);

			foreach (PlayerManager player in players) player.PlayerController.SetTeam(team);

			players.Clear();
		}

		public static Team GetOppositeTeam(Team currentTeam) {
			return Instance.teams[(currentTeam.TeamID + 1) % Instance.teams.Count];
		}

		public List<Team> Teams {
			get => teams;
			set => teams = value;
		}

		public static Team GetTeam(int teamID) {
			return Instance.teams[teamID];
		}

		public static Team GetTeam(PlayerManager playerManager) {
			return Instance.teams.Find(x => x.GetPlayers.Contains(playerManager));
		}

		public static Team GetTeam(BuildingManager manager) {
			return Instance.teams.Find(x => x.BuildingManager == manager);
		}

		public static Team GetTeam(GridSystem grid) {
			return Instance.teams.Find(x => x.GridSystem == grid);
		}
	}
}