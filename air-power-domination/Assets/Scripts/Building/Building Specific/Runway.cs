using System.Collections;
using UnityEngine;

namespace RDP.Building.Building_Specific {
	public class Runway : MonoBehaviour {
		[SerializeField] private int planesNeededToTakeOff;
		[SerializeField] private RunwayState state;

		public RunwayState State {
			get => state;
			set => state = value;
		}

		private void OnEnable() {
			State = RunwayState.Free;
		}

		public int PlanesNeededToTakeOff {
			get => planesNeededToTakeOff;
			set => planesNeededToTakeOff = value;
		}

		public void StartPlaneTakeOff(int planesToTakeOff, float deploymentDuration) {
			PlanesNeededToTakeOff = planesToTakeOff;
			State = RunwayState.Deploying;
			StartCoroutine(TakeOff(planesToTakeOff, deploymentDuration));
		}

		// Use this to call the animations;
		private IEnumerator TakeOff(int planesToTakeOff, float deploymentDuration) {
			for (int i = 0; i < planesToTakeOff; i++) {
				// TODO: Call animation here 
				yield return new WaitForSeconds(deploymentDuration);
				planesNeededToTakeOff--;
			}

			State = RunwayState.Free;
		}
	}
}