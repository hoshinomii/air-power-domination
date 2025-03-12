using System.Net.NetworkInformation;
using System.Net.Sockets;
using RDP.Common.Audio;
using RDP.Networking.Shared.Net.ConnectionManagement;
using UnityEngine;
using UnityEngine.Assertions;

namespace RDP.Networking.Client.UI {
	/// <summary>
	/// Provides backing logic for all of the UI that runs in the MainMenu stage.
	/// </summary>
	public class MainMenuUI : MonoBehaviour {
		[SerializeField] private PopupPanel m_ResponsePopup;

		private string k_DefaultIP = "127.0.0.1";
		private string _initialIP = "";

		private GameNetPortal m_GameNetPortal;

		private ClientGameNetPortal m_ClientNetPortal;

		private static MainMenuUI s_Instance = null;

		/// <summary>
		/// Small singleton getter, for easy access across other classes, this should be safe,
		/// as it is not touching any aspect of networking
		/// </summary>
		public static MainMenuUI Instance {
			get {
				if (s_Instance == null) s_Instance = FindObjectOfType<MainMenuUI>();

				return s_Instance;
			}
		}

		/// <summary>
		/// This will get more sophisticated as we move to a true relay model
		/// </summary>
		private const int k_ConnectPort = 9998;

		private void Start() {
			// Find the game Net Portal by tag - it should have been created by Startup
			GameObject GamePortalGO = GameObject.FindGameObjectWithTag("GameNetPortal");
			Assert.IsNotNull("No GameNetPortal found, Did you start the game from the Startup scene?");
			m_GameNetPortal = GamePortalGO.GetComponent<GameNetPortal>();
			m_ClientNetPortal = GamePortalGO.GetComponent<ClientGameNetPortal>();

			m_ClientNetPortal.NetworkTimedOut += OnNetworkTimeout;
			m_ClientNetPortal.OnUnityRelayJoinFailed += OnRelayJoinFailed;
			m_ClientNetPortal.ConnectFinished += OnConnectFinished;

			//any disconnect reason set? Show it to the user here.
			ConnectStatusToMessage(m_ClientNetPortal.DisconnectReason.Reason, false);
			m_ClientNetPortal.DisconnectReason.Clear();

			//Obtain local IP Address
			string ipv4 = GetIP(AddressFamilys.iPv4);
			Debug.Log(ipv4);

			//Change IP Address of default 127.0.0.1 to local IP Address
			k_DefaultIP = ipv4;

			if (ipv4 == "") k_DefaultIP = "127.0.0.1";

			Debug.Log("Local IP Address:" + k_DefaultIP);
		}

		public void OnHostClicked() {
			UISoundEffects.PlayButtonClick();
			m_ResponsePopup.SetupEnterGameDisplay(true, "HOST GAME",
				"Find the Host IP in the command prompt and using the ipconfig command.",
				"Select CONFIRM to host a Relay room <br> or select another mode",
				"Select CONFIRM to host a Unity Relay room <br> or select another mode", k_DefaultIP, "CONFIRM",
				(string connectInput, int connectPort, string playerName, OnlineMode onlineMode) => {
					m_GameNetPortal.PlayerName = playerName;
					switch (onlineMode) {
						case OnlineMode.Relay:
							m_GameNetPortal.StartPhotonRelayHost(connectInput);
							break;

						case OnlineMode.IpHost:
							m_GameNetPortal.StartHost(PostProcessIpInput(connectInput), connectPort);
							break;

						case OnlineMode.UnityRelay:
							Debug.Log("Unity Relay Host clicked");
							m_GameNetPortal.StartUnityRelayHost();
							break;
					}
				}, k_DefaultIP, k_ConnectPort);
		}

		public void OnConnectClicked() {
			UISoundEffects.PlayButtonClick();
			m_ResponsePopup.SetupEnterGameDisplay(false, "JOIN GAME", "Input the host IP below",
				"Input the room name below", "Input the join code below", "", "CONFIRM",
				(string connectInput, int connectPort, string playerName, OnlineMode onlineMode) => {
					m_GameNetPortal.PlayerName = playerName;

					switch (onlineMode) {
						case OnlineMode.Relay:
							if (ClientGameNetPortal.StartClientRelayMode(m_GameNetPortal, connectInput,
								out string failMessage) == false) {
								m_ResponsePopup.SetupNotifierDisplay("CONNECTION FAILED", failMessage, false, true);
								return;
							}

							break;

						case OnlineMode.IpHost:
							ClientGameNetPortal.StartClient(m_GameNetPortal, connectInput, connectPort);
							break;

						case OnlineMode.UnityRelay:
							Debug.Log($"Unity Relay Client, join code {connectInput}");
							m_ClientNetPortal.StartClientUnityRelayModeAsync(m_GameNetPortal, connectInput);
							break;
					}

					m_ResponsePopup.SetupNotifierDisplay("CONNECTING", "ATTEMPTING TO JOIN ...", true, false);
				}, k_DefaultIP, k_ConnectPort);
		}

