using System;
using Unity.Netcode;
using Unity.Netcode.Transports.UNET;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RDP.Networking.Shared.Net.ConnectionManagement {
	public class GameNetPortal : MonoBehaviour {
		[SerializeField] private NetworkManager m_NetworkManager;

		public NetworkManager NetManager => m_NetworkManager;

		/// <summary>
		/// the name of the player chosen at game start
		/// </summary>
		public string PlayerName;

		/// <summary>
		/// How many connections we create a Unity relay allocation for
		/// </summary>
		private const int k_MaxUnityRelayConnections = 8;

		// Instance of GameNetPortal placed in scene. There should only be one at once
		public static GameNetPortal Instance;
		private ClientGameNetPortal m_ClientPortal;
		private ServerGameNetPortal m_ServerPortal;

		private void Awake() {
			Debug.Assert(Instance == null);
			Instance = this;
			m_ClientPortal = GetComponent<ClientGameNetPortal>();
			m_ServerPortal = GetComponent<ServerGameNetPortal>();
		}

		private void Start() {
			DontDestroyOnLoad(gameObject);

			//we synthesize a "OnNetworkSpawn" event for the NetworkManager out of existing events. At some point
			//we expect NetworkManager will expose an event like this itself.
			NetManager.OnServerStarted += OnNetworkReady;
			NetManager.OnClientConnectedCallback += ClientNetworkReadyWrapper;
		}

		private void OnSceneEvent(SceneEvent sceneEvent) {
			// only processing single player finishing loading events
			if (sceneEvent.SceneEventType != SceneEventType.LoadComplete) return;

			m_ServerPortal.OnClientSceneChanged(sceneEvent.ClientId,
				SceneManager.GetSceneByName(sceneEvent.SceneName).buildIndex);
		}

		private void OnDestroy() {
			if (NetManager != null) {
				NetManager.OnServerStarted -= OnNetworkReady;
				NetManager.OnClientConnectedCallback -= ClientNetworkReadyWrapper;
			}

			Instance = null;
		}

		private void ClientNetworkReadyWrapper(ulong clientId) {
			if (clientId == NetManager.LocalClientId) {
				OnNetworkReady();
				NetManager.SceneManager.OnSceneEvent += OnSceneEvent;
			}
		}

		/// <summary>
		/// This method runs when NetworkManager has started up (following a successful connect on the client, or directly after StartHost is invoked
		/// on the host). It is named to match NetworkBehaviour.OnNetworkSpawn, and serves the same role, even though GameNetPortal itself isn't a NetworkBehaviour.
		/// </summary>
		private void OnNetworkReady() {
			if (NetManager.IsHost)
				//special host code. This is what kicks off the flow that happens on a regular client
				//when it has finished connecting successfully. A dedicated server would remove this.
				m_ClientPortal.OnConnectFinished(ConnectStatus.Success);

			m_ClientPortal.OnNetworkReady();
			m_ServerPortal.OnNetworkReady();
		}

		/// <summary>
		/// Initializes host mode on this client. Call this and then other clients should connect to us!
		/// </summary>
		/// <remarks>
		/// See notes in GNH_Client.StartClient about why this must be static.
		/// </remarks>
		/// <param name="ipaddress">The IP address to connect to (currently IPV4 only).</param>
		/// <param name="port">The port to connect to. </param>
		public void StartHost(string ipaddress, int port) {
			NetworkTransport chosenTransport =
				NetworkManager.Singleton.gameObject.GetComponent<TransportPicker>().IpHostTransport;
			NetworkManager.Singleton.NetworkConfig.NetworkTransport = chosenTransport;

			// Note: In most cases, this switch case shouldn't be necessary. It becomes necessary when having to deal with multiple transports like this
			// sample does, since current Transport API doesn't expose these fields.
			switch (chosenTransport) {
				case UNetTransport unetTransport:
					unetTransport.ConnectAddress = ipaddress;
					unetTransport.ServerListenPort = port;
					break;
				case UnityTransport unityTransport:
					unityTransport.SetConnectionData(ipaddress, (ushort) port);
					break;
				default:
					throw new Exception($"unhandled IpHost transport {chosenTransport.GetType()}");
			}

			NetManager.StartHost();
		}

		public void StartPhotonRelayHost(string roomName) {
			NetManager.StartHost();
		}

		public async void StartUnityRelayHost() {
			NetworkTransport chosenTransport =
				NetworkManager.Singleton.gameObject.GetComponent<TransportPicker>().UnityRelayTransport;
			NetworkManager.Singleton.NetworkConfig.NetworkTransport = chosenTransport;

			switch (chosenTransport) {
				case UnityTransport utp:
					Debug.Log("Setting up Unity Relay host");

					try {
						await UnityServices.InitializeAsync();
						if (!AuthenticationService.Instance.IsSignedIn) {
							await AuthenticationService.Instance.SignInAnonymouslyAsync();
							string playerId = AuthenticationService.Instance.PlayerId;
							Debug.Log(playerId);
						}
					} catch (Exception e) {
						Debug.LogErrorFormat($"{e.Message}");
						throw;
					}

					break;
				default:
					throw new Exception($"unhandled relay transport {chosenTransport.GetType()}");
			}

			NetManager.StartHost();
		}

		/// <summary>
		/// This will disconnect (on the client) or shutdown the server (on the host).
		/// It's a local signal (not from the network), indicating that the user has requested a disconnect.
		/// </summary>
		public void RequestDisconnect() {
			m_ClientPortal.OnUserDisconnectRequest();
			m_ServerPortal.OnUserDisconnectRequest();
			NetManager.Shutdown();
		}
	}
}