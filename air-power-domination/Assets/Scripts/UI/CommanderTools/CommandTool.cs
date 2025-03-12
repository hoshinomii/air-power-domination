using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace RDP.UI.CommanderTools {
	public class CommandTool : MonoBehaviour {
		public GameObject commandTool;
		public GameObject intWindow;
		public GameObject atkWindow;
		public GameObject point1;
		public GameObject point2;
		public GameObject intLeft;
		public GameObject intRight;
		public GameObject intTab;
		public GameObject atkTab;

		private bool _activate;

		[SerializeField] private float _speed = .3f;
		private CommandWindow CommandWindow => GetComponentInChildren<CommandWindow>();


		public bool Enabled {
			get => _activate;
			set => _activate = value;
		}

		// Start is called before the first frame update
		private void Start() {
			CommandWindow.CurrentTab = CurrentAttackTab.Airstrike;
			_activate = false;
			atkWindow.SetActive(false);
			Configure();
		}

		private void Configure() {
			intLeft.GetComponent<Button>().onClick.AddListener(SlideRight);

			intRight.GetComponent<Button>().onClick.AddListener(SlideLeft);
		}

		private void DisableAllButtons() {
			intRight.SetActive(false);
			intLeft.SetActive(false);
		}

		private void SlideLeft() {
			transform.DOMove(point1.transform.position, _speed).SetEase(Ease.InOutQuad);
			DisableAllButtons();
			intLeft.SetActive(true);
		}

		private void SlideRight() {
			transform.DOMove(point2.transform.position, _speed).SetEase(Ease.InOutQuad);
			DisableAllButtons();
			intRight.SetActive(true);
		}

		public void IntWindow() {
			if (CommandWindow) CommandWindow.rangeOutline.SetActive(false);
			intWindow.SetActive(true);
			atkWindow.SetActive(false);
		}

		public void AtkWindow() {
			if (CommandWindow) CommandWindow.CurrentTab = CurrentAttackTab.Airstrike;
			intWindow.SetActive(false);
			atkWindow.SetActive(true);
		}

		public void AirstrikeOn() {
			CommandWindow.CurrentTab = CurrentAttackTab.Airstrike;
		}

		public void ReconOn() {
			CommandWindow.CurrentTab = CurrentAttackTab.Recon;
		}
	}
}