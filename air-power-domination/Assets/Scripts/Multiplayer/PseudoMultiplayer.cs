using System.Collections.Generic;
using RDP.Grid_System;
using UnityEngine;

namespace RDP.Multiplayer {
	public class PseudoMultiplayer : MonoBehaviour {
		[Header("MUST HAVE 5 TRANSFORMS!!")] public List<Transform> BuildPositions = new List<Transform>();

		public void Setup(Team team) {
			GridSystem GridSystem = team.GridSystem;
			PlacedObjectTypeSO commandCenter = GridSystem.placedObjectTypeSOList[0];
			PlacedObjectTypeSO samSite = GridSystem.placedObjectTypeSOList[1];
			PlacedObjectTypeSO runway = GridSystem.placedObjectTypeSOList[2];
			PlacedObjectTypeSO aircraftHanger = GridSystem.placedObjectTypeSOList[3];
			PlacedObjectTypeSO watchTower = GridSystem.placedObjectTypeSOList[4];

			GridSystem.placedObjectTypeSO = commandCenter;
			GridSystem.RefreshSelectedObjectType();
			GridSystem.Build(BuildPositions[0].position, true, true);

			GridSystem.placedObjectTypeSO = samSite;
			GridSystem.RefreshSelectedObjectType();
			GridSystem.Build(BuildPositions[1].position, true, false);

			GridSystem.placedObjectTypeSO = runway;
			GridSystem.RefreshSelectedObjectType();
			GridSystem.Build(BuildPositions[2].position, true, false);

			GridSystem.placedObjectTypeSO = aircraftHanger;
			GridSystem.RefreshSelectedObjectType();
			GridSystem.Build(BuildPositions[3].position, true, false);

			GridSystem.placedObjectTypeSO = watchTower;
			GridSystem.RefreshSelectedObjectType();
			GridSystem.Build(BuildPositions[4].position, true, false);

			GridSystem.placedObjectTypeSO = null;
			GridSystem.RefreshSelectedObjectType();

			Debug.Log($"Simulation End");
		}
	}
}