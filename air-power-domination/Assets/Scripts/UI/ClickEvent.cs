using UnityEngine;

namespace RDP.UI {
    public class ClickEvent : MonoBehaviour {
        [SerializeField] private TriggerEvent onMouseDown;

        private bool debug;

        public TriggerEvent MouseDown {
            get => onMouseDown;
            set => onMouseDown = value;
        }

        [SerializeField] private TriggerEvent onMouseUp;

        public TriggerEvent MouseUp {
            get => onMouseUp;
            set => onMouseUp = value;
        }

        private void OnMouseDown() {
            //Debug.Log("OnMouseHoverEnter");
            onMouseDown?.Invoke();
        }

        private void OnMouseUp() {
            //Debug.Log("OnMouseHoverExit");
            onMouseUp?.Invoke();
        }
    }
}