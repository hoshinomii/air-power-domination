using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RDP.UI.UISelector {
	public class TabElement : MonoBehaviour {
		public float scaleDownFactor;
		public float transitionDuration;
		public GameObject bg;
		public Image BgRef => bg.GetComponent<Image>();
		public TextMeshProUGUI tabDisplayText;
		public RectTransform tabRectTransform => GetComponent<RectTransform>();

		public void Configure(float width, float height) {
			BgRef.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
		}

		public void Enable() {
			//tabRectTransform.localScale = Vector3.one;
			tabRectTransform.DOScale(Vector3.one, transitionDuration);
		}

		public void Disable() {
			//tabRectTransform.localScale = new Vector3(scaleDownFactor, scaleDownFactor, scaleDownFactor);
			tabRectTransform.DOScale(scaleDownFactor, transitionDuration);
		}
	}
}