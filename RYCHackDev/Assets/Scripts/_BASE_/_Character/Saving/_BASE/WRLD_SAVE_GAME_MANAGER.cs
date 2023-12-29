using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Deceilio.Psychain
{
    public class WRLD_SAVE_GAME_MANAGER : MonoBehaviour
    {
        public static WRLD_SAVE_GAME_MANAGER instance; // Static object for this script
        public PlayerManager player; // Reference to the Player Manager script

        [Header("SAVE GAME WRITER")]
        SaveGameDataWriter saveGameDataWriter; // Reference to the Save Game Data Writer script

        [Header("CURRENT CHARACTER DATA")]
        // CHARACTER SLOT #
        public CharacterSlot currentCharacterSlotBeingUsed; // Access enum to tell which character slot is currently being used
        public CharacterSaveData currentCharacterSaveData; // Reference to the Character Save Data script
        [SerializeField] private string saveFileName; // File name for the save data

        [Header("SAVE/LOAD")]
        [SerializeField] bool saveGame; // Checks for the save game
        [SerializeField] bool loadGame; // hecks for the load game

        [Header("WORLD SCENE INDEX")]
        [SerializeField] int worldSceneIndex = 1; // Choose which index scene to load 

        [Header("CHARACTER SLOTS")]
        public CharacterSaveData characterSlot01; // 1st character slot for character save data
        public CharacterSaveData characterSlot02; // 2nd character slot for character save data
        public CharacterSaveData characterSlot03; // 3rd character slot for character save data
        public CharacterSaveData characterSlot04; // 4th character slot for character save data
        public CharacterSaveData characterSlot05; // 5th character slot for character save data
        public CharacterSaveData characterSlot06; // 6th character slot for character save data
        public CharacterSaveData characterSlot07; // 7th character slot for character save data
        public CharacterSaveData characterSlot08; // 8th character slot for character save data
        public CharacterSaveData characterSlot09; // 9th character slot for character save data
        public CharacterSaveData characterSlot10; // 10th character slot for character save data

        private void Awake()
        {
            // BELOW CODE: There can be only for instance of this script, If not and more, then destroy the game object
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        private void Start()
        {
            DontDestroyOnLoad(gameObject); // Let you make the GameObject run one time for all the scene as "Don't Destory on Load"
            // BELOW CODE: Load all possible character profiles
            LoadAllCharacterProfiles();
        }
        private void Update()
        {
            if (saveGame) 
            {
                // BELOW CODE: Save the game
                saveGame = false;
                SaveGame();
            }
            else if (loadGame) 
            {
                // BELOW CODE: Load the save data
                loadGame = false;
                LoadGame();
            }
        }
        public string DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot characterSlot)
        {
            string fileName = "";

            switch (characterSlot)
            {
                case CharacterSlot.CharacterSlot_01:
                    fileName = "CharacterSlot_01";
                    break;
                case CharacterSlot.CharacterSlot_02:
                    fileName = "CharacterSlot_02";
                    break;
                case CharacterSlot.CharacterSlot_03:
                    fileName = "CharacterSlot_03";
                    break;
                case CharacterSlot.CharacterSlot_04:
                    fileName = "CharacterSlot_04";
                    break;
                case CharacterSlot.CharacterSlot_05:
                    fileName = "CharacterSlot_05";
                    break;
                case CharacterSlot.CharacterSlot_06:
                    fileName = "CharacterSlot_06";
                    break;
                case CharacterSlot.CharacterSlot_07:
                    fileName = "CharacterSlot_07";
                    break;
                case CharacterSlot.CharacterSlot_08:
                    fileName = "CharacterSlot_08";
                    break;
                case CharacterSlot.CharacterSlot_09:
                    fileName = "CharacterSlot_09";
                    break;
                case CharacterSlot.CharacterSlot_10:
                    fileName = "CharacterSlot_10";
                    break;
                default:
                    break;
            }

            return fileName;
        }

        // BELOW CODE: New game
        public void AttemptToCreateNewGame()
        {
            saveGameDataWriter = new SaveGameDataWriter();
            saveGameDataWriter.saveDataDirectoryPath = Application.persistentDataPath;

            // BELOW CODE: Check to see if we can create a new file
            saveGameDataWriter.dataSaveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_01);

            if (!saveGameDataWriter.CheckToSeeIfFileExists())
            {
                // BELOW CODE: If this profile slot is not taken, Make a new one using this slot
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_01;
                currentCharacterSaveData = new CharacterSaveData();
                StartCoroutine(LoadWorldSceneAsynchronously());
                return;
            }

            // BELOW CODE: Check to see if we can create a new file
            saveGameDataWriter.dataSaveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_02);

            if (!saveGameDataWriter.CheckToSeeIfFileExists())
            {
                // BELOW CODE: If this profile slot is not taken, Make a new one using this slot
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_02;
                currentCharacterSaveData = new CharacterSaveData();
                StartCoroutine(LoadWorldSceneAsynchronously());
                return;
            }

            // BELOW CODE: Check to see if we can create a new file
            saveGameDataWriter.dataSaveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_03);

            if (!saveGameDataWriter.CheckToSeeIfFileExists())
            {
                // BELOW CODE: If this profile slot is not taken, Make a new one using this slot
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_03;
                currentCharacterSaveData = new CharacterSaveData();
                StartCoroutine(LoadWorldSceneAsynchronously());
                return;
            }

            // BELOW CODE: Check to see if we can create a new file
            saveGameDataWriter.dataSaveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_04);

            if (!saveGameDataWriter.CheckToSeeIfFileExists())
            {
                // BELOW CODE: If this profile slot is not taken, Make a new one using this slot
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_04;
                currentCharacterSaveData = new CharacterSaveData();
                StartCoroutine(LoadWorldSceneAsynchronously());
                return;
            }

            // BELOW CODE: Check to see if we can create a new file
            saveGameDataWriter.dataSaveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_05);

            if (!saveGameDataWriter.CheckToSeeIfFileExists())
            {
                // BELOW CODE: If this profile slot is not taken, Make a new one using this slot
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_05;
                currentCharacterSaveData = new CharacterSaveData();
                StartCoroutine(LoadWorldSceneAsynchronously());
                return;
            }

            // BELOW CODE: Check to see if we can create a new file
            saveGameDataWriter.dataSaveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_06);

            if (!saveGameDataWriter.CheckToSeeIfFileExists())
            {
                // BELOW CODE: If this profile slot is not taken, Make a new one using this slot
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_06;
                currentCharacterSaveData = new CharacterSaveData();
                StartCoroutine(LoadWorldSceneAsynchronously());
                return;
            }

            // BELOW CODE: Check to see if we can create a new file
            saveGameDataWriter.dataSaveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_07);

            if (!saveGameDataWriter.CheckToSeeIfFileExists())
            {
                // BELOW CODE: If this profile slot is not taken, Make a new one using this slot
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_07;
                currentCharacterSaveData = new CharacterSaveData();
                StartCoroutine(LoadWorldSceneAsynchronously());
                return;
            }

            // BELOW CODE: Check to see if we can create a new file
            saveGameDataWriter.dataSaveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_08);

            if (!saveGameDataWriter.CheckToSeeIfFileExists())
            {
                // BELOW CODE: If this profile slot is not taken, Make a new one using this slot
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_08;
                currentCharacterSaveData = new CharacterSaveData();
                StartCoroutine(LoadWorldSceneAsynchronously());
                return;
            }

            // BELOW CODE: Check to see if we can create a new file
            saveGameDataWriter.dataSaveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_09);

            if (!saveGameDataWriter.CheckToSeeIfFileExists())
            {
                // BELOW CODE: If this profile slot is not taken, Make a new one using this slot
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_09;
                currentCharacterSaveData = new CharacterSaveData();
                StartCoroutine(LoadWorldSceneAsynchronously());
                return;
            }

            // BELOW CODE: Check to see if we can create a new file
            saveGameDataWriter.dataSaveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_10);

            if (!saveGameDataWriter.CheckToSeeIfFileExists())
            {
                // BELOW CODE: If this profile slot is not taken, Make a new one using this slot
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_10;
                currentCharacterSaveData = new CharacterSaveData();
                StartCoroutine(LoadWorldSceneAsynchronously());
                return;
            }

            // BELOW CODE: If there are no slots, then notify the player
            UI_TitleScreenManager.Instance.DisplayNoFreeCharacterSlotsPopup();
        }
        // BELOW CODE: Save game
        public void SaveGame()
        {
            // BELOW CODE: Save the current file, with a file name depending on which slot we are using
            saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(currentCharacterSlotBeingUsed);

            saveGameDataWriter = new SaveGameDataWriter();
            // BELOW CODE: Generally works on multiple machine types (Application.persistentDataPath)
            saveGameDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            saveGameDataWriter.dataSaveFileName = saveFileName;

            // BELOW CODE: Pass along our characters data to the current save file
            player.SaveCharacterDataToCurrentSaveData(ref currentCharacterSaveData);

            // BELOW CODE: Write the current data to a json file and save it on this device
            saveGameDataWriter.WriteCharacterDataToSaveFile(currentCharacterSaveData);

            Debug.Log("SAVING GAME...." % WRLD_COLORIZE_EDITOR.Cyan % WRLD_FONT_FORMAT.Bold);
            Debug.Log("FILED SAVED AS...." % WRLD_COLORIZE_EDITOR.Cyan % WRLD_FONT_FORMAT.Bold + saveFileName);
        }
        // BELOW CODE: Load game
        public void LoadGame()
        {
            // BELOW CODE: Load a previous file, with a name file depending on which slot we are using
            saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(currentCharacterSlotBeingUsed);

            saveGameDataWriter = new SaveGameDataWriter();
            // BELOW CODE: Generally works on multiple machine types(Application.persistentDataPath)
            saveGameDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            saveGameDataWriter.dataSaveFileName = saveFileName;
            currentCharacterSaveData = saveGameDataWriter.LoadCharacterDataFromJson();

            StartCoroutine(LoadWorldSceneAsynchronously());
        }

        // BELOW CODE: Delete game
        public void DeleteGame(CharacterSlot characterSlot)
        {
            // BELOW CODE: CHOOSE file based on the name
            saveGameDataWriter = new SaveGameDataWriter();
            // BELOW CODE: Generally works on multiple machine types(Application.persistentDataPath)
            saveGameDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            saveGameDataWriter.dataSaveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

            saveGameDataWriter.DeleteSaveFile();
        }

        // BELOW CODE: Load all character profiles on device when starting a game
        private void LoadAllCharacterProfiles()
        {
            saveGameDataWriter = new SaveGameDataWriter();
            saveGameDataWriter.saveDataDirectoryPath = Application.persistentDataPath;

            saveGameDataWriter.dataSaveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_01);
            characterSlot01 = saveGameDataWriter.LoadCharacterDataFromJson();

            saveGameDataWriter.dataSaveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_02);
            characterSlot02 = saveGameDataWriter.LoadCharacterDataFromJson();

            saveGameDataWriter.dataSaveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_03);
            characterSlot03 = saveGameDataWriter.LoadCharacterDataFromJson();

            saveGameDataWriter.dataSaveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_04);
            characterSlot04 = saveGameDataWriter.LoadCharacterDataFromJson();

            saveGameDataWriter.dataSaveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_05);
            characterSlot05 = saveGameDataWriter.LoadCharacterDataFromJson();

            saveGameDataWriter.dataSaveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_06);
            characterSlot06 = saveGameDataWriter.LoadCharacterDataFromJson();

            saveGameDataWriter.dataSaveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_07);
            characterSlot07 = saveGameDataWriter.LoadCharacterDataFromJson();

            saveGameDataWriter.dataSaveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_08);
            characterSlot08 = saveGameDataWriter.LoadCharacterDataFromJson();

            saveGameDataWriter.dataSaveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_09);
            characterSlot09 = saveGameDataWriter.LoadCharacterDataFromJson();

            saveGameDataWriter.dataSaveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_10);
            characterSlot10 = saveGameDataWriter.LoadCharacterDataFromJson();
        }
        private IEnumerator LoadWorldSceneAsynchronously()
        {
            if(player == null)
            {
                player = FindObjectOfType<PlayerManager>();
            }

            //AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);

            // BELOW CODE: If we want to use different scenes for levels in our project
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(currentCharacterSaveData.sceneIndex);

            while (!loadOperation.isDone)
            {
                float loadingProgress = Mathf.Clamp01(loadOperation.progress / 0.9f);
                // TO-DO: Enable a loading screen & pass a loading progress to a slider/loading effect
                yield return null;
            }

            player.LoadCharacterDataFromCurrentCharacterSaveData(ref currentCharacterSaveData);
        }
        public int GetWorldSceneIndex()
        {
            return worldSceneIndex;
        }
    }
}
