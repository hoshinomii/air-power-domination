using UnityEngine;

namespace RDP.Grid_System {
	public class VisualizedNode : MonoBehaviour {
		private MeshRenderer _renderer;

		[SerializeField] private Material unoccupiedMaterial;
		[SerializeField] private Material occupiedMaterial;

		private Node nodeData;

		public Node NodeData {
			get => nodeData;
			set => nodeData = value;
		}

		private void FixedUpdate() {
			_renderer.material = NodeData.PlacedObject ? occupiedMaterial : unoccupiedMaterial;
		}

		public void Configure(Node node, Vector3 pos, float cellSize) {
			_renderer = GetComponent<MeshRenderer>();
			NodeData = node;
			float offset = cellSize / 2;
			transform.position = new Vector3(pos.x + offset, 0.3f, pos.z + offset);
			transform.localScale = new Vector3(cellSize, cellSize, 1);
		}
	}
}