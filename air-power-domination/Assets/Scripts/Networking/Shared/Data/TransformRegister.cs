using RDP.Networking.Shared.ScriptableObjects;
using UnityEngine;

namespace RDP.Networking.Shared.Data {
	/// <summary>
	/// Class which registers a transform to an associated TransformVariable ScriptableObject.
	/// </summary>
	public class TransformRegister : MonoBehaviour {
		[SerializeField] private TransformVariable m_TransformVariable;

		private void OnEnable() {
			m_TransformVariable.Value = transform;
		}

		private void OnDisable() {
			m_TransformVariable.Value = null;
		}
	}
}