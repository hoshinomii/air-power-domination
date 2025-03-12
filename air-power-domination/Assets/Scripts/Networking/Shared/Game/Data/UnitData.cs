using System;
using UnityEngine;

namespace RDP.Networking.Shared.Game.Data {
	[Serializable]
	public struct UnitData {
		public UnitRole role;
		public GameObject unitPrefab;
		public Vector3 insertionPoint;
		public int amount;
		public Color32 primaryColor;
		public Sprite image;
	}
}