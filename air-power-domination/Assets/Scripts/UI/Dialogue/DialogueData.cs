using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace GameNet
{
    [System.Serializable]
    public class DialogueData {

        [Header("General Data")]
        public string Name;
        [TextArea]
        public string text;
        public bool disableOnContinuePress;

        public int ImageIndex = 0;

        [SerializeField]
        private TriggerEvent OnDialogueLoad;
        public TriggerEvent DialogueLoad { get { return OnDialogueLoad; } set { OnDialogueLoad = value; } }

        public void LoadDialogue() {
            OnDialogueLoad?.Invoke();
        }
    }
}

[Serializable]
public class TriggerEvent : UnityEvent {  }
