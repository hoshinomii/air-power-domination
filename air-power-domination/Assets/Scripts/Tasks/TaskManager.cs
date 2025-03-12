using System.Collections.Generic;
using UnityEngine;

namespace RDP.Tasks {
	public class TaskManager : MonoBehaviour {
		[SerializeField] private List<Task> currentTasks = new List<Task>();

		// Start is called before the first frame update
		private void Start() {
			Init();
		}

		public void CompleteTask(Task task) {
			if (currentTasks.Contains(task)) currentTasks.Remove(task);
		}

		public void GetTask(Task task) {
			currentTasks.Add(task);
		}

		private void Init() { }
	}
}