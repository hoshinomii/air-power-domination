using System;

namespace RDP.Networking.Shared.Net {
	public static class NetworkGuidExtensions {
		public static NetworkGuid ToNetworkGuid(this Guid id) {
			NetworkGuid networkId = new NetworkGuid();
			networkId.FirstHalf = BitConverter.ToUInt64(id.ToByteArray(), 0);
			networkId.SecondHalf = BitConverter.ToUInt64(id.ToByteArray(), 8);
			return networkId;
		}

		public static Guid ToGuid(this NetworkGuid networkId) {
			byte[] bytes = new byte[16];
			Buffer.BlockCopy(BitConverter.GetBytes(networkId.FirstHalf), 0, bytes, 0, 8);
			Buffer.BlockCopy(BitConverter.GetBytes(networkId.SecondHalf), 0, bytes, 8, 8);
			return new Guid(bytes);
		}
	}
}