using RDP.Multiplayer;
using RDP.Networking.Shared.Game.Data;
using RDP.Player;
using RDP.Tasks;
using RDP.UI.CommanderTools;
using RDP.UI.UISelector;
using RDP.Unit_Controls;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RDP.UI {
	public class UIManager : MonoBehaviour, ITeamReference {
		private PlayerController player;

		public PlayerController Player {
			get => player;
			set => player = value;
		}

		public TaskManager taskManager;
		public UnitSelector unitSelector;
		public RectTransform boxVisual;

		[Required] public GameObject CommanderTools;
		[Required] public GameObject UISelector;

		[Required] public GameObject GameOverScreen;

		private Team _teamReference;

		private UISelection _uiSelection;

		public UISelection UISelection {
			get => _uiSelection;
			set => _uiSelection = value;
		}

		private void Awake() {
			CommanderTools.SetActive(false); //Hide the commander tools
			UISelection = GetComponentInChildren<UISelection>();
		}

		// Start is called before the first frame update
		private void Start() { }

		// Update is called once per frame
		private void Update() {
			//  HandleTasksUI();
		}

		// IF player role is commander re enable the commander tools
		public void Setup(Vocation playerRole) {
			if (playerRole != Vocation.Commander) return;
			CommanderTools.SetActive(true);
			CommandTool commanderTools = GetComponentInChildren<CommandTool>();
			if (commanderTools) {
				UIGridNodes satellite = GetComponentInChildren<UIGridNodes>();
				if (satellite != null) {
					Debug.Log($"Satellite Component found, {satellite}, {_teamReference}");
					satellite.Setup(TeamManager.GetOppositeTeam(GetTeam()).GridSystem.Grid);
				}
			}
		}

		public void SelectAllUnits() {
			unitSelector.SelectAllUnits();
		}

		public void SetTeam(Team team) {
			_teamReference = team;
		}

		public Team GetTeam() {
			return _teamReference;
		}
	}
}