using RDP.Common.Utils;
using RDP.Multiplayer;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RDP.Scenario {
	public class ScenarioEngine : MonoBehaviour, IScenario {
		public static ScenarioEngine Instance;

		[TabGroup("Scenario Instantiate Prefabs")] [ShowInInspector] [TabGroup("Scenario Instantiate Prefabs")]
		public GameObject airstrikePrefab;

		[ShowInInspector] [TabGroup("Scenario Instantiate Prefabs")]
		public GameObject reconPrefab;

		[SerializeField] private float setupCountdown = 5;


		[SerializeField] public float airstrikeRangePerPlaneScalar = .5f;
		public int AirstrikeRangePerNode => (int) (40 * airstrikeRangePerPlaneScalar);

		[SerializeField] public float reconRangePerGuardUnitScalar = 1;
		public int ReconRangePerNode => (int) (40 * reconRangePerGuardUnitScalar);

		private float _range;

		private void Awake() {
			Instance = this;
		}

		public void ExecuteScenario(ScenarioType scenarioType, Team team, Vector3 position, int resourceCount,
		                            bool debug) {
			switch (scenarioType) {
				case ScenarioType.Attack:
					Airstrike(position, resourceCount, team, debug);
					break;
				case ScenarioType.Recon:
					Recon(position, resourceCount, team, debug);
					break;
			}
		}

		private void Airstrike(Vector3 position, int resourceCount, Team team, bool debug) {
			if (!debug) { // if debug is set to false then dont check if there is enough Planes to conduct the Operation
				if (!team.BuildingManager.AirstrikePossible(resourceCount)) {
					Debug.Log("Insufficient Planes!!!");
					return;
				}

				// Remove the selected planes for now
				team.BuildingManager.RemovePlanes(resourceCount);
			}

			float range = resourceCount * team.GridSystem.cellSize * 2 * airstrikeRangePerPlaneScalar;
			GameObject scenario = Instantiate(airstrikePrefab, position, Quaternion.identity);
			Debug.Log(
				$"[Scenario Engine] Starting Airstrike at Position {position} With Range: {range} and Countdown: {setupCountdown}");
			scenario.GetComponent<Airstrike>().Setup(team, range, setupCountdown, position, resourceCount);
		}

		private void Recon(Vector3 position, int resourceCount, Team team, bool debug) {
			if (!debug) // if debug is set to false then dont check if there is enough Planes to conduct the Operation
				if (!team.UnitManager.HasGuards) {
					Debug.Log("Insufficient Guards!!!");
					return;
				}
			// Remove the selected planes for now

			float range = resourceCount * (team.GridSystem.cellSize * 2 * reconRangePerGuardUnitScalar);


			Debug.Log(
				$"[Scenario Engine] Starting Recon at Position {position} With Range: {range} and Countdown: {setupCountdown}");
			GameObject scenario = Instantiate(reconPrefab, position, Quaternion.identity);
			scenario.GetComponent<Recon>().Setup(team, range, setupCountdown, position, resourceCount);
		}

		private async void AirstrikeAsync(Vector3 position, int range, Team team) { }

		// Call this for non testing functions or to bomb the enemy Team
		public void SetupScenario(ScenarioType type, Team team, int x = 0, int y = 0, int resourceCount = 0) {
			if (LevelManager.Instance.State == LevelState.Preparation) return; // This will make it so that only scenarios can be called when it is in action phase
			Vector3 position = Utils.GetPositionalDataFromGrid(TeamManager.GetOppositeTeam(team), x, y);
			ExecuteScenario(type, team, position, resourceCount, false);
		}

		// Testing Function
		public void SetupScenarioTest(ScenarioType type, Team team, int x = 0, int y = 0, int range = 5) {
			Vector3 position = Utils.GetPositionalDataFromGrid(team, x, y);
			ExecuteScenario(type, team, position, range, true);
		}
	}
}