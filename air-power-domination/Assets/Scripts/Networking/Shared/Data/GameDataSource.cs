using System.Collections.Generic;
using RDP.Networking.Shared.Game.Data;
using RDP.Networking.Shared.ScriptableObjects;
using UnityEngine;

namespace RDP.Networking.Shared.Data {
	public class GameDataSource : MonoBehaviour {
		[Tooltip("All CharacterClass data should be slotted in here")] [SerializeField]
		private PlayerDataSO[] m_CharacterData;

		private Dictionary<Vocation, PlayerDataSO> m_CharacterDataMap;


		/// <summary>
		/// static accessor for all GameData.
		/// </summary>
		public static GameDataSource Instance { get; private set; }

		/// <summary>
		/// Contents of the CharacterData list, indexed by CharacterType for convenience.
		/// </summary>
		public Dictionary<Vocation, PlayerDataSO> CharacterDataByType {
			get {
				if (m_CharacterDataMap == null) {
					m_CharacterDataMap = new Dictionary<Vocation, PlayerDataSO>();
					foreach (PlayerDataSO data in m_CharacterData) {
						if (m_CharacterDataMap.ContainsKey(data.CharacterClass))
							throw new System.Exception(
								$"Duplicate vocation definition detected: {data.CharacterClass}");
						m_CharacterDataMap[data.CharacterClass] = data;
					}
				}

				return m_CharacterDataMap;
			}
		}

		private void Awake() {
			if (Instance != null) throw new System.Exception("Multiple GameDataSources defined!");

			DontDestroyOnLoad(gameObject);
			Instance = this;
		}
	}
}