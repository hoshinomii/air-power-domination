using Unity.Netcode;
using Unity.Netcode.Transports.UNET;
using UnityEngine;
using UnityEngine.Assertions;

namespace RDP.Networking.Shared {
	/// <summary>
	/// This exists as a small helper to easily swap transports while still supporting the UI switch from ip to relay based transports.
	/// </summary>
	public class TransportPicker : MonoBehaviour {
		[SerializeField] private NetworkTransport m_IpHostTransport;

		[SerializeField] private NetworkTransport m_UnityRelayTransport;

		/// <summary>
		/// The transport used when hosting the game on an IP address.
		/// </summary>
		public NetworkTransport IpHostTransport => m_IpHostTransport;

		/// <summary>
		/// The transport used when hosting the game over a unity relay server.
		/// </summary>
		public NetworkTransport UnityRelayTransport => m_UnityRelayTransport;

		private void OnValidate() {
			Assert.IsTrue(
				m_IpHostTransport == null ||
				(m_IpHostTransport as UNetTransport || m_IpHostTransport as UnityTransport),
				"IpHost transport must be either Unet transport or UTP.");
		}
	}
}