using UnityEngine;

namespace RDP.UI {
	public class HoverEvent : MonoBehaviour {
		[SerializeField] private TriggerEvent onMouseOverEnter;

		private bool debug;

		public TriggerEvent MouseOverEnter {
			get => onMouseOverEnter;
			set => onMouseOverEnter = value;
		}

		[SerializeField] private TriggerEvent onMouseOverExit;

		public TriggerEvent MouseOverExit {
			get => onMouseOverExit;
			set => onMouseOverExit = value;
		}

		private void OnMouseEnter() {
			//Debug.Log("OnMouseHoverEnter");
			onMouseOverEnter?.Invoke();
		}

		private void OnMouseExit() {
			//Debug.Log("OnMouseHoverExit");
			onMouseOverExit?.Invoke();
		}
	}
}