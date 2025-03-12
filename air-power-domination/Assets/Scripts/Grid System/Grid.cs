using System;
using RDP.Common.Utils;
using UnityEngine;

namespace RDP.Grid_System {
	public class Grid<TGridObject> {
		private float _cellSize;
		private TGridObject[,] _gridArray;
		private Vector3 _originPosition;

		private int _width,
			_height;

		public Grid(int width, int height, float cellSize, Vector3 originPosition,
		            Func<Grid<TGridObject>, int, int, TGridObject> createGridObject) {
			_width = width;
			_height = height;
			_cellSize = cellSize;
			_originPosition = originPosition;

			_gridArray = new TGridObject[width, height];
			for (int x = 0; x < width; x++) {
				for (int z = 0; z < height; z++) _gridArray[x, z] = createGridObject(this, x, z);
			}

			bool showDebug = false;

			if (showDebug) {
				TextMesh[,] debugTextArray = new TextMesh[width, height];

				for (int x = 0; x < _gridArray.GetLength(0); x++) {
					for (int z = 0; z < _gridArray.GetLength(1); z++) {
						debugTextArray[x, z] = Utils.CreateWorldText(_gridArray[x, z]?.ToString(), null,
							GetWorldPosition(x, z) + new Vector3(cellSize, 0, cellSize) * .5f, 35, Color.white,
							TextAnchor.MiddleCenter, TextAlignment.Center);
						Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z + 1), Color.white, 100f);
						Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x + 1, z), Color.white, 100f);
					}
				}

				Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
				Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

				OnGridObjectChanged += (object sender, OnGridObjectChangedEventArgs eventArgs) => {
					debugTextArray[eventArgs.X, eventArgs.Z].text = _gridArray[eventArgs.X, eventArgs.Z]?.ToString();
				};
			}
		}

		public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;

		public int Width => _width;

		public int Height => _height;

		public TGridObject[,] GetNodeArray() {
			return _gridArray;
		}

		public float GetCellSize() {
			return _cellSize;
		}

		public Vector3 GetWorldPosition(int x, int z) {
			return new Vector3(x, 0, z) * _cellSize + _originPosition;
		}

		public void GetXZ(Vector3 worldPosition, out int x, out int z) {
			x = Mathf.FloorToInt((worldPosition - _originPosition).x / _cellSize);
			z = Mathf.FloorToInt((worldPosition - _originPosition).z / _cellSize);
		}

		public void SetGridObject(int x, int z, TGridObject value) {
			if (x >= 0 && z >= 0 && x < _width && z < _height) {
				_gridArray[x, z] = value;
				TriggerGridObjectChanged(x, z);
			}
		}

		public void TriggerGridObjectChanged(int x, int z) {
			OnGridObjectChanged?.Invoke(this, new OnGridObjectChangedEventArgs {
				X = x, Z = z
			});
		}

		public void SetGridObject(Vector3 worldPosition, TGridObject value) {
			GetXZ(worldPosition, out int x, out int z);
			SetGridObject(x, z, value);
		}

		public TGridObject GetGridObject(int x, int z) {
			if (x >= 0 && z >= 0 && x < _width && z < _height)
				return _gridArray[x, z];
			else
				return default;
		}

		public TGridObject GetGridObject(Vector3 worldPosition) {
			int x, z;
			GetXZ(worldPosition, out x, out z);
			return GetGridObject(x, z);
		}

		public Vector2Int ValidateGridPosition(Vector2Int gridPosition) {
			return new Vector2Int(
				Mathf.Clamp(gridPosition.x, 0, _width - 1),
				Mathf.Clamp(gridPosition.y, 0, _height - 1)
			);
		}

		public class OnGridObjectChangedEventArgs : EventArgs {
			public int X, Z;
		}
	}
}