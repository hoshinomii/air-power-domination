using UnityEngine;

namespace RDP.Unit_Controls {
	public class UnitDrag : MonoBehaviour {
		[SerializeField] private UnitSelector unitSelector;

		public Camera camera;

		// Graphical
		[SerializeField] public RectTransform boxVisual;

		private Vector2 _endPosition;

		// Logical
		private Rect _selectionBox;
		private Vector2 _startPosition;

		private void Start() {
			_startPosition = Vector2.zero;
			_endPosition = Vector2.zero;
			// DrawVisual();
		}

		private void Update() {
			// When clicked
			if (Input.GetMouseButtonDown(0)) {
				_startPosition = Input.mousePosition;
				_selectionBox = new Rect();
			}

			// When dragging
			if (Input.GetMouseButton(0)) {
				_endPosition = Input.mousePosition;
				DrawVisual();
				DrawSelection();
			}

			// When release click
			if (Input.GetMouseButtonUp(0)) {
				SelectUnits();
				_startPosition = Vector2.zero;
				_endPosition = Vector2.zero;
				DrawVisual();
			}
		}

		private void DrawVisual() {
			Vector2 boxStart = _startPosition;
			Vector2 boxEnd = _endPosition;

			// // Debugging
			// var ray = camera.ScreenPointToRay(_startPosition);
			// Debug.DrawRay (ray.origin, ray.direction * 50000000, Color.red);

			Vector2 boxCenter = (boxStart + boxEnd) / 2;
			boxVisual.position = boxCenter;

			Vector2 boxSize = new Vector2(Mathf.Abs(boxStart.x - boxEnd.x), Mathf.Abs(boxStart.y - boxEnd.y));

			boxVisual.sizeDelta = boxSize;
		}

		private void DrawSelection() {
			// Perform X calculation
			if (Input.mousePosition.x < _startPosition.x) {
				// Dragging left
				_selectionBox.xMin = Input.mousePosition.x;
				_selectionBox.xMax = _startPosition.x;
			} else {
				// Dragging right
				_selectionBox.xMin = _startPosition.x;
				_selectionBox.xMax = Input.mousePosition.x;
			}

			// Perform Y calculation
			if (Input.mousePosition.y < _startPosition.y) {
				// Dragging down
				_selectionBox.yMin = Input.mousePosition.y;
				_selectionBox.yMax = _startPosition.y;
			} else {
				// Dragging up
				_selectionBox.yMin = _startPosition.y;
				_selectionBox.yMax = Input.mousePosition.y;
			}
		}

		private void SelectUnits() {
			foreach (GameObject unit in unitSelector.unitList) // If unit is within the bounds of the selection rect
				if (_selectionBox.Contains(camera.WorldToScreenPoint(unit.transform.position)))
					// Add unit to selection if any unit is within the selection
					unitSelector.DragSelect(unit);
		}
	}
}