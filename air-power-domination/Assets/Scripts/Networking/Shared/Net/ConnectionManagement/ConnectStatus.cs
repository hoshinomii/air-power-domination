namespace RDP.Networking.Shared.Net.ConnectionManagement {
	public enum ConnectStatus {
		Undefined,
		Success, //client successfully connected. This may also be a successful reconnect.
		ServerFull, //can't join, server is already at capacity.
		LoggedInAgain, //logged in on a separate client, causing this one to be kicked out.
		UserRequestedDisconnect, //Intentional Disconnect triggered by the user.
		GenericDisconnect //server disconnected, but no specific reason given.
	}
}