using UnityEngine;

namespace RDP.Building {
	[CreateAssetMenu(fileName = "Data", menuName = "Prototype 1/Building Data", order = 1)]
	public class BuildingData : ScriptableObject {
		[Header("BuildingType")] public BuildingType buildingType;
	}
}