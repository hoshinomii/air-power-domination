using System.Collections.Generic;
using RDP.Grid_System;
using RDP.Multiplayer;
using RDP.Networking.Shared.Game.Data;
using RDP.Player;
using RDP.Unit_Controls;
using UnityEngine;
using UnityEngine.UI;

namespace RDP.UI.UISelector {
	public class UISelection : MonoBehaviour {
		private bool executeOnce = true;

		[SerializeField] private List<UISelectionObject> selectionObjects = new List<UISelectionObject>();
		[SerializeField] private List<TabElement> tabElements = new List<TabElement>();
		private UIManager UIManager => GetComponentInParent<UIManager>();


		[Space] [Header("UI Elements")] [SerializeField]
		private GameObject unitSelectionNodePrefab;

		[SerializeField] private GameObject unitSelectionWindowPrefab;
		[SerializeField] private GameObject tabPrefab;

		[Header("UI Parents")] [SerializeField]
		private Transform unitSelectionWindowParent;

		[SerializeField] private Transform tabParent;

		public void Configure(PlayerController player) {
			//Commander Configuration, Load in the Building Nodes
			if (player.playerDataSo.CharacterClass == TeamManager.COMMANDER) {
				GameObject unitWindow = Instantiate(unitSelectionWindowPrefab, unitSelectionWindowParent);
				GameObject tab = Instantiate(tabPrefab, tabParent);
				TabElement tabElement = tab.GetComponent<TabElement>();
				tabElement.tabDisplayText.text = $"Buildings";

				UISelectionObject data =
					new UISelectionObject(unitWindow, tabElement, player.playerDataSo.CharacterClass, this);

				GridSystem gridSystem = player.GetTeam().GridSystem;
				List<PlacedObjectTypeSO> buildingDataList = player.GetTeam().GridSystem.placedObjectTypeSOList;


				player.GetTeam().GridSystem.placedObjectTypeSOList.ForEach(building => {
					GameObject buildingNode = Instantiate(unitSelectionNodePrefab, unitWindow.transform);
					SelectionNode node = buildingNode.GetComponent<SelectionNode>();
					node.Configure(building.image, data);
					node.StateText.text = building.nameString;
					// When the Tab is clicked, open the tab
					node.Button.onClick.AddListener(() => {
						ResetAllNodes(data);
						// check if state is selected if so set to disabled else set to selected
						if (node.State == SelectionNodeState.selected) {
							node.State = SelectionNodeState.unselected;
							gridSystem.placedObjectTypeSO = null;
							Debug.Log("Un-click");
						} else {
							node.State = SelectionNodeState.selected;
							gridSystem.placedObjectTypeSO = building;
						}

						gridSystem.RefreshSelectedObjectType();
					});

					data.Nodes.Add(node);
				});

				tab.GetComponent<Button>().onClick.AddListener(() => {
					UnselectAllTabs();
					data.SetState(UISelectionState.selected);
				});

				tabElements.Add(tabElement);
				selectionObjects.Add(data);
			}
		}

		public void Configure(PlayerController player, List<GameObject> unitData, List<UnitRole> roles) {
			if (unitData == null || roles == null) return;
			foreach (UnitRole role in roles) {
				GameObject unitWindow = Instantiate(unitSelectionWindowPrefab, unitSelectionWindowParent);
				GameObject tab = Instantiate(tabPrefab, tabParent);
				UnitData udata = player.playerDataSo.unitDataList.unitsToSpawn.Find(unit => unit.role == role);
				TabElement tabElement = tab.GetComponent<TabElement>();
				tabElement.tabDisplayText.text = role == UnitRole.WeaponsSpecialist ? $"Specialist" : role.ToString();


				UISelectionObject data = new UISelectionObject(unitWindow, tabElement, role, this);
				List<GameObject> units = unitData.FindAll(x => x.GetComponent<Unit>().role == role);


				foreach (GameObject t in units) {
					GameObject unitNode = Instantiate(unitSelectionNodePrefab, unitWindow.transform);
					SelectionNode node = unitNode.GetComponent<SelectionNode>();
					node.Configure(udata.image, data, t.GetComponent<Unit>());
					data.Nodes.Add(node);


					node.Button.onClick.AddListener(() => {
						UnitSelector selector = UIManager.Player.GetComponent<UnitSelector>();
						ResetAllNodes(data);

						// check if state is selected if so set to disabled else set to selected
						if (node.State == SelectionNodeState.selected) {
							node.State = SelectionNodeState.unselected;
							Debug.Log("Un-click");
							if (selector) selector.Deselect(t);
						} else {
							node.State = SelectionNodeState.selected;
							if (selector) selector.ClickSelect(t);
						}
					});
				}

				tab.GetComponent<Button>().onClick.AddListener(() => {
					UnselectAllTabs();
					data.SetState(UISelectionState.selected);
				});


				tabElements.Add(tabElement);
				selectionObjects.Add(data);
			}
		}

		// Last min configurations rescaling the tabs based on how many tabs are created
		private void PostConfig() {
			GridLayoutGroup grid = tabParent.GetComponent<GridLayoutGroup>();
			float tabWidth = tabParent.GetComponent<RectTransform>().rect.width / selectionObjects.Count;
			float tabHeight = tabParent.GetComponent<RectTransform>().rect.height;

			foreach (TabElement tabElement in tabElements) tabElement.Configure(tabWidth, tabHeight);

			grid.cellSize = new Vector2(tabWidth, tabHeight);
		}

		private void UnselectAllTabs() {
			foreach (UISelectionObject tab in selectionObjects) tab.SetState(UISelectionState.unselected);
		}

		public void ResetAllNodes(UISelectionObject tab) {
			foreach (SelectionNode node in tab.Nodes) node.State = SelectionNodeState.unselected;
		}

		public void ResetAllNodes(Vocation tab) {
			UISelectionObject data = selectionObjects.Find(x => x.PlayerRole == tab);
			foreach (SelectionNode node in data.Nodes) node.State = SelectionNodeState.unselected;
		}

		public void ResetAllNodes(UnitRole tab) {
			UISelectionObject data = selectionObjects.Find(x => x.Role == tab);
			foreach (SelectionNode node in data.Nodes) node.State = SelectionNodeState.unselected;
		}

		private void FixedUpdate() {
			//Set the first tab to be selected when it first init
			if (executeOnce) {
				PostConfig();
				UnselectAllTabs();
				executeOnce = false;
				selectionObjects[0].SetState(UISelectionState.selected);
			}
		}
	}
}