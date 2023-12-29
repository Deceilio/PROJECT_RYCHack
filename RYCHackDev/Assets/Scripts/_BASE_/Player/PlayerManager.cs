using UnityEngine;
using UnityEngine.SceneManagement;

namespace Deceilio.Psychain
{
    public class PlayerManager : CharacterManager
    {
        [HideInInspector] public PlayerCameraManager playerCameraManager; // Reference to the Player Camera Manager script
        [HideInInspector] public PlayerInputManager playerInputManager; // Reference to the Player Input Manager script
        [HideInInspector] public PlayerLocomotionManager playerLocomotionManager; // Reference to the Player Locomotion Manager script
        [HideInInspector] public PlayerAnimatorManager playerAnimatorManager; // Reference to the Player Animator Manager script
        [HideInInspector] public PlayerCombatManager playerCombatManager; // Reference to the Player Combat Manager script
        [HideInInspector] public PlayerInventoryManager playerInventoryManager; // Reference to the Player Inventory Manager script
        [HideInInspector] public PlayerEquipmentManager playerEquipmentManager; // eference to the Player Equipment Manager script
        [HideInInspector] public PlayerStatsManager playerStatsManager; // Reference to the Player Stats Manager script       
        [HideInInspector] public PlayerSkillManager playerSkillManager; // Reference to the Player Skill Manager script
        [HideInInspector] public PlayerStatesManager playerStatesManager; // Reference to the Player States Manager script
        [HideInInspector] public PlayerWeaponSlotManager playerWeaponSlotManager; // Reference to the Player Weapon Slot Manager script
        [HideInInspector] public PlayerSoundFXManager playerSoundFXManager; // Reference to the Player Sound FX Manager script
        [HideInInspector] public PlayerUIManager playerUIManager; // Reference to the Player UI Manager script
        [HideInInspector] public UI_QuickSlots quickSlotsUI; // Reference to the UI_QuickSlots script 
        [HideInInspector] public UI_Interactable interactableUI; // Reference to the UI_Interactable script        
        
        [Header("INTERACTABLES")]
        public GameObject interactableUIGameObject; // Reference to the interactable UI game object
        public GameObject itemInteractableGameObject; // Reference to the item game object which player interacted
        protected override void Awake() // Assigning all the script automatically when the game is load
        {
            base.Awake();
            // Fixed camera issue by using FindObjectOfType instead of CameraHandler.singelton; Hurray!!
            playerCameraManager = FindObjectOfType<PlayerCameraManager>(); 
            playerInputManager = GetComponent<PlayerInputManager>();
            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerSkillManager = GetComponent<PlayerSkillManager>();
            playerStatesManager = GetComponent<PlayerStatesManager>();
            playerCombatManager = GetComponent<PlayerCombatManager>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
            playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
            playerSoundFXManager = GetComponent<PlayerSoundFXManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerUIManager = FindObjectOfType<PlayerUIManager>();
            quickSlotsUI = FindObjectOfType<UI_QuickSlots>();
            interactableUI = FindObjectOfType<UI_Interactable>();

            //WRLD_SAVE_GAME_MANAGER.instance.player = this; Enable after menu of this project is finished
        }
        protected override void Update() // Calling interaction and states, etc in Update
        {
            base.Update();
            playerInventoryManager.FilterWeaponsByHoldingType();
            playerInputManager.UseAllInputs();
            playerLocomotionManager.UseAllMovement();
            playerStatsManager.RegenStamina();
            playerInputManager.CheckForInterctableObject();
        }
        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }  
        public void LateUpdate() // Disabling input in late update
        {    
            playerInputManager.tapYInput = false;
            playerInputManager.inventoryInput = false;

            float delta = Time.fixedDeltaTime;
            
            if (playerCameraManager != null)
            {
                playerCameraManager.FollowTarget(delta);
                playerCameraManager.UseCameraRotation(delta, playerInputManager.mouseX, playerInputManager.mouseY);
            }
        }
        public void OpenChestInteraction(Transform playerStandsHereWhenOpeningChest) // Interaction for chest items
        {
            playerLocomotionManager.rigidBody.velocity = Vector3.zero; // Stops the player from ice skating
            transform.position = playerStandsHereWhenOpeningChest.transform.position;
            playerAnimatorManager.PlayTargetActionAnimation("Open Chest", true);
        }
        public GameObject FindInActiveObjectByName(string name)
        {
            Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
            for (int i = 0; i < objs.Length; i++)
            {
                if (objs[i].hideFlags == HideFlags.None)
                {
                    if (objs[i].name == name)
                    {
                        return objs[i].gameObject;
                    }
                }
            }
            return null;
        }
        public void PassThroughFogWallInteraction(Transform fogWallEntrance)
        {
             playerLocomotionManager.rigidBody.velocity = Vector3.zero; // Stops the player from ice skating

             Vector3 rotationDirection = fogWallEntrance.transform.forward;
             Quaternion turnRotation = Quaternion.LookRotation(rotationDirection);
             transform.rotation = turnRotation; // Rotate over time so it does not look rigid

             playerAnimatorManager.PlayTargetActionAnimation("Pass Through Fog", true, true, true, true);
        }

