using System.Collections.Generic;
using RDP.Building;
using RDP.Tasks;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RDP.UI.Buildings {
	public class BuildingInfoUI : MonoBehaviour {
		
		private enum State { enabled, disabled }

		private State state;
		
		[SerializeField] private Vector3 maintainRotationValue;
		[SerializeField] private bool maintainRotation;

		private Vector3 originalScaleCanvas;
		[SerializeField] private GameObject canvas;
		[SerializeField] private Color32 mainColor;
		[SerializeField] private TextMeshProUGUI buildingName;
		[SerializeField] private TextMeshProUGUI taskDescription;
		[SerializeField] private TextMeshProUGUI unitRequired;

		[SerializeField] private Image headerImage;
		[SerializeField] private Image progressBar;
		[SerializeField] [Range(0, 100)] private float progress;

		[SerializeField] private float animationSpeed;

		private Building.Building building;
		private string _buildingNameText;
		private string _taskDescriptionText;
		private string _unitRequiredText;
		private Task _currentTask;
		
		private RectTransform RectTransform => GetComponent<RectTransform>();
		[Required] [SerializeField] private CanvasGroup canvasGroup;

		[SerializeField] private GameObject occupiedIconPrefab;
		[SerializeField] private Transform occupiedIconParent;
		
		private List<InteractableIcon> _occupiedIcons = new List<InteractableIcon>();
		private List<InteractableIcon> _interactableIcons = new List<InteractableIcon>();
		
		

		private void Start() {
			Configure();
		}

		private void Configure() {
			state = State.disabled;
			originalScaleCanvas = canvas.transform.localScale;
			building = GetComponentInParent<Building.Building>();

			BuildingInteractor buildingInteract = building.GetComponent<BuildingInteractor>();

			if (!buildingInteract) return;
			
			// for (int i = 0; i < buildingInteract.AvailableInsertionPoints; i++) {
			// 	Debug.Log("HI");
			// 	InteractableIcon node = Instantiate(occupiedIconPrefab, occupiedIconParent).GetComponent<InteractableIcon>();
			// 	_interactableIcons.Add(node);
			// }
		}


		public void RemoveUnitFromOccupiedIcons(GameObject unit) {
			// Find the gameobject linked to the occupied Node
			InteractableIcon node = _occupiedIcons.Find(x => x.unitAssigned == unit);
			
			// then unlink it
			node.RemoveUnit();
			
			// and remove it from the list
			_occupiedIcons.Remove(node);
			
			// and add it to the list of available nodes
			_interactableIcons.Add(node);
		}
		
		public void AssignUnitToInteractableUI(GameObject unit) {
			// check if there are any available unoccupied nodes
			if (_interactableIcons.Count == 0) return;
			
			// find the first unoccupied node
			InteractableIcon node = _interactableIcons.Find(x => x.unitAssigned == null);

			// then call setup with the unit
			node.Setup(unit);
			
			// add this interactable node to the occupied list
			_occupiedIcons.Add(node);
			
			// remove this interactable node from the available list
			_interactableIcons.Remove(node);
		}

		private void UpdateData() {
			_buildingNameText = building.buildingName;
			_currentTask = building.CurrentTask;
			_taskDescriptionText = _currentTask.Description;
			_unitRequiredText = _currentTask.UnitRoleString;
			_buildingNameText = _currentTask.BuildingName;
			progress = _currentTask.CurrentProgress;
			headerImage.color = mainColor;
			progressBar.color = mainColor;
		}

		private void MapData() {
			buildingName.text = _buildingNameText;
			taskDescription.text = _taskDescriptionText;
			unitRequired.text = _unitRequiredText;
			unitRequired.color = mainColor;
			progressBar.fillAmount = _currentTask.IsEmpty ? 1 : progress / 100; //if no current task set to empty;
			// Debug.Log($"{progress} fillAmount Version: {progress / 100}");
		}

		private void LateUpdate() {
			UpdateData();
			MapData();
			MaintainRotation();
		}

		private void MaintainRotation() {
			if (maintainRotation) RectTransform.rotation = Quaternion.Euler(maintainRotationValue);
		}

		private void SetCanvasState(bool state = false) {
			canvas.SetActive(state);
		}


		public void UpdateCanvas() {
			switch (state) {
				// if state is disabled, set it to enabled
				case State.disabled:
					state = State.enabled;
					SetCanvasState(true);
					return;
				// do the opposite if state is enabled
				case State.enabled:
					state = State.disabled;
					SetCanvasState(false);
					break;
			}
		}

		public void Hide() {
			//Debug.Log("hiding");
			//canvasGroup.DOFade(0, 0);
			canvas.SetActive(false);
		}

		public void SetScaling(Vector3 zoomValue) { }
	}
}