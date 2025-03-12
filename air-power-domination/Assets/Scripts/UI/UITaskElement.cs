using System;
using TMPro;
using UnityEngine;

namespace RDP.UI {
	[Serializable]
	public class UITaskElement : MonoBehaviour {
		public TextMeshProUGUI title;
		public TextMeshProUGUI description;
		public TextMeshProUGUI reward;
	}
}