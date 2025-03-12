using System;
using UnityEngine;

namespace RDP.Networking.Shared.ScriptableObjects {
	/// <summary>
	/// This ScriptableObject defines a Player Character for BossRoom. It defines its CharacterClass field for
	/// associated game-specific properties, as well as its graphics representation.
	/// </summary>
	[CreateAssetMenu]
	[Serializable]
	public class Avatar : GuidScriptableObject {
		public PlayerDataSO CharacterClass;
		public Guid Guid;
		public GameObject Graphics;
		public GameObject GraphicsCharacterSelect;

		public Sprite Portrait;
	}
}