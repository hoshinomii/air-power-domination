using RDP.Networking.Shared.Game.Entity;
using RDP.Networking.Shared.ScriptableObjects;
using UnityEngine;

namespace RDP.Networking.Shared.Game {
	/// <summary>
	/// A runtime list of <see cref="PersistentPlayer"/> objects that is populated both on clients and server.
	/// </summary>
	[CreateAssetMenu]
	public class PersistentPlayerRuntimeCollection : RuntimeCollection<PersistentPlayer> {
		public bool TryGetPlayer(ulong clientID, out PersistentPlayer persistentPlayer) {
			for (int i = 0; i < Items.Count; i++)
				if (clientID == Items[i].OwnerClientId) {
					persistentPlayer = Items[i];
					return true;
				}

			persistentPlayer = null;
			return false;
		}
	}
}