using RDP.Multiplayer;
using UnityEngine;

namespace RDP.Scenario {
	public interface IScenario {
		void ExecuteScenario(ScenarioType scenarioType, Team team, Vector3 position, int range, bool debug);
	}
}