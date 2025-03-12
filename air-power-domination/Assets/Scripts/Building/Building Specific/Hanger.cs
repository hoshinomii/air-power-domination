using RDP.Tasks.TaskTypes;
using UnityEngine;

namespace RDP.Building.Building_Specific {
	public class Hanger : MonoBehaviour, ITaskExecutableRecursive {
		public int planesToAddUponCompletion = 1;
		private Building Building => GetComponentInParent<Building>();

		public void Execute() {
			Debug.Log("Added Planes");
			Building.buildingManager.AddPlanes(planesToAddUponCompletion);
		}
	}
}