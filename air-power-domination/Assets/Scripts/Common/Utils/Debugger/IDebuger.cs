namespace RDP.Common.Utils.Debugger {
	public interface IDebuger {
		void Log(string message);
		void LogWarning(string message);
	}
}