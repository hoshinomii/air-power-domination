using System;
using RDP.Multiplayer;
using UnityEngine;
using UnityEngine.Serialization;

namespace RDP.Grid_System {
	public class BuildingGhost : MonoBehaviour, ITeamReference {
		[FormerlySerializedAs("GridSystem")] public GridSystem gridSystem;
		private PlacedObjectTypeSO placedObjectTypeSO;

		private Team _teamReference;
		private Transform _visual;
		public bool DontFollowMouse;

		private void Start() {
			RefreshVisual();
			gridSystem.OnSelectedChanged += Instance_OnSelectedChanged;
		}

		private void LateUpdate() {
			if (DontFollowMouse) return;
			Vector3 targetPosition = gridSystem.GetMouseWorldSnappedPosition();
			targetPosition.y = 1f;
			transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 15f);
			transform.rotation =
				Quaternion.Lerp(transform.rotation, gridSystem.GetPlacedObjectRotation(), Time.deltaTime * 15f);
		}

		public void SetTeam(Team team) {
			_teamReference = team;
		}

		public Team GetTeam() {
			return _teamReference;
		}

		private void Instance_OnSelectedChanged(object sender, EventArgs e) {
			RefreshVisual();
		}

		private void RefreshVisual() {
			if (_visual != null) {
				Destroy(_visual.gameObject);
				_visual = null;
			}

			PlacedObjectTypeSO placedObjectTypeSO = gridSystem?.GetPlacedObjectTypeSO();
			if (placedObjectTypeSO == null) return;

			_visual = Instantiate(placedObjectTypeSO.visual, Vector3.zero, Quaternion.identity);
			_visual.parent = transform;
			_visual.localPosition = Vector3.zero;
			_visual.localEulerAngles = Vector3.zero;
			SetLayerRecursive(_visual.gameObject, 11);
		}

		private void SetLayerRecursive(GameObject targetGameObject, int layer) {
			targetGameObject.layer = layer;

			foreach (Transform child in targetGameObject.transform) SetLayerRecursive(child.gameObject, layer);
		}
	}
}