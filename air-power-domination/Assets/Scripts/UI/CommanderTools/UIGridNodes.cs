using System.Collections.Generic;
using RDP.Grid_System;
using UnityEngine;
using UnityEngine.UI;

namespace RDP.UI.CommanderTools {
	public class UIGridNodes : MonoBehaviour {
		[SerializeField] private List<UINodePrefab> nodes = new List<UINodePrefab>();
		[SerializeField] private Transform nodePrefab;
		[SerializeField] private float spacing;


		public Vector2 cellSize;
		private Transform nodeParent => transform;
		private GridLayoutGroup layoutGroup => GetComponent<GridLayoutGroup>();

		private Vector2Int cellSpacing => new Vector2Int((int) layoutGroup.spacing.x, (int) layoutGroup.spacing.y);
		private RectTransform _rectTransform => GetComponent<RectTransform>();

		public void Setup(Grid<Node> gridData) {
			Rect rect = _rectTransform.rect;
			float x_spacing = cellSpacing.x * 2;
			float y_spacing = cellSpacing.y * 2;

			float rectWidth = rect.width;
			float rectHeight = rect.height;

			cellSize = new Vector2(rectWidth / gridData.Width - x_spacing,
				rectHeight / gridData.Height - y_spacing);
			layoutGroup.cellSize = cellSize;
			SpawnNodes(gridData.Width, gridData.Height, gridData);
		}

		private void SpawnNodes(int width, int height, Grid<Node> gridData) {
			for (int y = 0; y < height; y++) {
				for (int x = 0; x < width; x++) {
					UINodePrefab node = Instantiate(nodePrefab, nodeParent).GetComponent<UINodePrefab>();
					node.transform.SetParent(nodeParent);
					node.Setup(gridData.GetGridObject(x, y));
					nodes.Add(node.GetComponent<UINodePrefab>());
				}
			}
		}

		public void SetAllNodeStates(NodeState state) {
			foreach (UINodePrefab node in nodes) node.State = state;
		}
	}
}