using GameNet.Plugins.Demigiant.DOTween.Modules;
using RDP.Unit_Controls;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RDP.UI.UISelector {
	public class SelectionNode : MonoBehaviour {
		private bool Configured = false;
		[SerializeField] private Unit linkedUnit;
		[SerializeField] private UISelectionObject selectionObject;
		[SerializeField] private SelectionNodeState state;
		[SerializeField] private Image image;

		[SerializeField] private GameObject stateImage;
		private Image StateImageRef => stateImage.GetComponent<Image>();

		[SerializeField] private TextMeshProUGUI stateText;
		private Button interactButton => GetComponent<Button>();

		[Header("Animation")] [SerializeField] private float transitionDuration;

		private KeyCode keyDownTrigger;
		private bool OnKeyDownTrigger;

		public Button Button => interactButton;

		public TextMeshProUGUI StateText {
			get => stateText;
			set => stateText = value;
		}

		public SelectionNodeState State {
			get => state;
			set => state = value;
		}

		public void Configure(Sprite image, UISelectionObject selectionObject, Unit unit = null) {
			linkedUnit = unit;
			this.image.sprite = image;
			State = SelectionNodeState.unselected;
			this.selectionObject = selectionObject;
			Configured = true;
		}

		private void FixedUpdate() {
			if (Configured) {
				UpdateButtonState();
				UpdateStateText();
				
				if (!linkedUnit) return;
				if (linkedUnit.state != UnitState.Killed) return;
				
				//Unit Killed Disable this!
				Unselected(); // Make sure to unselect it
				State = SelectionNodeState.disabled;
				StateText.text = "KILLED";
			} else {
				Debug.LogWarning($"UI Selection Node not configured!");
			}
		}

		private void UpdateStateText() {
			if (linkedUnit) StateText.text = linkedUnit.AIState;
		}

		private void UpdateButtonState() {
			switch (state) {
				case SelectionNodeState.selected:
					Selected();
					break;
				case SelectionNodeState.unselected:
					Unselected();
					break;
				case SelectionNodeState.disabled:
					Disabled();
					break;
			}
		}

		private void Selected() {
			StateImageRef.DOColor(new Color32(255, 255, 255, 255), transitionDuration);
		}

		private void Unselected() {
			StateImageRef.DOColor(new Color32(255, 255, 255, 0), transitionDuration);
		}

		private void Disabled() {
			//stateImage.SetActive(false);
			//stateTextBackground.color = Color.red;
			Button.interactable = false;
		}
	}
}