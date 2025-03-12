using Unity.Netcode;

namespace RDP.Networking.Shared.Net {
	public struct NetworkGuid : INetworkSerializable {
		public ulong FirstHalf;
		public ulong SecondHalf;

		public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
			serializer.SerializeValue(ref FirstHalf);
			serializer.SerializeValue(ref SecondHalf);
		}
	}
}