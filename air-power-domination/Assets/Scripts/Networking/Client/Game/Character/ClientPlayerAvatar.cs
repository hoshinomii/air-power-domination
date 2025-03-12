using System;
using Unity.Netcode;
using UnityEngine;

namespace RDP.Networking.Client.Game.Character {
	public class ClientPlayerAvatar : NetworkBehaviour {
		[SerializeField] private ClientPlayerAvatarRuntimeCollection m_PlayerAvatars;

		public static event Action<ClientPlayerAvatar> LocalClientSpawned;

		public static event Action LocalClientDespawned;

		public override void OnNetworkSpawn() {
			name = "PlayerAvatar" + OwnerClientId;

			if (IsClient && IsOwner) LocalClientSpawned?.Invoke(this);

			if (m_PlayerAvatars) m_PlayerAvatars.Add(this);
		}

		public override void OnNetworkDespawn() {
			if (IsClient && IsOwner) LocalClientDespawned?.Invoke();

			RemoveNetworkCharacter();
		}

		public override void OnDestroy() {
			base.OnDestroy();
			RemoveNetworkCharacter();
		}

		private void RemoveNetworkCharacter() {
			if (m_PlayerAvatars) m_PlayerAvatars.Remove(this);
		}
	}
}