using System;
using RDP.Networking.Shared.Net;
using RDP.Networking.Shared.ScriptableObjects;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;
using Avatar = RDP.Networking.Shared.ScriptableObjects.Avatar;

namespace RDP.Networking.Shared.Game.Entity {
	/// <summary>
	/// NetworkBehaviour component to send/receive GUIDs from server to clients.
	/// </summary>
	public class NetworkAvatarGuidState : NetworkBehaviour {
		[FormerlySerializedAs("AvatarGuidArray")] [HideInInspector]
		public NetworkVariable<NetworkGuid> AvatarGuid = new NetworkVariable<NetworkGuid>();

		private CharacterClassContainer m_CharacterClassContainer;

		[SerializeField] private AvatarRegistry m_AvatarRegistry;

		private Avatar m_Avatar;

		public Avatar RegisteredAvatar {
			get {
				if (m_Avatar == null) RegisterAvatar(AvatarGuid.Value.ToGuid());

				return m_Avatar;
			}
		}

		private void Awake() {
			m_CharacterClassContainer = GetComponent<CharacterClassContainer>();
		}

		public void RegisterAvatar(Guid guid) {
			if (guid.Equals(Guid.Empty))
				// not a valid Guid
				return;

			// based on the Guid received, Avatar is fetched from AvatarRegistry
			if (!m_AvatarRegistry.TryGetAvatar(guid, out Avatar avatar)) {
				Debug.LogError("Avatar not found!");
				return;
			}

			if (m_Avatar != null)
				// already set, this is an idempotent call, we don't want to Instantiate twice
				return;

			m_Avatar = avatar;

			m_CharacterClassContainer.SetCharacterClass(avatar.CharacterClass);
		}
	}
}