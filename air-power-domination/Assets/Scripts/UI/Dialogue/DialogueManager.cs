using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GameNet
{
    public class DialogueManager : MonoBehaviour
    {
        private int index = 0;
        private bool dialoguePlaying = false;
        private bool isSkippingText = false;

        [Header("Dialouge Details")]
        public DialogueData[] dialogues;
        public float DelayPerText; //Sets the delay of the text 
        public float dialogueCompleteDuration;

        [Header("UI")]
        public TextMeshProUGUI TextDisplay;
        public GameObject DialogueCanvas;
        public GameObject ContinueButton;

        public Image InfoTarget;
        public Sprite[] Images;

        public static DialogueManager Instance;
        private void Awake() {
           Instance = this;
    }

        // Start is called before the first frame update
        void Start()
        {
            StartDialogue();
        }

        // Update is called once per frame
        void Update()
        {
            return;
            
            if (TextDisplay.text == dialogues[index].text) {
                ContinueButton.SetActive(true); //Display Continue Button After Dialogue has finished;
            }

            if(Input.GetMouseButtonDown(0)) {
                Debug.Log(dialoguePlaying + " dialogue system" );
                if(dialoguePlaying) {
                    SkipAnimationType();

                }
            }
        }

        IEnumerator Type() {

            dialogues[index].LoadDialogue(); //Invoke Any Functions if exists
            InfoTarget.sprite = Images[dialogues[index].ImageIndex]; //load the index image in question

            float delay = dialogueCompleteDuration * Time.fixedDeltaTime;


            foreach (char letter in dialogues[index].text.ToCharArray()) {
                if (!isSkippingText) {
                    TextDisplay.text += letter; //Display the Message
                    yield return new WaitForSecondsRealtime(delay);
                }
            }

            ContinueButton.SetActive(true);

        }

        public void SetIndex(int val) {
            index = val;
        }

        public void NextIndex() {
            index++;
        }

        public void StartDialogue() {

            //Startup the systemmmm
            dialoguePlaying = true;
            //AdvancedCameraController.Instance.isEnabled = false;
            isSkippingText = false;
            ContinueButton.SetActive(false);
            InfoTarget.sprite = Images[0]; //Set the Image to blank on start;
            TextDisplay.text = "";
            DialogueCanvas.SetActive(true);
            StartCoroutine(Type());
        }

        public void SkipAnimationType() {
            isSkippingText = true;
            TextDisplay.text = "";
            TextDisplay.text = dialogues[index].text;
        }

        public void NextSentence() {

            if (dialogues[index].disableOnContinuePress) { 
                Close();
                return; 
            }
        
            ContinueButton.SetActive(false);

            if (index < dialogues.Length - 1) { //Check if dialogue is there 
                index++;
                TextDisplay.text = " ";
                isSkippingText = false;
                StartDialogue(); 
            }
            else {
                Close();
            }
        }

        public void Close() {
            dialoguePlaying = false;
            //AdvancedCameraController.Instance.isEnabled = true;
            DialogueCanvas.SetActive(false);
            //AdvancedWaveSpawnner.Instance.callOnce = false; //RESET the callonce
            TextDisplay.text = "";
        }

        public void Skip() {
            //ContinueButton.SetActive(false);
            DialogueCanvas.SetActive(false);
            TextDisplay.text = "";
        }
    }
}
