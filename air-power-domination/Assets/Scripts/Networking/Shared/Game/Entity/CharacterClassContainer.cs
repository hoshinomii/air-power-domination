using RDP.Networking.Shared.ScriptableObjects;
using UnityEngine;

namespace RDP.Networking.Shared.Game.Entity {
	/// <summary>
	/// The source of truth for a PC/NPCs CharacterClass.
	/// Player's class is dynamic. NPC's class isn't. This class serves as a single access point for static vs dynamic classes
	/// </summary>
	public class CharacterClassContainer : MonoBehaviour {
		[SerializeField] private PlayerDataSO m_CharacterClass;

		public PlayerDataSO CharacterClass {
			get {
				if (m_CharacterClass == null) m_CharacterClass = m_State.RegisteredAvatar.CharacterClass;

				return m_CharacterClass;
			}
		}

		private NetworkAvatarGuidState m_State;

		private void Awake() {
			m_State = GetComponent<NetworkAvatarGuidState>();
		}

		public void SetCharacterClass(PlayerDataSO characterClass) {
			m_CharacterClass = characterClass;
		}
	}
}