using Unity.Netcode;
using UnityEngine;

namespace RDP.Networking.Shared.Game.Entity {
	/// <summary>
	/// NetworkBehaviour that represents a player connection and is the "Default Player Prefab" inside Netcode for
	/// GameObjects' NetworkManager. This NetworkBehaviour will contain several other NetworkBehaviours that should
	/// persist throughout the duration of this connection, meaning it will persist between scenes.
	/// </summary>
	[RequireComponent(typeof(NetworkObject))]
	public class PersistentPlayer : NetworkBehaviour {
		[SerializeField] private PersistentPlayerRuntimeCollection m_PersistentPlayerRuntimeCollection;

		[SerializeField] private NetworkNameState m_NetworkNameState;

		[SerializeField] private NetworkAvatarGuidState m_NetworkAvatarGuidState;

		public NetworkNameState NetworkNameState => m_NetworkNameState;

		public NetworkAvatarGuidState NetworkAvatarGuidState => m_NetworkAvatarGuidState;

		private void Awake() {
			DontDestroyOnLoad(this);
		}

		public override void OnNetworkSpawn() {
			gameObject.name = "PersistentPlayer" + OwnerClientId;

			// Note that this is done here on OnNetworkSpawn in case this NetworkBehaviour's properties are accessed
			// when this element is added to the runtime collection. If this was done in OnEnable() there is a chance
			// that OwnerClientID could be its default value (0).
			m_PersistentPlayerRuntimeCollection.Add(this);
		}

		public override void OnDestroy() {
			base.OnDestroy();
			RemovePersistentPlayer();
		}

		public override void OnNetworkDespawn() {
			RemovePersistentPlayer();
		}

		private void RemovePersistentPlayer() {
			m_PersistentPlayerRuntimeCollection.Remove(this);
		}
	}
}