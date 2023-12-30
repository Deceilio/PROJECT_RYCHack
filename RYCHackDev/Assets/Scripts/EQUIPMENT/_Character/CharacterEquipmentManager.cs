using UnityEngine;
using System.Collections.Generic;

namespace Deceilio.Psychain
{
    public class CharacterEquipmentManager : MonoBehaviour
    {
        CharacterManager character; // Reference to the Character Manager script

        [Header("CURRENT ARMOUR")]
        public HeadArmour currentHeadArmour; // Reference to the current head armour
        public ChestArmour currentChestArmour; // Reference to the current chest armour
        public HandArmour currentHandArmour; // Reference to the current hand armour
        public LegArmour currentLegArmour; // Reference to the current leg armour

        [Header("ARMOUR INVENTORY")]
        public List<HeadArmour> headArmourInventory; // List of head armours in head armour inventory
        public List<ChestArmour> chestArmourInventory; // List of chest armours in chest armour inventory
        public List<HandArmour> handArmourInventory; // List of hand armours in hand armour inventory
        public List<LegArmour> legArmourInventory; // List of leg armours in leg armour inventory

        [Header("ARMOUR EQUIPMENT CHANGER")]
        // Head armour
        HeadArmourChanger headArmourChanger; // Reference to the Head Armour Changer script
        // Chest armour
        ChestArmourChanger chestArmourChanger; // Reference to the Chest Armour Changer script
        LeftUpperArmArmourChanger leftUpperArmArmourChanger; // Reference to the Left Upper Arm Armour Changer script
        RightUpperArmArmourChanger rightUpperArmArmourChanger; //Reference to the Right Upper Arm Armour Changer script
        // Leg armour
        HipArmourChanger hipArmourChanger; // Reference to the Hip Armour Changer script
        LeftLegArmourChanger leftLegArmourChanger; // Reference to the Left Leg Armour Changer script
        RightLegArmourChanger rightLegArmourChanger; // Reference to the Right Leg Armour Changer script
        // Hand armour
        LeftForeArmArmourChanger leftForeArmArmourChanger; // Reference to the Left Fore Arm Armour Changer script
        RightForeArmArmourChanger rightForeArmArmourChanger; // Reference to the Right Fore Arm Armour Changer script
        LeftHandArmourChanger leftHandArmourChanger; // Reference to the Left Hand Armour Changer script
        RightHandArmourChanger rightHandArmourChanger; // Reference to the Right Hand Armour Changer script

        [Header("DEFAULT MODELS")]
        // Head armour
        public string defaultHeadArmourModel; // Reference to the default head armour model
        // Chest armour
        public string defaultChestArmourModel; // Reference to the default chest armour model
        public string defaultLeftUpperArmArmourModel; // Reference to the default left upper arm armour model
        public string defaultRightUpperArmArmourModel; // Reference to the default right upper arm armour model
        // Leg armour
        public string defaultHipArmourModel; // Reference to the default hip armour model
        public string defaultLeftLegArmourModel; // Reference to the default left leg armour model
        public string defaultRightLegArmourModel; // Reference to the default right leg armour model
        // Hand armour    
        public string defaultLeftForeArmArmourModel; // Reference to the default left fore arm armour model
        public string defaultRightForeArmArmourModel; // Reference to the default right fore arm armour model
        public string defaultLeftHandArmourModel; // Reference to the default left hand armour model
        public string defaultRightHandArmourModel; // Reference to the default right hand armour model

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();

