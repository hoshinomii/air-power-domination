using System;

namespace RDP.Networking.Shared.Net.ConnectionManagement {
	[Serializable]
	public class ConnectionPayload {
		public string clientGUID;
		public int clientScene = -1;
		public string playerName;
	}
}