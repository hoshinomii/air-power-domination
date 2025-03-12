using RDP.Grid_System;
using UnityEngine;

namespace RDP.Multiplayer {
	public class MapData : MonoBehaviour {
		public GameObject gridPrefab;
		public Transform playerSpawnPoint;
		public GridSystem GridSystem => gridPrefab.GetComponent<GridSystem>();
	}
}