using RDP.Networking.Shared.ScriptableObjects;
using Unity.Netcode;
using UnityEngine;

namespace RDP.Networking.Shared.Game.State {
	public class NetworkGameState : NetworkBehaviour {
		[SerializeField] private TransformVariable m_GameStateTransformVariable;

		[SerializeField] private NetworkWinState m_NetworkWinState;

		public NetworkWinState NetworkWinState => m_NetworkWinState;

		private void Awake() {
			DontDestroyOnLoad(this);
		}

		public override void OnNetworkSpawn() {
			gameObject.name = "NetworkGameState";

			m_GameStateTransformVariable.Value = transform;
		}

		public override void OnNetworkDespawn() {
			m_GameStateTransformVariable.Value = null;
		}
	}
}