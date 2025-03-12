using RDP.Common.Utils;
using RDP.Scenario;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RDP.UI.CommanderTools {
	public class Recon : MonoBehaviour {
		public Color32 rangeOutlineColor;

		private CommandWindow CommandWindow => GetComponent<CommandWindow>();
		[SerializeField] [Required] private Button reconButton;
		[SerializeField] [Required] private TextMeshProUGUI positionText;
		[SerializeField] private Color32 highlightColor;
		[SerializeField] private bool bold;
		private string UseBoldStart => bold ? "<b>" : "";
		private string UseBoldEnd => bold ? "</b>" : "";

		private void Start() {
			reconButton.onClick.AddListener(Execute);
		}

		public void Execute() {
			ScenarioEngine.Instance.SetupScenario(ScenarioType.Recon, GetComponentInParent<UIManager>().GetTeam(),
				CommandWindow.X, CommandWindow.Y, 3);
		}

		private void Update() {
			UpdateText();
		}

		private void UpdateText() {
			positionText.text =
				$"{UseBoldStart}INSERTION POINT:{UseBoldEnd} <#{Utils.ColorToHex(highlightColor)}>{CommandWindow.X}, {CommandWindow.Y}</color>";
		}
	}
}