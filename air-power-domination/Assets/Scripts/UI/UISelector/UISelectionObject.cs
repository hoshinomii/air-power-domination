using System.Collections.Generic;
using RDP.Networking.Shared.Game.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RDP.UI.UISelector {
	[System.Serializable]
	public class UISelectionObject {
		[SerializeField] private List<SelectionNode> nodes = new List<SelectionNode>();
		[SerializeField] private UISelectionState state;
		[SerializeField] private GameObject window;
		[SerializeField] private TabElement tab;
		[SerializeField] private UnitRole role;
		[SerializeField] private Vocation playerRole;
		[SerializeField] private UISelection selectionRef;

		private Image _windowImage;

		private Image _tabImage;
		private TextMeshProUGUI _tabText;

		public GameObject Window => window;
		public TabElement Tab => tab;
		public UnitRole Role => role;
		public List<SelectionNode> Nodes => nodes;
		public Vocation PlayerRole => playerRole;

		public UISelectionState State {
			get => state;
			set => state = value;
		}

		public UISelectionObject(GameObject window, TabElement tab, UnitRole role, UISelection selectionRef) {
			this.window = window;
			this.tab = tab;
			this.role = role;
			playerRole = Vocation.None;
			this.selectionRef = selectionRef;

			_tabImage = tab.GetComponent<Image>();
			_tabText = tab.GetComponentInChildren<TextMeshProUGUI>();

			State = UISelectionState.unselected;
			UpdateState();
		}

		// No Role taken in
		public UISelectionObject(GameObject window, TabElement tab, Vocation vocation, UISelection selectionRef) {
			this.window = window;
			this.tab = tab;
			State = UISelectionState.unselected;
			playerRole = vocation;
			this.selectionRef = selectionRef;

			_tabImage = tab.GetComponent<Image>();
			_tabText = tab.GetComponentInChildren<TextMeshProUGUI>();
		}


		public void SetState(UISelectionState state) {
			State = state;
			UpdateState();
		}

		private void UpdateState() {
			switch (State) {
				case UISelectionState.selected:
					Selected();
					break;
				case UISelectionState.unselected:
					Unselected();
					break;
				case UISelectionState.disabled:
					Disabled();
					break;
			}
		}

		private void Selected() {
			tab.Enable();
			window.SetActive(true);
		}

		private void Unselected() {
			tab.Disable();
			selectionRef.ResetAllNodes(this);
			window.SetActive(false);
		}

		private void Disabled() { }
	}
}