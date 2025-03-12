using UnityEngine;

namespace RDP.Common.Utils {
	public static class ScreenshotUtils {
		
		// <summary>
		// Hi
		// </summary>
		public static void TakeScreenshot(string fileName, int size) {
			ScreenCapture.CaptureScreenshot(fileName, 2);
		}
		
	}
}