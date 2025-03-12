using RDP.Unit_Controls;
using RDP.Unit_Controls.Movement;
using UnityEngine;
using UnityEngine.UI;

namespace RDP.UI.Buildings {
    public class InteractableIcon : MonoBehaviour {
        public GameObject occupiedIconBg;
        public Image occupiedIcon;
        public Button Button => GetComponent<Button>();

        private IconState _state;
        public GameObject unitAssigned;

        public IconState State {
            get => _state;
            set => SetState(value);
        }

        public bool IsOccupied => State == IconState.Occupied;

        private void Awake() {
            State = IconState.Unoccupied;
        }

        private void SetState(IconState value) {
            _state = value;
            switch (value) {
                case IconState.Occupied:
                    Occupied();
                    break;
                case IconState.Unoccupied:
                    Unoccupied();
                    break;
            }
        }

        private void Unoccupied() {
            Button.interactable = false;
            occupiedIconBg.SetActive(false);
        }

        private void Occupied() {
            Button.interactable = true;
            occupiedIconBg.SetActive(true);
        }

        // When a unit successfully interacts with a building, this should be called
        public void Setup(GameObject unit) {

            unitAssigned = unit;
            
            Button.onClick.RemoveAllListeners(); // Remove any previous Listeners attached to the button to prevent any unintended functionality
            Button.onClick.AddListener(() => {
                //When clicked, make the unit uninteract with the building
                unit.GetComponent<UnitMovement>().InteractWithBuilding(false);
            });
            
            Sprite unitIcon = unit.GetComponent<Unit>().data.image;
            occupiedIcon.sprite = unitIcon;
            
            //Set the state of the icon
            State = IconState.Occupied;
        }

        public void RemoveUnit() {
            State = IconState.Unoccupied;
            Button.onClick.RemoveAllListeners(); 
        }
    }
    
    
}