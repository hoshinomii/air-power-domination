namespace RDP.Networking.Shared.Net.ConnectionManagement {
	public enum OnlineMode {
		IpHost = 0, // The server is hosted directly and clients can join by ip address.
		Relay = 1, // The server is hosted over a relay server and clients join by entering a room name.
		UnityRelay = 2 // The server is hosted over a Unity Relay server and clients join by entering a join code.
	}
}