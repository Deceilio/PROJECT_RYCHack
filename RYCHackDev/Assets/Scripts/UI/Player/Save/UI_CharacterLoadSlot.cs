using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Deceilio.Psychain
{
    public class UI_CharacterLoadSlot : MonoBehaviour
    {
        SaveGameDataWriter saveGameDataWriter; // Reference to the Save File Data Writer script
        Image loadSlotImage; // Reference to the load slot image

        [Header("GAME SLOTS")]
        public CharacterSlot characterSlot; // Reference to the character slots enums for accessing character slots

        [Header("CHARACTER INFO")]
        public TextMeshProUGUI characterName; // Text Mesh Pro Text for character name
        public TextMeshProUGUI timePlayed; // Text Mesh Pro Text for time played

        private void Awake()
        {
            loadSlotImage = GetComponent<Image>();
        }
        private void OnEnable()
        {
            LoadSaveSlots();
        }
        private void LoadSaveSlots()
        {
            saveGameDataWriter = new SaveGameDataWriter();
            saveGameDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
        
            if(characterSlot == CharacterSlot.CharacterSlot_01)
            {
                saveGameDataWriter.dataSaveFileName = WRLD_SAVE_GAME_MANAGER.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
    
                // BELOW CODE: If file exists, get information from it
                if(saveGameDataWriter.CheckToSeeIfFileExists())
                {
                    // BELOW CODE: Change Image color to red
                    loadSlotImage.color = Color.red;
                    characterName.text = WRLD_SAVE_GAME_MANAGER.instance.characterSlot01.characterName;   
                }
                // BELOW CODE: If it doesn't exist, Disable the game object
                else
                {
                    //gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_02)
            {
                saveGameDataWriter.dataSaveFileName = WRLD_SAVE_GAME_MANAGER.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                // BELOW CODE: If file exists, get information from it
                if (saveGameDataWriter.CheckToSeeIfFileExists())
                {
                    // BELOW CODE: Change Image color to red
                    loadSlotImage.color = Color.red;

                    characterName.text = WRLD_SAVE_GAME_MANAGER.instance.characterSlot02.characterName;
                }
                // BELOW CODE: If it doesn't exist, Disable the game object
                else
                {
                    //gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_03)
            {
                saveGameDataWriter.dataSaveFileName = WRLD_SAVE_GAME_MANAGER.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                // BELOW CODE: If file exists, get information from it
                if (saveGameDataWriter.CheckToSeeIfFileExists())
                {
                    // BELOW CODE: Change Image color to red
                    loadSlotImage.color = Color.red;

                    characterName.text = WRLD_SAVE_GAME_MANAGER.instance.characterSlot03.characterName;
                }
                // BELOW CODE: If it doesn't exist, Disable the game object
                else
                {
                    //gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_04)
            {
                saveGameDataWriter.dataSaveFileName = WRLD_SAVE_GAME_MANAGER.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                // BELOW CODE: If file exists, get information from it
                if (saveGameDataWriter.CheckToSeeIfFileExists())
                {
                    // BELOW CODE: Change Image color to red
                    loadSlotImage.color = Color.red;

                    characterName.text = WRLD_SAVE_GAME_MANAGER.instance.characterSlot04.characterName;
                }
                // BELOW CODE: If it doesn't exist, Disable the game object
                else
                {
                    //gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_05)
            {
                saveGameDataWriter.dataSaveFileName = WRLD_SAVE_GAME_MANAGER.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                // BELOW CODE: If file exists, get information from it
                if (saveGameDataWriter.CheckToSeeIfFileExists())
                {
                    // BELOW CODE: Change Image color to red
                    loadSlotImage.color = Color.red;

                    characterName.text = WRLD_SAVE_GAME_MANAGER.instance.characterSlot05.characterName;
                }
                // BELOW CODE: If it doesn't exist, Disable the game object
                else
                {
                    //gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_06)
            {
                saveGameDataWriter.dataSaveFileName = WRLD_SAVE_GAME_MANAGER.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                // BELOW CODE: If file exists, get information from it
                if (saveGameDataWriter.CheckToSeeIfFileExists())
                {
                    // BELOW CODE: Change Image color to red
                    loadSlotImage.color = Color.red;

                    characterName.text = WRLD_SAVE_GAME_MANAGER.instance.characterSlot06.characterName;
                }
                // BELOW CODE: If it doesn't exist, Disable the game object
                else
                {
                    //gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_07)
            {
                saveGameDataWriter.dataSaveFileName = WRLD_SAVE_GAME_MANAGER.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                // BELOW CODE: If file exists, get information from it
                if (saveGameDataWriter.CheckToSeeIfFileExists())
                {
                    // BELOW CODE: Change Image color to red
                    loadSlotImage.color = Color.red;

                    characterName.text = WRLD_SAVE_GAME_MANAGER.instance.characterSlot07.characterName;
                }
                // BELOW CODE: If it doesn't exist, Disable the game object
                else
                {
                    //gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_08)
            {
                saveGameDataWriter.dataSaveFileName = WRLD_SAVE_GAME_MANAGER.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                // BELOW CODE: If file exists, get information from it
                if (saveGameDataWriter.CheckToSeeIfFileExists())
                {
                    // BELOW CODE: Change Image color to red
                    loadSlotImage.color = Color.red;

                    characterName.text = WRLD_SAVE_GAME_MANAGER.instance.characterSlot08.characterName;
                }
                // BELOW CODE: If it doesn't exist, Disable the game object
                else
                {
                    //gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_09)
            {
                saveGameDataWriter.dataSaveFileName = WRLD_SAVE_GAME_MANAGER.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                // BELOW CODE: If file exists, get information from it
                if (saveGameDataWriter.CheckToSeeIfFileExists())
                {
                    // BELOW CODE: Change Image color to red
                    loadSlotImage.color = Color.red;

                    characterName.text = WRLD_SAVE_GAME_MANAGER.instance.characterSlot09.characterName;
                }
                // BELOW CODE: If it doesn't exist, Disable the game object
                else
                {
                    //gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_10)
            {
                saveGameDataWriter.dataSaveFileName = WRLD_SAVE_GAME_MANAGER.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                // BELOW CODE: IIf file exists, get information from it
                if (saveGameDataWriter.CheckToSeeIfFileExists())
                {
                    // BELOW CODE: Change Image color to red
                    loadSlotImage.color = Color.red;

                    characterName.text = WRLD_SAVE_GAME_MANAGER.instance.characterSlot10.characterName;
                }
                // BELOW CODE: If it doesn't exist, Disable the game object
                else
                {
                    //gameObject.SetActive(false);
                }
            }
        }
        public void SaveGameFromCharacterSlot()
        {
            WRLD_SAVE_GAME_MANAGER.instance.currentCharacterSlotBeingUsed = characterSlot;
            WRLD_SAVE_GAME_MANAGER.instance.SaveGame();
        }
        public void SelectCurrentSlot()
        {
            PlayerUIManager.Instance.SelectCharacterSlot(characterSlot);
        }
    }
}