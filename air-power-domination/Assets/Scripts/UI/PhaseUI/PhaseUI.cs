using System;
using System.Collections;
using System.Collections.Generic;
using RDP.Multiplayer;
using RDP.Networking.Shared.Game.Data;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace RDP.UI.PhaseUI
{
    public class PhaseUI : MonoBehaviour {

        
        public static PhaseUI Instance { get; private set; }
        [Header("Phase Header")]
        public GameObject phaseHeaderUI;
        public TextMeshProUGUI currentPhaseText;
        
        [Space] [Header("Phase Animation")]
        public GameObject phaseAnimationUI;
        public Animator phaseAnimator;
        public TextMeshProUGUI phaseText;

        [Space] [Header("Error Handling")]
        public GameObject errorHandlingUI;
        public TextMeshProUGUI errorText;
        public Animator errorHandlingAnimator;
        private static readonly int StartTrigger = Animator.StringToHash("Start");
        private static readonly int EndTrigger = Animator.StringToHash("End");

        private void Awake() {
            Instance = this;
            phaseAnimationUI.SetActive(false);
            errorHandlingUI.SetActive(false);
        }

        // Update is called once per frame
        void Update() {
            ReadPhaseData();
        }

        private void ReadPhaseData() {
            currentPhaseText.text = LevelManager.Instance.State.ToString();
        }

        #region CLIENT & SERVER RPC CALLS
        
        /// <summary>
        /// Call This to show the phase header
        /// </summary>
        /// <param name="message">Message to Output</param>
        /// <param name="duration">How long will it show for</param>
        // public void StartShowPrepMessage(string message = "FUBUKI.. FUBUKI!!! FUBUKI!!!! FUBUKI!!!!", float duration = 1f) {
        //     if (IsServer) {
        //         ShowPreMessageClientRpc(message, duration);
        //     } else {
        //         ShowPrepMessageServerRpc(message, duration);
        //     }
        // }
        
        // public void StartShowErrorMessage( string message = "FUBUKI.. FUBUKI!!! FUBUKI!!!! FUBUKI!!!!", float duration = 1f) {
        //     if (IsServer) {
        //         ShowErrorMessageServerRpc(message, duration);
        //     } else {
        //         ShowErrorMessageClientRpc(message, duration);
        //     }
        // }

        [ClientRpc]
        private void ShowPreMessageClientRpc(string message, float duration) {
            ShowPrepMessage(message, duration);
        }

        [ServerRpc (RequireOwnership = false)]
        private void ShowPrepMessageServerRpc(string message, float duration) {
            ShowPreMessageClientRpc(message, duration);
        }
        
        [ClientRpc]
        private void ShowErrorMessageClientRpc(string message, float duration) {
            ShowPrepMessage(message, duration);
        }

        [ServerRpc (RequireOwnership = false)]
        private void ShowErrorMessageServerRpc(string message, float duration) {
            ShowErrorMessageClientRpc(message, duration);
        }

        #endregion
        
        
        public void ShowErrorMessage(string message = "FUBUKI.. FUBUKI!!! FUBUKI!!!! FUBUKI!!!!", float duration = 1f) {
            Debug.Log($"I was Called Error Handler");
            errorText.text = message;
            StartCoroutine(ErrorMessageCo(message, duration));
        }

        public void ShowPrepMessage(string message = "FUBUKI.. FUBUKI!!! FUBUKI!!!! FUBUKI!!!!", float duration = 1f) {
            phaseText.text = message;
            StartCoroutine(PrepMessageCo(message, duration));
        }

        IEnumerator PrepMessageCo(string message, float duration) {
            phaseAnimationUI.SetActive(true);
            
            phaseAnimator.SetTrigger(StartTrigger);
            yield return new WaitForSecondsRealtime(duration);
            phaseAnimator.SetTrigger(EndTrigger);
            
            yield return new WaitForSecondsRealtime(0.2f); // Let the animation run first
            phaseAnimationUI.SetActive(false); 
        }

        IEnumerator ErrorMessageCo(string message, float duration) {
            errorHandlingUI.SetActive(true);
            
            errorHandlingAnimator.SetTrigger(StartTrigger);
            yield return new WaitForSecondsRealtime(duration);
            errorHandlingAnimator.SetTrigger(EndTrigger);
            
            yield return new WaitForSecondsRealtime(0.2f); // Let the animation run first
            errorHandlingUI.SetActive(false); 
        }
    }
}
