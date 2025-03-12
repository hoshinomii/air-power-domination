using RDP.Networking.Client.Game.Character;
using RDP.Networking.Shared.Game.Entity;
using RDP.Networking.Shared.ScriptableObjects;
using UnityEngine;

namespace RDP.Networking.Client.Game {
	/// <summary>
	/// A runtime list of <see cref="PersistentPlayer"/> objects that is populated both on clients and server.
	/// </summary>
	[CreateAssetMenu]
	public class ClientPlayerAvatarRuntimeCollection : RuntimeCollection<ClientPlayerAvatar> { }
}