using UnityEngine;

namespace RDP.Common.Utils.Debugger {
	public class DebugTool : IDebuger {
		private readonly string _debuggerTitle;

		public DebugTool(string title) {
			Debug.Log($"[DebugTool] Initializing Debugger for Object Name: [{title}]");
			_debuggerTitle = title;
		}

		public void Log(string message) {
			Debug.Log($"[{_debuggerTitle}] {message}");
		}

		public void LogWarning(string message) {
			Debug.Log($"[{_debuggerTitle}] {message}");
		}
	}
}