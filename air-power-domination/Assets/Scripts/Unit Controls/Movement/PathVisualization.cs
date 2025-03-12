using UnityEngine;

namespace RDP.Unit_Controls.Movement {
	public class PathVisualization : MonoBehaviour {
		public LineRenderer line;

		private void Start() {
			line = GetComponentInChildren<LineRenderer>();
		}

		public void CreateLines(Vector3[] positions, Color32 color) {
			line.material.color = color;
			line.positionCount = positions.Length;
			for (int i = 0; i < positions.Length; i++)
				line.SetPosition(i, new Vector3(positions[i].x, transform.position.y, positions[i].z));
		}

		public void ClearLines() {
			// clear the lines
			line.positionCount = 0;
		}
	}
}