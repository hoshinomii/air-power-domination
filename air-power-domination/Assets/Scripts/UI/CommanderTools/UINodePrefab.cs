using GameNet.Plugins.Demigiant.DOTween.Modules;
using RDP.Grid_System;
using UnityEngine;
using UnityEngine.UI;

namespace RDP.UI.CommanderTools {
	public class UINodePrefab : MonoBehaviour {
		[SerializeField] private NodeState _state = NodeState.Idle;
		private Node _node;
		[SerializeField] private int x, y;
		[SerializeField] private bool concealed, built, destroyed, executeOnce;

		[SerializeField] private Image image;
		[SerializeField] private Image outlineImage;
		private CommandWindow CommandWindow => GetComponentInParent<CommandWindow>();
		private Button Button => GetComponent<Button>();

		private UIGridNodes GridNodeManager => GetComponentInParent<UIGridNodes>();

		private Color32 OriginalBaseColor => image.color;

		[SerializeField] private Color32 originalBorderColor;
		[SerializeField] private Color32 selectedBaseColor;
		[SerializeField] private Color32 builtColor;
		[SerializeField] private Color32 destroyedColor;

		private void SetState() {
			GridNodeManager.SetAllNodeStates(NodeState.Idle);
			CommandWindow.AnimateRange();
			State = NodeState.Selected;
			CommandWindow.X = x;
			CommandWindow.Y = y;
		}


		public void Setup(Node node) {
			Button.onClick.AddListener(SetState);
			_node = node;
			x = node.X;
			y = node.Y;
			concealed = node.Concealed;
			built = node.BuiltState;
		}

		public NodeState State {
			get => _state;
			set {
				_state = value;
				executeOnce = false;
			}
		}

		private void UpdateColor() {
			if (destroyed) {
				image.color = destroyedColor;
				return;
			}

			if (built) image.color = concealed ? OriginalBaseColor : builtColor;
		}

		private void UpdateUISelectedState() {
			switch (State) {
				case NodeState.Idle:
					Idle();
					break;

				case NodeState.Selected:
					Selected();
					break;
			}
		}

		private void Idle() {
			outlineImage.DOColor(new
				Color32(
					originalBorderColor.r,
					originalBorderColor.g,
					originalBorderColor.b,
					0), 0.2f);
		}

		private void Selected() {
			CommandWindow.currentNode = null;
			CommandWindow.currentNode = this;
			outlineImage.DOColor(selectedBaseColor, 0.2f);
		}

		private void UpdateNodeState() {
			built = _node.BuiltState;
			concealed = _node.Concealed;
			destroyed = _node.Destroyed;
		}

		private void FixedUpdate() {
			UpdateUISelectedState();
			UpdateNodeState();
			UpdateColor();
		}
	}
}