        public void SaveCharacterDataToCurrentSaveData(ref CharacterSaveData currentCharacterSaveData)
        {
            // BELOW CODE: Saving which scene the player is right now
            currentCharacterSaveData.sceneIndex = SceneManager.GetActiveScene().buildIndex;
            // BELOW CODE: Saving the name of the player
            currentCharacterSaveData.characterName = playerStatsManager.characterName;
            // BELOW CODE: Saving the level of the player
            //currentCharacterSaveData.characterLevel = playerStatsManager.playerLevel;

            // BELOW CODE: Saving position of the player
            currentCharacterSaveData.xPosition = transform.position.x;
            currentCharacterSaveData.yPosition = transform.position.y;
            currentCharacterSaveData.zPosition = transform.position.z;

            // BELOW CODE: Saving weapons of the player
            currentCharacterSaveData.currentRightHandWeaponID = playerInventoryManager.rightHandWeapon.itemID;
            currentCharacterSaveData.currentLeftHandWeaponID = playerInventoryManager.leftHandWeapon.itemID;

            // BELOW CODE: Saving armours of the player
            if (playerEquipmentManager.currentHeadArmour != null)
            {
                currentCharacterSaveData.currentHeadArmourItemId = playerEquipmentManager.currentHeadArmour.itemID;
            }
            else
            {
                currentCharacterSaveData.currentHeadArmourItemId = -1;
            }

            if (playerEquipmentManager.currentChestArmour != null)
            {
                currentCharacterSaveData.currentChestArmourItemId = playerEquipmentManager.currentChestArmour.itemID;
            }
            else
            {
                currentCharacterSaveData.currentChestArmourItemId = -1;
            }

            if (playerEquipmentManager.currentLegArmour != null)
            {
                currentCharacterSaveData.currentLegArmourItemId = playerEquipmentManager.currentLegArmour.itemID;
            }
            else
            {
                currentCharacterSaveData.currentLegArmourItemId = -1;
            }

            if (playerEquipmentManager.currentHandArmour != null)
            {
                currentCharacterSaveData.currentHandArmourItemId = playerEquipmentManager.currentHandArmour.itemID;
            }
            else
            {
                currentCharacterSaveData.currentHandArmourItemId = -1;
            }

        }
        public void LoadCharacterDataFromCurrentCharacterSaveData(ref CharacterSaveData currentCharacterSaveData)
        {
            // BELOW CODE: Loading the scene which player saved on the slot
            currentCharacterSaveData.sceneIndex = SceneManager.GetActiveScene().buildIndex;
            // BELOW CODE: Loading name of the player
            playerStatsManager.characterName = currentCharacterSaveData.characterName;
            // BELOW CODE: Loading level of the player
            //playerStatsManager.playerLevel = currentCharacterSaveData.characterLevel;

            // BELOW CODE: Loading position of the player
            transform.position = new Vector3(currentCharacterSaveData.xPosition, currentCharacterSaveData.yPosition, currentCharacterSaveData.zPosition);

            // BELOW CODE: Loading weapons of the player
            playerInventoryManager.rightHandWeapon = WRLD_ITEM_DATABASE.instance.GetMeleeWeaponItemByID(currentCharacterSaveData.currentRightHandWeaponID);
            playerInventoryManager.leftHandWeapon = WRLD_ITEM_DATABASE.instance.GetMeleeWeaponItemByID(currentCharacterSaveData.currentLeftHandWeaponID);
            playerWeaponSlotManager.LoadBothWeaponsOnSlots();

            // BELOW CODE: Loading armours of the player
            WRLD_ARMOUR_ITEM headArmour = WRLD_ITEM_DATABASE.instance.GetArmourItemByID(currentCharacterSaveData.currentHeadArmourItemId);

            // BELOW CODE: If this item exist in database, we apply it
            if (headArmour != null)
            {
                playerEquipmentManager.currentHeadArmour = headArmour as HeadArmour;
            }

            WRLD_ARMOUR_ITEM chestArmour = WRLD_ITEM_DATABASE.instance.GetArmourItemByID(currentCharacterSaveData.currentChestArmourItemId);

            // BELOW CODE: If this item exist in database, we apply it
            if (chestArmour != null)
            {
                playerEquipmentManager.currentChestArmour = chestArmour as ChestArmour;
            }

            WRLD_ARMOUR_ITEM legArmour = WRLD_ITEM_DATABASE.instance.GetArmourItemByID(currentCharacterSaveData.currentLegArmourItemId);

            // BELOW CODE: If this item exist in database, we apply it
            if (legArmour != null)
            {
                playerEquipmentManager.currentLegArmour = legArmour as LegArmour;
            }

            WRLD_ARMOUR_ITEM handArmour = WRLD_ITEM_DATABASE.instance.GetArmourItemByID(currentCharacterSaveData.currentHandArmourItemId);

            // BELOW CODE: If this item exist in database, we apply it
            if (handArmour != null)
            {
                playerEquipmentManager.currentHandArmour = handArmour as HandArmour;
            }

            playerEquipmentManager.EquipAllArmourModels();
        }
    }
}