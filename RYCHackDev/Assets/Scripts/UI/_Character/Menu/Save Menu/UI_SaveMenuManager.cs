using UnityEngine;
using UnityEngine.UI;

namespace Deceilio.Psychain
{
    public class UI_SaveMenuManager : MonoBehaviour
    {
        PlayerControls playerControls; // Reference to the Player Controls script
        
        [Header("SAVE SCREEN INPUTS")]
        [SerializeField] bool deleteCharacterSlot = false; // Use to delete the character slot

        [Header("RETURN BUTTON")]
        public Button returnButton; // Reference to the return button 

        private void Update()
        {
            if (deleteCharacterSlot)
            {
                deleteCharacterSlot = false;
                PlayerUIManager.Instance.AttemptToDeleteCharacterSlot();
            }
        }

        private void OnEnable()
        {
            returnButton.Select();

            if (playerControls == null)
            {
                playerControls = new PlayerControls();
                playerControls.PlayerUI.X.performed += i => deleteCharacterSlot = true;
            }
            
            playerControls.Enable();
        }

        private void OnDisable()
        {
            playerControls.Disable();
        }
    }
}