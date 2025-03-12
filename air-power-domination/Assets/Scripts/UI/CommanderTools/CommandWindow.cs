using DG.Tweening;
using GameNet.Plugins.Demigiant.DOTween.Modules;
using RDP.Scenario;
using UnityEngine;
using UnityEngine.UI;

namespace RDP.UI.CommanderTools {
	public class CommandWindow : MonoBehaviour {
		private bool callOnce;
		public UINodePrefab currentNode;

		public GameObject airstrikeTab;
		public GameObject reconTab;

		private float _rangePerResource = 0;
		public int resourceCount;

		private Vector2 NodeSize => GetComponentInChildren<UIGridNodes>().cellSize;
		[SerializeField] private Color32 defaultColor = new Color32(255, 255, 255, 255);
		private CurrentAttackTab currentTab;

		public CurrentAttackTab CurrentTab {
			get => currentTab;
			set {
				currentTab = value;
				float cellSize;
				switch (currentTab) {
					case CurrentAttackTab.Airstrike:
						rangeOutline.SetActive(false);
						RangeOutline.color = AttackClass.rangeOutlineColor;
						rangeOutlineInner.color = new Color32(AttackClass.rangeOutlineColor.r,
							AttackClass.rangeOutlineColor.g, AttackClass.rangeOutlineColor.b, 100);
						airstrikeTab.SetActive(true);
						reconTab.SetActive(false);
						break;

					case CurrentAttackTab.Recon:
						rangeOutline.SetActive(false);
						RangeOutline.color = ReconClass.rangeOutlineColor;
						rangeOutlineInner.color = new Color32(ReconClass.rangeOutlineColor.r,
							ReconClass.rangeOutlineColor.g, ReconClass.rangeOutlineColor.b, 100);
						airstrikeTab.SetActive(false);
						reconTab.SetActive(true);
						break;

					case CurrentAttackTab.None:

						_rangePerResource = 0;
						resourceCount = 0;

						RangeOutline.color = defaultColor;
						airstrikeTab.SetActive(false);
						reconTab.SetActive(false);
						rangeOutline.SetActive(false);
						break;
				}
			}
		}


		private Vector2Int _gridPosition;
		private Attack AttackClass => GetComponentInChildren<Attack>();
		private Recon ReconClass => GetComponentInChildren<Recon>();

		public GameObject rangeOutline;
		[SerializeField] private Image rangeOutlineInner;
		public Image RangeOutline => rangeOutline.GetComponent<Image>();
		public RectTransform RangeOutlineRectTransform => rangeOutline.GetComponent<RectTransform>();

		public int X {
			get => _gridPosition.x;
			set => _gridPosition.x = value;
		}

		public int Y {
			get => _gridPosition.y;
			set => _gridPosition.y = value;
		}

		private void Configure() {
			rangeOutline.SetActive(false);
			RangeOutlineRectTransform.sizeDelta = NodeSize;
		}

		private void Awake() {
			callOnce = true;
		}

		private void FixedUpdate() {
			if (callOnce) {
				callOnce = false;
				Configure();
			}

			UpdateRangeData();
			if (!currentNode) return;
			//RangeOutlineRectTransform.position = currentNode.GetComponent<RectTransform>().position + new Vector3(NodeSize.x / 2,
			//    (NodeSize.y / 2) * -1 , 1);

			RangeOutlineRectTransform.DOMove(currentNode.GetComponent<RectTransform>().position + new Vector3(
				NodeSize.x / 2,
				NodeSize.y / 2 * -1, 1), .1f);
		}

		public void AnimateRange() {
			rangeOutline.SetActive(true);
			float cellSize = NodeSize.x * resourceCount * _rangePerResource;
			RangeOutlineRectTransform.sizeDelta = Vector2.zero;
			RangeOutlineRectTransform.DOSizeDelta(new Vector2(cellSize, cellSize), .1f);
		}

		private void AnimateRange(Vector2 range) {
			if (!currentNode) return;
			rangeOutline.SetActive(true);
			RangeOutlineRectTransform.sizeDelta = Vector2.zero;
			RangeOutlineRectTransform.DOSizeDelta(range, .1f);
		}

		private void UpdateRangeData() {
			switch (currentTab) {
				case CurrentAttackTab.Airstrike:
					_rangePerResource = ScenarioEngine.Instance.airstrikeRangePerPlaneScalar;
					resourceCount = AttackClass.planeAmount;

					break;
				case CurrentAttackTab.Recon:
					_rangePerResource = ScenarioEngine.Instance.reconRangePerGuardUnitScalar;
					resourceCount = 3;
					break;
			}

			float cellSize = NodeSize.x * resourceCount * _rangePerResource;
			AnimateRange(new Vector2(cellSize, cellSize));
		}
	}
}