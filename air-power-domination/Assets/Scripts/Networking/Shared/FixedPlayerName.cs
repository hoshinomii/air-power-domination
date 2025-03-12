using Unity.Collections;
using Unity.Netcode;

namespace RDP.Networking.Shared {
	/// <summary>
	/// Wrapping FixedString so that if we want to change player name max size in the future, we only do it once here
	/// </summary>
	public struct FixedPlayerName : INetworkSerializable {
		private FixedString32Bytes m_Name;

		public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
			serializer.SerializeValue(ref m_Name);
		}

		public override string ToString() {
			return m_Name.ToString();
		}

		public static implicit operator string(FixedPlayerName s) {
			return s.ToString();
		}

		public static implicit operator FixedPlayerName(string s) {
			return new FixedPlayerName() {m_Name = new FixedString32Bytes(s)};
		}
	}
}