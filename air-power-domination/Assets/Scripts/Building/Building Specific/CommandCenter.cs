using RDP.Multiplayer;
using RDP.Tasks.TaskTypes;
using UnityEngine;

namespace RDP.Building.Building_Specific {
	public class CommandCenter : MonoBehaviour, ITaskExecutableRepair {
		public void Execute() {
			LevelManager.Instance.GameOver(GetComponentInParent<Building>().buildingManager.GetTeam());
		}
	}
}