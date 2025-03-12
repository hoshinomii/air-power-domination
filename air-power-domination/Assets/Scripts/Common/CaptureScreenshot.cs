using RDP.Common.Utils;
using UnityEngine;

namespace RDP.Common {
    public class CaptureScreenshot : MonoBehaviour {
        public KeyCode input;
        public string pathToSaveToo;
        public int size = 2;

        private void FixedUpdate() {
            if (!Input.GetKeyUp(input)) return;
            ScreenshotUtils.TakeScreenshot($"{pathToSaveToo}", size);
            Debug.Log("Screenshot taken");
        }
    }
}