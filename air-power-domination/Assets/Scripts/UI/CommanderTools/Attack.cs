using System;
using RDP.Common.Utils;
using RDP.Data;
using RDP.Multiplayer;
using RDP.Scenario;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RDP.UI.CommanderTools {
	public class Attack : MonoBehaviour {
		[SerializeField] private TMP_InputField planeAmountInput;
		[SerializeField] private int planeCountLimit = 5;

		public int planeAmount;
		private CommandWindow CommandWindow => GetComponent<CommandWindow>();

		private UIManager UIManager => GetComponentInParent<UIManager>();
		private Team Team => UIManager.GetTeam();


		[SerializeField] private TextMeshProUGUI positionText;
		[SerializeField] private TextMeshProUGUI rangeText;
		[SerializeField] private TextMeshProUGUI planeAvailableText;
		[SerializeField] private TextMeshProUGUI airstrikeStatusText;
		[SerializeField] [Required] private Button attackButton;
		[SerializeField] private RangeData ranges;
		[SerializeField] private float range;

		[SerializeField] private Color32 highlightColor;
		[SerializeField] private bool bold;

		public Color32 rangeOutlineColor;

		private void Start() {
			planeAmountInput.text = "0";
			attackButton.onClick.AddListener(Execute);
		}

		private void Update() {
			GetInput();
			UpdateText();
			UpdateButton();
		}

		private void UpdateButton() {
			if (planeAmount > 0 && CanAirstrike)
				attackButton.interactable = true;
			else
				attackButton.interactable = false;
		}

		private int PlaneAmount => Team.BuildingManager.PlaneCount;
		private bool CanAirstrike => Team.BuildingManager.AirstrikePossible(planeAmount);

		private void UpdateText() {
			rangeText.text =
				$"{UseBoldStart}ATTACK RANGE:{UseBoldEnd} <#{Utils.ColorToHex(highlightColor)}>{planeAmount * ScenarioEngine.Instance.AirstrikeRangePerNode}</color>";
			positionText.text =
				$"{UseBoldStart}STRIKE LOCATION:{UseBoldEnd} <#{Utils.ColorToHex(highlightColor)}>{CommandWindow.X}, {CommandWindow.Y}</color>";
			planeAvailableText.text =
				$"{UseBoldStart}PLANES AVAILABLE:{UseBoldEnd} <#{Utils.ColorToHex(highlightColor)}>{PlaneAmount}</color> <#{Utils.FailureColorHex}>(-{planeAmount})</color>";
			airstrikeStatusText.text = CanAirstrike
				? $"<#{Utils.SuccessColorHex}>Airstrike is available</color>"
				: $"<#{Utils.FailureColorHex}>Airstrike is not available</color>";
		}

		private string UseBoldStart => bold ? "<b>" : "";
		private string UseBoldEnd => bold ? "</b>" : "";

		public void GetInput() {
			if (planeAmountInput.text != "") {
				planeAmount = Convert.ToInt32(planeAmountInput.text);
				if (planeAmount > planeCountLimit) {
					planeAmount = planeCountLimit;
					planeAmountInput.text = planeCountLimit.ToString();
				}
			}
		}

		public void Execute() {
			ScenarioEngine.Instance.SetupScenario(ScenarioType.Attack, Team, CommandWindow.X, CommandWindow.Y,
				planeAmount);
		}
	}
}