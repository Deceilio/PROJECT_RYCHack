using UnityEngine;
using TMPro;
using Deceilio.Psychain;

namespace Deceilio.Phaedra
{
    public class UI_CharacterSaveSlot : MonoBehaviour
    {
        SaveGameDataWriter saveGameDataWriter; // Reference to the Save File Data Writer script

        [Header("GAME SLOTS")]
        public CharacterSlot characterSlot; // Reference to the character slots enums for accessing character slots

        [Header("CHARACTER INFO")]
        public TextMeshProUGUI characterName; // Text Mesh Pro Text for character name
        public TextMeshProUGUI timePlayed; // Text Mesh Pro Text for time played

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
                    characterName.text = WRLD_SAVE_GAME_MANAGER.instance.characterSlot01.characterName;   
                }
                // BELOW CODE: If it doesn't exist, Disable the game object
                else
                {
                    gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_02)
            {
                saveGameDataWriter.dataSaveFileName = WRLD_SAVE_GAME_MANAGER.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                // BELOW CODE: If file exists, get information from it
                if (saveGameDataWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WRLD_SAVE_GAME_MANAGER.instance.characterSlot02.characterName;
                }
                // BELOW CODE: If it doesn't exist, Disable the game object
                else
                {
                    gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_03)
            {
                saveGameDataWriter.dataSaveFileName = WRLD_SAVE_GAME_MANAGER.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                // BELOW CODE: If file exists, get information from it
                if (saveGameDataWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WRLD_SAVE_GAME_MANAGER.instance.characterSlot03.characterName;
                }
                // BELOW CODE: If it doesn't exist, Disable the game object
                else
                {
                    gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_04)
            {
                saveGameDataWriter.dataSaveFileName = WRLD_SAVE_GAME_MANAGER.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                // BELOW CODE: If file exists, get information from it
                if (saveGameDataWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WRLD_SAVE_GAME_MANAGER.instance.characterSlot04.characterName;
                }
                // BELOW CODE: If it doesn't exist, Disable the game object
                else
                {
                    gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_05)
            {
                saveGameDataWriter.dataSaveFileName = WRLD_SAVE_GAME_MANAGER.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                // BELOW CODE: If file exists, get information from it
                if (saveGameDataWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WRLD_SAVE_GAME_MANAGER.instance.characterSlot05.characterName;
                }
                // BELOW CODE: If it doesn't exist, Disable the game object
                else
                {
                    gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_06)
            {
                saveGameDataWriter.dataSaveFileName = WRLD_SAVE_GAME_MANAGER.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                // BELOW CODE: If file exists, get information from it
                if (saveGameDataWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WRLD_SAVE_GAME_MANAGER.instance.characterSlot06.characterName;
                }
                // BELOW CODE: If it doesn't exist, Disable the game object
                else
                {
                    gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_07)
            {
                saveGameDataWriter.dataSaveFileName = WRLD_SAVE_GAME_MANAGER.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                // BELOW CODE: If file exists, get information from it
                if (saveGameDataWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WRLD_SAVE_GAME_MANAGER.instance.characterSlot07.characterName;
                }
                // BELOW CODE: If it doesn't exist, Disable the game object
                else
                {
                    gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_08)
            {
                saveGameDataWriter.dataSaveFileName = WRLD_SAVE_GAME_MANAGER.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                // BELOW CODE: If file exists, get information from it
                if (saveGameDataWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WRLD_SAVE_GAME_MANAGER.instance.characterSlot08.characterName;
                }
                // BELOW CODE: If it doesn't exist, Disable the game object
                else
                {
                    gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_09)
            {
                saveGameDataWriter.dataSaveFileName = WRLD_SAVE_GAME_MANAGER.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                // BELOW CODE: If file exists, get information from it
                if (saveGameDataWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WRLD_SAVE_GAME_MANAGER.instance.characterSlot09.characterName;
                }
                // BELOW CODE: If it doesn't exist, Disable the game object
                else
                {
                    gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_10)
            {
                saveGameDataWriter.dataSaveFileName = WRLD_SAVE_GAME_MANAGER.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                // BELOW CODE: IIf file exists, get information from it
                if (saveGameDataWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WRLD_SAVE_GAME_MANAGER.instance.characterSlot10.characterName;
                }
                // BELOW CODE: If it doesn't exist, Disable the game object
                else
                {
                    gameObject.SetActive(false);
                }
            }
        }
        public void LoadGameFromCharacterSlot()
        {
            WRLD_SAVE_GAME_MANAGER.instance.currentCharacterSlotBeingUsed = characterSlot;
            WRLD_SAVE_GAME_MANAGER.instance.LoadGame();
        }
        public void SelectCurrentSlot()
        {
            UI_TitleScreenManager.Instance.SelectCharacterSlot(characterSlot);
        }
    }
}