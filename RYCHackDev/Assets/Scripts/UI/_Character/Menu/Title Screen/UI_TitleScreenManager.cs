using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Deceilio.Psychain
{
    public class UI_TitleScreenManager : MonoBehaviour
    {
        // LOGIC: First it will show press any key text
        // LOGIC: We will wait for any input for any key 
        // LOGIC: Then we enable all the content object

        public static UI_TitleScreenManager Instance; //Static Instance for Title Screen Manager

        [Header("MENUS")]
        [SerializeField] GameObject titleScreenMainMenu; //Reference to Title Screen Main Menu Game Object
        [SerializeField] GameObject titleScreenLoadMenu; //Reference to Title Screen Load Menu Game Object

        [Header("MENU FLAGS")]
        [SerializeField] bool isStartKeyPressed = false; // Checks if press any key is pressed or not

        [Header("BUTTONS")]
        [SerializeField] Button mainMenuNewGameButton; // New Game Button to start new game
        [SerializeField] Button mainMenuLoadGameButton; // Load Game Button to open loading screen
        [SerializeField] Button loadMenuReturnButton; // Return Button to return to title screen
        [SerializeField] Button deleteCharacterPopUpConfirmButton; // Delete Character Button to confirm the Popup

        [Header("POP UPS")]
        [SerializeField] GameObject noCharacterSlotsPopUp; // Reference to No Character Slots Pop Ups GameObject
        [SerializeField] Button noCharacterSlotsOkayButton; // Reference to No Character Slots Okay Button GameObject
        [SerializeField] GameObject deleteCharacterSlotPopUp; // Reference to Delete Character Slot Pop Up Game Object

        [Header("CHARACTER SLOTS")]
        public CharacterSlot currentSelectedSlot = CharacterSlot.NO_SLOT; // Reference to Current Selected Slot

        [Header("CONTENT OBJECTS")]
        public GameObject contentObject; // Reference to the Content Objects
        public GameObject splashScreenObject; // Reference to the splash screen object

        [Header("TEXT OBJECTS")]
        public TextMeshProUGUI pressAnyKeyText; // Reference to the press any key text 
        private void Start()
        {
            contentObject.SetActive(false);
        }
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        private void Update()
        {
            // BELOW CODE: Check if the alpha value of the text color is 255
            if (pressAnyKeyText.color.a == 1.0f)
            {
                if (!isStartKeyPressed)
                {
                    OpenContentPanel();
                }
            }
        }
        private void OpenContentPanel()
        {
            if(!splashScreenObject.activeSelf)
            {
                // BELOW CODE: Check if any key or mouse button is pressed
                if (Input.anyKeyDown)
                {
                    isStartKeyPressed = true;

                    // BELOW CODE: Change the alpha value of the text color to 0
                    Color newColor = pressAnyKeyText.color;
                    newColor.a = 0f;
                    pressAnyKeyText.color = newColor;
                    contentObject.SetActive(true);
                }
            }
        }
        public void StartNewGame()
        {
            WRLD_SAVE_GAME_MANAGER.instance.AttemptToCreateNewGame();
        }
        public void OpenLoadGameMenu()
        {
            // BELOW CODE: Close main menu
            titleScreenMainMenu.SetActive(false);
            // BELOW CODE: Open load menu
            titleScreenLoadMenu.SetActive(true);
            // BELOW CODE: Select the return button
            loadMenuReturnButton.Select();
        }
        public void CloseLoadGameMenu()
        {
            // BELOW CODE: CLOSE load menu
            titleScreenLoadMenu.SetActive(false);
            // BELOW CODE: OPEN main menu
            titleScreenMainMenu.SetActive(true);

            // BELOW CODE: SELECT the load button
            mainMenuLoadGameButton.Select();
        }
        public void DisplayNoFreeCharacterSlotsPopup()
        {
            noCharacterSlotsPopUp.SetActive(true);
            noCharacterSlotsOkayButton.Select();
        }
        public void CloseNoFreeCharacterSlotsPopup()
        {
            noCharacterSlotsPopUp.SetActive(false);
            mainMenuNewGameButton.Select();
        }
        public void SelectCharacterSlot(CharacterSlot characterSlot)
        {
            currentSelectedSlot = characterSlot;
        }
        public void SelectNoSlot()
        {
            currentSelectedSlot = CharacterSlot.NO_SLOT;
        }
        public void AttemptToDeleteCharacterSlot()
        {
            if (currentSelectedSlot != CharacterSlot.NO_SLOT)
            {
                deleteCharacterSlotPopUp.SetActive(true);
                deleteCharacterPopUpConfirmButton.Select();
            }
        }
        public void DeleteCharacterSlot()
        {
            deleteCharacterSlotPopUp.SetActive(false);
            WRLD_SAVE_GAME_MANAGER.instance.DeleteGame(currentSelectedSlot);

            // BELOW CODE: We disable and then enable load menu, to refresh all of t he character slots
            titleScreenLoadMenu.SetActive(false);
            titleScreenLoadMenu.SetActive(true);

            loadMenuReturnButton.Select();
        }
        public void CloseDeleteCharacterPopUp()
        {
            deleteCharacterSlotPopUp.SetActive(false);
            loadMenuReturnButton.Select();
        }
    }
}
