namespace RDP.Networking.Shared.Net.ConnectionManagement {
	/// <summary>
	/// Represents a single player on the game server
	/// </summary>
	public struct PlayerData {
		public string m_PlayerName; //name of the player
		public ulong m_ClientID; //the identifying id of the client

		public PlayerData(string playerName, ulong clientId) {
			m_PlayerName = playerName;
			m_ClientID = clientId;
		}
	}
}