            // Head armour
            headArmourChanger = GetComponentInChildren<HeadArmourChanger>();
            // Chest armour
            chestArmourChanger = GetComponentInChildren<ChestArmourChanger>();
            leftUpperArmArmourChanger = GetComponentInChildren<LeftUpperArmArmourChanger>();
            rightUpperArmArmourChanger = GetComponentInChildren<RightUpperArmArmourChanger>();
            // Leg armour
            hipArmourChanger = GetComponentInChildren<HipArmourChanger>();
            leftLegArmourChanger = GetComponentInChildren<LeftLegArmourChanger>();
            rightLegArmourChanger = GetComponentInChildren<RightLegArmourChanger>();
            // Hand armour 
            leftForeArmArmourChanger = GetComponentInChildren<LeftForeArmArmourChanger>();
            rightForeArmArmourChanger = GetComponentInChildren<RightForeArmArmourChanger>();
            leftHandArmourChanger = GetComponentInChildren<LeftHandArmourChanger>();
            rightHandArmourChanger = GetComponentInChildren<RightHandArmourChanger>();
        }
        protected virtual void Start()
        {
            EquipAllArmourModels();
        }
        public void EquipAllArmourModels()
        {
            float poisonResistance = 0;
            float totalEquipmentLoad = 0;

            #region Head Armour
            //headArmourChanger.UnEquipAllHeadArmours();

            if (currentHeadArmour != null)
            {
                //headArmourChanger.EquipHeadArmourByName(currentHeadArmour.headArmourName);
                character.characterStatsManager.physicalDamageAbsorptionHead = currentHeadArmour.physicalDefense;
                poisonResistance += currentHeadArmour.poisonResistance;
                totalEquipmentLoad += currentHeadArmour.weight;
                Debug.Log("Head Absorption is " % WRLD_COLORIZE_EDITOR.Green % WRLD_FONT_FORMAT.Bold + character.characterStatsManager.physicalDamageAbsorptionHead + "%" % WRLD_COLORIZE_EDITOR.Green % WRLD_FONT_FORMAT.Bold);
            }
            else
            {
                // BELOW CODE: Equip nothing or default armour (HEAD)
                headArmourChanger.EquipHeadArmourByName(defaultHeadArmourModel);
                character.characterStatsManager.physicalDamageAbsorptionHead = 0;
            }
            #endregion

            #region Chest Armour
            chestArmourChanger.UnEquipAllChestArmours();
            leftUpperArmArmourChanger.UnEquipAllLeftUpperArmArmours();
            rightUpperArmArmourChanger.UnEquipAllRightUpperArmArmours();

            if (currentChestArmour != null)
            {
                //chestArmourChanger.EquipChestArmourByName(currentChestArmour.chestArmourName);
                leftUpperArmArmourChanger.EquipLeftUpperArmArmourByName(currentChestArmour.leftUpperArmModelName);
                rightUpperArmArmourChanger.EquipRightUpperArmArmourByName(currentChestArmour.rightUpperArmModelName);
                character.characterStatsManager.physicalDamageAbsorptionChest = currentChestArmour.physicalDefense;
                poisonResistance += currentChestArmour.poisonResistance;
                totalEquipmentLoad += currentChestArmour.weight;
                Debug.Log("Chest Absorption is " % WRLD_COLORIZE_EDITOR.Green % WRLD_FONT_FORMAT.Bold + character.characterStatsManager.physicalDamageAbsorptionChest + "%" % WRLD_COLORIZE_EDITOR.Green % WRLD_FONT_FORMAT.Bold);
            }
            else
            {
                // BELOW CODE: Equip nothing or default armour (CHEST)
                chestArmourChanger.EquipChestArmourByName(defaultChestArmourModel);
                leftUpperArmArmourChanger.EquipLeftUpperArmArmourByName(defaultLeftUpperArmArmourModel);
                rightUpperArmArmourChanger.EquipRightUpperArmArmourByName(defaultRightUpperArmArmourModel);
                character.characterStatsManager.physicalDamageAbsorptionChest = 0;
            }
            #endregion

            #region Hips & Legs Armour
            hipArmourChanger.UnEquipAllHipArmours();
            leftLegArmourChanger.UnEquipAllLeftLegArmours();
            rightLegArmourChanger.UnEquipAllRightLegArmours();

            if (currentLegArmour != null)
            {
                //hipArmourChanger.EquipHipArmourByName(currentLegArmour.hipModelName);
                leftLegArmourChanger.EquipLeftLegArmourByName(currentLegArmour.leftLegName);
                rightLegArmourChanger.EquipRightLegArmourByName(currentLegArmour.rightLegName);
                character.characterStatsManager.physicalDamageAbsorptionLegs = currentLegArmour.physicalDefense;
                poisonResistance += currentLegArmour.poisonResistance;
                totalEquipmentLoad += currentLegArmour.weight;
                Debug.Log("Legs Absorption are " % WRLD_COLORIZE_EDITOR.Green % WRLD_FONT_FORMAT.Bold + character.characterStatsManager.physicalDamageAbsorptionLegs + "%" % WRLD_COLORIZE_EDITOR.Green % WRLD_FONT_FORMAT.Bold);
            }
            else
            {
                // BELOW CODE: Equip nothing or default armour (HIPS & LEGS)
                hipArmourChanger.EquipHipArmourByName(defaultHipArmourModel);
                leftLegArmourChanger.EquipLeftLegArmourByName(defaultLeftLegArmourModel);
                rightLegArmourChanger.EquipRightLegArmourByName(defaultRightLegArmourModel);
                character.characterStatsManager.physicalDamageAbsorptionLegs = 0;
            }
            #endregion

            #region Hands Armour
            leftForeArmArmourChanger.UnEquipAllLeftForeArmArmours();
            rightForeArmArmourChanger.UnEquipAllRightForeArmArmours();
            leftHandArmourChanger.UnEquipAllLeftHandArmours();
            rightHandArmourChanger.UnEquipAllRightHandArmours();

            if (currentHandArmour != null)
            {
                //leftForeArmArmourChanger.EquipLeftForeArmArmourByName(currentHandArmour.leftForeArmModelName);
                rightForeArmArmourChanger.EquipRightForeArmArmourByName(currentHandArmour.rightForeArmModelName);
                leftHandArmourChanger.EquipLeftHandArmourByName(currentHandArmour.leftHandArmourName);
                rightHandArmourChanger.EquipRightHandArmourByName(currentHandArmour.rightHandArmourName);
                character.characterStatsManager.physicalDamageAbsorptionHands = currentHandArmour.physicalDefense;
                poisonResistance += currentHandArmour.poisonResistance;
                totalEquipmentLoad += currentHandArmour.weight;
                Debug.Log("Hands Absorption are " % WRLD_COLORIZE_EDITOR.Green % WRLD_FONT_FORMAT.Bold + character.characterStatsManager.physicalDamageAbsorptionHands + "%" % WRLD_COLORIZE_EDITOR.Green % WRLD_FONT_FORMAT.Bold);
            }
            else
            {
                // BELOW CODE: Equip nothing or default armour (HANDS)
                //leftForeArmArmourChanger.EquipLeftForeArmArmourByName(defaultLeftForeArmArmourModel);
                rightForeArmArmourChanger.EquipRightForeArmArmourByName(defaultRightForeArmArmourModel);
                leftHandArmourChanger.EquipLeftHandArmourByName(defaultLeftHandArmourModel);
                rightHandArmourChanger.EquipRightHandArmourByName(defaultRightHandArmourModel);
                character.characterStatsManager.physicalDamageAbsorptionHands = 0;
            }
            #endregion
            character.characterStatsManager.poisonResistance = poisonResistance;
            character.characterStatsManager.CalculateAndSetCurrentEquipLoad(totalEquipmentLoad);
        }
    }
}