using RDP.Networking.Shared.Game.Data;
using UnityEngine;

namespace RDP.Networking.Shared.ScriptableObjects {
	[CreateAssetMenu(fileName = "Data", menuName = "Prototype 1/Task Data", order = 1)]

// TODO: We can add more in the future to suit the needs.
	public class TaskData : ScriptableObject {
		public string name;
		public float processingTime;
		public UnitRole unitRoleRequired;
		public Vocation playerRoleRequired;
		public bool isRecursive;
	}
}