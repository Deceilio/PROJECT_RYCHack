using UnityEngine;
using TMPro;

namespace Deceilio.Psychain
{
    public class WRLD_CONSOLE_MANAGER : MonoBehaviour
    {
        [Header("CONSOLE")]
        public GameObject consolePanel; // Reference to the Console GameObject
        public TMP_InputField consoleTextField; // Reference to the input field for the console
        public string commandText; // String value for the console command text 

        [Header("COMMANDS")]
        public string buildNixCommand = "build nix"; // String value for the build nix command
        public string enableCapeCommand = "enable cape"; // String value for the enable cape command
        public string disableCapeCommand = "disable cape"; // String value for the disable cape command
        public string rechargeSkillPoints = "recharge skill"; // String value for recharging the skill points
        public string disableUI = "disable ui"; // String value for disabling the ui
        public string enableFogWall = "enable fog wall";  // String value for enabling fog wall
        public string enableSaveMenu = "save game"; // String value for enabling save menu
        //public string restartSceneCommand = "restart scene"; // String value for the restart scene command

        [Header("PLAYER DATA")]
        public Animator playerAnimator; // Reference to the animator used by the player
        public Avatar nixAvatar; // Reference to the nix avatar for the animator
        public GameObject nixCape; // Reference to the nix Cape Game Object
        public GameObject playerUIManager; // Reference to the Player UI Game Object

        [Header("CHARACTER GFX")]
        public GameObject nixCharacterGFX; // Reference to the Nix Character GFX 

        [Header("FOG WALL")]
        public GameObject fogWall; // Reference to the Fog Wall Game Object

        [Header("CONSOLE LOGS")]
        public TextMeshProUGUI debugErrorLog; // Reference to the debug error log text
        public TextMeshProUGUI debugWarningLog; // Reference to the debug warning log
        public TextMeshProUGUI debugNormalLog; // Reference to the debug normal Log

        [Header("MENU OBJECTS")]
        public GameObject saveMenuManager; // Reference to the save menu manager object
        private void Start()
        {  
            consoleTextField.onValueChanged.AddListener(delegate {GetConsoleCommand(); });
        }
        private void Update()
        {
            OpenConsole();

            if(consolePanel.activeSelf)
            {
                if(Input.GetKeyDown(KeyCode.C))
                {
                    consoleTextField.ActivateInputField();
                }
                if(Input.GetKeyDown(KeyCode.Escape))
                {
                    CloseConsole(); 
                }
            }
        }
        public void OpenConsole()
        {
            if(Input.GetKeyDown(KeyCode.C))
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                consolePanel.SetActive(true);
            }
        }
        public void CloseConsole()
        {
            consoleTextField.text = ""; 
            consolePanel.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        private void GetConsoleCommand()
        {
            commandText = consoleTextField.text;
            if(commandText == buildNixCommand)
            {
                if(playerAnimator.avatar == nixAvatar)
                {
                    debugNormalLog.text = ""; 
                    debugErrorLog.text = "";
                    debugWarningLog.text = "Can't change the player because it's already in the scene.";                    
                }
                else
                {
                    //BELOW CODE: Spawn the Nix GFX
                    nixCharacterGFX.SetActive(true);

                    // BELOW CODE: Change the player's animator avatar
                    playerAnimator.avatar = nixAvatar;
                    debugErrorLog.text = "";
                    debugWarningLog.text = "";
                    debugNormalLog.text = "Changes added! Player switched to Nix.";
                }
            }
            else if(commandText == disableCapeCommand)
            {
                if(playerAnimator.avatar == nixAvatar)
                {
                    if(nixCape.activeSelf)
                    {
                        nixCape.SetActive(false);
                        debugWarningLog.text = ""; 
                        debugErrorLog.text = "";
                        debugNormalLog.text = "The Nix's cape character is removed!";                    
                    } 
                    else if(!nixCape.activeSelf)
                    {
                        debugNormalLog.text = ""; 
                        debugErrorLog.text = "";
                        debugWarningLog.text = "Can't disable the cape as it's already disabled.";                    
                    }
                }
            }
            else if(commandText == enableCapeCommand)
            {
                if(playerAnimator.avatar == nixAvatar)
                { 
                    if(!nixCape.activeSelf)
                    {
                        nixCape.SetActive(true);
                        debugWarningLog.text = ""; 
                        debugErrorLog.text = "";
                        debugNormalLog.text = "The Nix's cape is enabled!";                    
                    } 
                    else if(nixCape.activeSelf)
                    {
                        debugNormalLog.text = ""; 
                        debugErrorLog.text = "";
                        debugWarningLog.text = "Can't enable the cape as it's already enabled.";                    
                    }
                }
            }
            else if(commandText == rechargeSkillPoints)
            {
                PlayerManager player = FindObjectOfType<PlayerManager>();
                UI_SkillsPointBar skillBar = FindObjectOfType<UI_SkillsPointBar>();
                if(player.playerStatsManager.currentSkillPoints == 100)
                {
                    debugNormalLog.text = ""; 
                    debugErrorLog.text = "";
                    debugWarningLog.text = "Can't recharge skill points, as it's already maxed out!"; 
                }
                else 
                {
                    skillBar.SetCurrentSkillPoints(player.playerStatsManager.maxSkillPoints);
                    player.playerStatsManager.currentSkillPoints = player.playerStatsManager.maxSkillPoints;
                    debugWarningLog.text = ""; 
                    debugErrorLog.text = "";
                    debugNormalLog.text = "The skill points are now maxed out!";
                }           
            }
            else if(commandText == disableUI)
            {
                if(!playerUIManager.activeSelf)
                {
                    debugNormalLog.text = ""; 
                    debugErrorLog.text = "";
                    debugWarningLog.text = "Can't disable UI as it's already disabled"; 
                }
                else 
                {
                    playerUIManager.SetActive(false);
                    debugWarningLog.text = ""; 
                    debugErrorLog.text = "";
                    debugNormalLog.text = "The UI is disabled now!";
                }           
            }
            else if(commandText == enableFogWall)
            {
                if(fogWall.activeSelf)
                {
                    debugNormalLog.text = ""; 
                    debugErrorLog.text = "";
                    debugWarningLog.text = "Can't enable fog wall as it's already enabled"; 
                }
                else 
                {
                    fogWall.SetActive(true);
                    debugWarningLog.text = ""; 
                    debugErrorLog.text = "";
                    debugNormalLog.text = "The fog wall is enabled now!";
                }           
            }
            else if(commandText == enableSaveMenu)
            {
                if(saveMenuManager.activeSelf)
                {
                    debugNormalLog.text = ""; 
                    debugErrorLog.text = "";
                    debugWarningLog.text = "Can't enable save menu as it's already enabled"; 
                }
                else 
                {
                    saveMenuManager.SetActive(true);
                    debugWarningLog.text = ""; 
                    debugErrorLog.text = "";
                    debugNormalLog.text = "The save menu enabled now!";
                }           
            }
            else
            {
                debugErrorLog.text = "";
                debugWarningLog.text = "";
                debugNormalLog.text = "";
            }

            for(int i = 0; i < commandText.Length; i++)
            {
                if(commandText[i] >= 'A' && commandText[i] <= 'Z')
                {
                    debugNormalLog.text = "";
                    debugWarningLog.text = "";
                    debugErrorLog.text = "All commands will work on lowercase. Uppercase commands are not allowed.";
                }
            }
        }
    }
}