		private string PostProcessIpInput(string ipInput) {
			string ipAddress = ipInput;
			if (string.IsNullOrEmpty(ipInput)) ipAddress = k_DefaultIP;

			return ipAddress;
		}

		/// <summary>
		/// Callback when the server sends us back a connection finished event.
		/// </summary>
		/// <param name="status"></param>
		private void OnConnectFinished(ConnectStatus status) {
			ConnectStatusToMessage(status, true);
		}

		/// <summary>
		/// Takes a ConnectStatus and shows an appropriate message to the user. This can be called on: (1) successful connect,
		/// (2) failed connect, (3) disconnect.
		/// </summary>
		/// <param name="connecting">pass true if this is being called in response to a connect finishing.</param>
		private void ConnectStatusToMessage(ConnectStatus status, bool connecting) {
			switch (status) {
				case ConnectStatus.Undefined:
				case ConnectStatus.UserRequestedDisconnect:
					break;
				case ConnectStatus.ServerFull:
					m_ResponsePopup.SetupNotifierDisplay("CONNECTION FAILED",
						"THE HOST IS FULL AND CANNOT ACCEPT ANY ADDITIONAL CONNECTIONS", false, true);
					break;
				case ConnectStatus.Success:
					if (connecting) m_ResponsePopup.SetupNotifierDisplay("SUCCESS!", "JOINING NOW", false, true);
					break;
				case ConnectStatus.LoggedInAgain:
					m_ResponsePopup.SetupNotifierDisplay("CONNECTION FAILED",
						"YOU HAVE LOGGED IN ELSEWHERE USING THE SAME ACCOUNT", false, true);
					break;
				case ConnectStatus.GenericDisconnect:
					string title = connecting ? "CONNECTION FAILED" : "DISCONNECTED FROM HOST";
					string text = connecting ? "SOMETHING WENT WRONG" : "THE CONNECTION TO THE HOST WAS LOST";
					m_ResponsePopup.SetupNotifierDisplay(title, text, false, true);
					break;
				default:
					Debug.LogWarning(
						$"New ConnectStatus {status} has been added, but no connect message defined for it.");
					break;
			}
		}

		/// <summary>
		/// This should allow us to push a message pop up for connection responses from within other classes
		/// </summary>
		/// <param name="title"></param>
		/// <param name="message"></param>
		/// <param name="displayImage"></param>
		/// <param name="displayConfirmation"></param>
		public void PushConnectionResponsePopup(string title, string message, bool displayImage,
		                                        bool displayConfirmation) {
			m_ResponsePopup.SetupNotifierDisplay(title, message, displayImage, displayConfirmation);
		}

		/// <summary>
		/// Invoked when the client sent a connection request to the server and didn't hear back at all.
		/// This should create a UI letting the player know that something went wrong and to try again
		/// </summary>
		private void OnNetworkTimeout() {
			m_ResponsePopup.SetupNotifierDisplay("CONNECTION FAILED", "UNABLE TO REACH HOST/SERVER", false, true,
				"PLEASE TRY AGAIN");
		}

		private void OnRelayJoinFailed(string message) {
			PushConnectionResponsePopup("UNITY RELAY: JOIN FAILED", $"{message}", true, true);
		}

		private void OnDestroy() {
			if (m_ClientNetPortal != null) {
				m_ClientNetPortal.NetworkTimedOut -= OnNetworkTimeout;
				m_ClientNetPortal.ConnectFinished -= OnConnectFinished;
				m_ClientNetPortal.OnUnityRelayJoinFailed -= OnRelayJoinFailed;
			}

			// Release this instance as soon as we are destroyed
			s_Instance = null;
		}

		public string GetIP(AddressFamilys addFam) {
			//Return null if ADDRESSFAM is Ipv6 but Os does not support it
			if (addFam == AddressFamilys.iPv6 && !Socket.OSSupportsIPv6) return null;

			string output = "";

			foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces()) {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
				//NetworkInterfaceType _type1 = NetworkInterfaceType.Wireless80211;
				NetworkInterfaceType type2 = NetworkInterfaceType.Ethernet;

				//if ((item.NetworkInterfaceType == _type1 || item.NetworkInterfaceType == _type2) && item.OperationalStatus == OperationalStatus.Up)
				if (item.NetworkInterfaceType == type2 && item.OperationalStatus == OperationalStatus.Up)
#endif
					foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
						//IPv4
						if (addFam == AddressFamilys.iPv4) {
							if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
								if (ip.Address.ToString() != "127.0.0.1")
									output = ip.Address.ToString();
						}

						//IPv6
						else if (addFam == AddressFamilys.iPv6) {
							if (ip.Address.AddressFamily == AddressFamily.InterNetworkV6)
								output = ip.Address.ToString();
						}
			}

			return output;
		}

		public enum AddressFamilys {
			iPv4,
			iPv6
		}
	}
}