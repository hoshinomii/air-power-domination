using System;
using RDP.Building;
using RDP.Networking.Shared.Game.Data;
using UnityEngine;

namespace RDP.Tasks {
	[Serializable]
	public class Task {
		private BuildingManager _buildingManager;

		[SerializeField] [Range(0, 100)] private float currentProgress;
		[SerializeField] private string buildingName;
		[SerializeField] private string name;
		[SerializeField] private Vocation playerVocation;
		[SerializeField] private TaskManager taskManager;
		[SerializeField] private TaskState taskState;
		[SerializeField] private UnitRole unitRole;
		[SerializeField] private float processingTime;
		[SerializeField] private bool isRecursive;

		// Create one without using TaskData Scriptable Object for a simple Task (One Start Point and One End Point).
		public Task(string name, string buildingName, float processingRate, BuildingManager buildingManager,
		            Vocation role, UnitRole unitRole, bool recursive) {
			processingTime = processingRate;
			this.name = name;
			this.buildingName = buildingName;
			playerVocation = role;
			this.unitRole = unitRole;
			_buildingManager = buildingManager;
			taskState = TaskState.Incomplete;
			isRecursive = recursive;
		}

		public bool IsRecursive => isRecursive;
		public string Description => name;
		public string UnitRoleString => unitRole.ToString();
		public string BuildingName => buildingName;

		public float CurrentProgress {
			get => currentProgress;
			set => currentProgress = value;
		}

		//function to check if this class is empty
		public bool IsEmpty => string.IsNullOrEmpty(name);

		public TaskManager TaskManager {
			get => taskManager;
			set => taskManager = value;
		}

		public UnitRole UnitRole {
			get => unitRole;
			set => unitRole = value;
		}

		public TaskState State {
			get => taskState;
			set => taskState = value;
		}

		public Vocation PlayerVocation => playerVocation;

		public float ProcessingRate {
			get => processingTime;
			set => processingTime = value;
		}

		// public void CompleteTask()
		// {
		//     // taskManager.CompleteTask(this);
		// }

		public void SetCurrentProgress(float currentProgress) {
			// Debug.Log($"Current Progress For Task {name} is {this.currentProgress} and it is adding {currentProgress}");;
			if (this.currentProgress >= 100) {
				taskState = TaskState.Complete;
				this.currentProgress = 100;
				return;
			}

			this.currentProgress += currentProgress;
		}

		public float GetCurrentProgress() {
			return currentProgress;
		}

		public void Restart() {
			State = TaskState.Incomplete;
		}
	}
}