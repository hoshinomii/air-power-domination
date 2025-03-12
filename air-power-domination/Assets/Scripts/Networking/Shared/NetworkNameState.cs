using Unity.Netcode;
using UnityEngine;

namespace RDP.Networking.Shared {
	/// <summary>
	/// NetworkBehaviour containing only one NetworkVariableString which represents this object's name.
	/// </summary>
	public class NetworkNameState : NetworkBehaviour {
		[HideInInspector] public NetworkVariable<FixedPlayerName> Name = new NetworkVariable<FixedPlayerName>();
	}
}