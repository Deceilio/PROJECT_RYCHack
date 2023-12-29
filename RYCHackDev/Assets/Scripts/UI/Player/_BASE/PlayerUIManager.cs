using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Deceilio.Psychain
{
    public class PlayerUIManager : MonoBehaviour
    {
        public static PlayerUIManager Instance; // Static Instance for Player UI Manager
        PlayerManager player; // Reference to the Player Manager script
        public UI_ItemStatsWindow itemStatsWindowUI; // Reference to the Item Stats Window UI script
        public UI_EquipmentWindow equipmentWindowUI; // Reference to the Equipment Window UI script
        public UI_PopUpManager popUpUIManager; // Reference to the pop up ui manager script

        [Header("UI WINDOWS")]
        public GameObject hudWindow; // Reference to the HUD window game object
        public GameObject selectWindow; // Reference to the select window game object
        public GameObject equipmentScreenWindow; // Reference to the equipment screen window game object
        public GameObject weaponInventoryWindow; // Reference to the weapon inventory window game object
        public GameObject skillScreenWindow; // Reference to the skills screen window game object
        public GameObject itemStatsWindow; // Reference to the item stats window game object
        public GameObject levelUpWindow; // Reference to the level up window
        public GameObject saveMenuWindow; // Reference to the save menu window game object

        [Header("EQUIPMENT WINDOW SLOT SELECTED")]
        public bool rightHandSlot01Selected; // Checks if the right hand slot 1st weapon is selected or not
        public bool rightHandSlot02Selected; // Checks if the right Hand Slot 2nd weapon is selected or not
        public bool leftHandSlot01Selected; // Checks if the left hand slot 1st weapon is selected or not
        public bool leftHandSlot02Selected; // Checks if the left hand slot 2nd weapon is selected or not
        public bool headArmourSlotSelected; // Checks if the head armour is selected or not
        public bool chestArmourSlotSelected; // Checks if the chest armour is selected or not
        public bool handArmourSlotSelected; // Checks if the hand armour is selected or not
        public bool legArmourSlotSelected; // Checks if the leg armour is selected or not

        [Header("WEAPON INVENTORY")]
        public GameObject weaponInventorySlotPrefab; // Prefab for the weapon inventory slot
        public Transform weaponInventorySlotsParent; // Parent slot for the weapon inventory Slot
        [HideInInspector] public UI_WeaponInventorySlot[] weaponInventorySlots; // List of all the slots for weapon inventory

        [Header("HEAD ARMOUR INVENTORY")]
        public GameObject headArmourInventoryWindow; // Reference to the head armour inventory window object
        public GameObject headArmourInventorySlotPrefab; // Prefab for the head armour inventory slot
        public Transform headArmourInventorySlotsParent; // Parent slot for the head armour inventory slot
        UI_HeadArmourInventorySlot[] headArmourInventorySlots; // List of all the slots for head armour inventory

        [Header("CHEST ARMOUR INVENTORY")]
        public GameObject chestArmourInventoryWindow; // Reference to the chest armour inventory window object
        public GameObject chestArmourInventorySlotPrefab; // Prefab for the chest armour inventory slot
        public Transform chestArmourInventorySlotsParent; // Parent slot for the chest armour inventory slot
        UI_ChestArmourInventorySlot[] chestArmourInventorySlots; // List of all the slots for chest armour inventory

        [Header("HAND ARMOUR INVENTORY")]
        public GameObject handArmourInventoryWindow; // Reference to the hand armour inventory window object
        public GameObject handArmourInventorySlotPrefab; // Prefab for the hand armour inventory slot
        public Transform handArmourInventorySlotsParent; // Parent slot for the hand armour inventory slot
        UI_HandArmourInventorySlot[] handArmourInventorySlots; // List of all the slots for hand armour inventory

        [Header("LEG ARMOUR INVENTORY")]
        public GameObject legArmourInventoryWindow; // Reference to the leg armour inventory window object
        public GameObject legArmourInventorySlotPrefab; // Prefab for the leg armour inventory slot
        public Transform legArmourInventorySlotsParent; // Parent slot of the leg armour inventory slot
        UI_LegArmourInventorySlot[] legArmourInventorySlots; // List of all the slots for leg armour inventory

        [Header("BUTTONS")]
        public Button selectInventoryButton; // Reference to the Select Inventory Button script

        [Header("CHARACTER SLOTS")]
        public CharacterSlot currentSelectedSlot = CharacterSlot.NO_SLOT; // Reference to Current Selected

        [Header("POP UPS")]
        [SerializeField] GameObject deleteCharacterSlotPopUp; // Reference to Delete Character Slot Pop Up Game Object
        [SerializeField] Button deleteCharacterPopUpConfirmButton; // Delete Character Button to confirm the Popup

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

            player = FindObjectOfType<PlayerManager>();
            popUpUIManager = GetComponentInChildren<UI_PopUpManager>();

            headArmourInventorySlots = headArmourInventorySlotsParent.GetComponentsInChildren<UI_HeadArmourInventorySlot>();
            chestArmourInventorySlots = chestArmourInventorySlotsParent.GetComponentsInChildren<UI_ChestArmourInventorySlot>();
            handArmourInventorySlots = handArmourInventorySlotsParent.GetComponentsInChildren<UI_HandArmourInventorySlot>();
            legArmourInventorySlots = legArmourInventorySlotsParent.GetComponentsInChildren<UI_LegArmourInventorySlot>();
        }
        private void Start()
        {
            weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<UI_WeaponInventorySlot>();
            equipmentWindowUI.LoadWeaponsOnEquipmentScreen(player.playerInventoryManager); // Need to add more functionality of equipment window yet
        }
        public void UpdateUI()
        {
            #region Weapon Inventory Slots
            for (int i = 0; i < weaponInventorySlots.Length; i++)
            {
                if(i < player.playerInventoryManager.weaponInventory.Count)
                {
                    if(weaponInventorySlots.Length < player.playerInventoryManager.weaponInventory.Count)
                    {
                        Instantiate(weaponInventorySlotPrefab, weaponInventorySlotsParent);
                        weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<UI_WeaponInventorySlot>();
                    }
                    // Inside the loop for weapon inventory slots
                    weaponInventorySlots[i].AddItem(player.playerInventoryManager.weaponInventory[i]);

                }
                else
                {
                    weaponInventorySlots[i].ClearInventorySlot();
                }
            }
            #endregion
            #region Head Inventory Slots
            for (int i = 0; i < headArmourInventorySlots.Length; i++)
            {
                if (i < player.playerEquipmentManager.headArmourInventory.Count)
                {
                    if (headArmourInventorySlots.Length < player.playerEquipmentManager.headArmourInventory.Count)
                    {
                        Instantiate(headArmourInventorySlotsParent, headArmourInventorySlotsParent);
                        headArmourInventorySlots = headArmourInventorySlotsParent.GetComponentsInChildren<UI_HeadArmourInventorySlot>();
                    }
                    headArmourInventorySlots[i].AddItem(player.playerEquipmentManager.headArmourInventory[i]);
                }
                else
                {
                    headArmourInventorySlots[i].ClearInventorySlot();
                }
            }
            #endregion
            #region Chest Inventory Slots
            for (int i = 0; i < chestArmourInventorySlots.Length; i++)
            {
                if (i < player.playerEquipmentManager.chestArmourInventory.Count)
                {
                    if (chestArmourInventorySlots.Length < player.playerEquipmentManager.chestArmourInventory.Count)
                    {
                        Instantiate(chestArmourInventorySlotsParent, chestArmourInventorySlotsParent);
                        chestArmourInventorySlots = chestArmourInventorySlotsParent.GetComponentsInChildren<UI_ChestArmourInventorySlot>();
                    }
                    chestArmourInventorySlots[i].AddItem(player.playerEquipmentManager.chestArmourInventory[i]);
                }
                else
                {
                    chestArmourInventorySlots[i].ClearInventorySlot();
                }
            }
            #endregion
            #region Hand Inventory Slots
            for (int i = 0; i < handArmourInventorySlots.Length; i++)
            {
                if (i < player.playerEquipmentManager.handArmourInventory.Count)
                {
                    if (handArmourInventorySlots.Length < player.playerEquipmentManager.handArmourInventory.Count)
                    {
                        Instantiate(handArmourInventorySlotsParent, handArmourInventorySlotsParent);
                        handArmourInventorySlots = handArmourInventorySlotsParent.GetComponentsInChildren<UI_HandArmourInventorySlot>();
                    }
                    handArmourInventorySlots[i].AddItem(player.playerEquipmentManager.handArmourInventory[i]);
                }
                else
                {
                    handArmourInventorySlots[i].ClearInventorySlot();
                }
            }
            #endregion
            #region Leg Inventory Slots
            for (int i = 0; i < legArmourInventorySlots.Length; i++)
            {
                if (i < player.playerEquipmentManager.legArmourInventory.Count)
                {
                    if (legArmourInventorySlots.Length < player.playerEquipmentManager.legArmourInventory.Count)
                    {
                        Instantiate(legArmourInventorySlotsParent, legArmourInventorySlotsParent);
                        legArmourInventorySlots = legArmourInventorySlotsParent.GetComponentsInChildren<UI_LegArmourInventorySlot>();
                    }
                    legArmourInventorySlots[i].AddItem(player.playerEquipmentManager.legArmourInventory[i]);
                }
                else
                {
                    legArmourInventorySlots[i].ClearInventorySlot();
                }
            }
            #endregion
        }

        public void OpenSelectWindow()
        {
            selectWindow.SetActive(true);
            selectInventoryButton.Select();
        }
        public void CloseSelectWindow()
        {
            selectWindow.SetActive(false);
        }
        public void CloseAllInventoryWindow()
        {
            ResetAllSelectedSlots();
            saveMenuWindow.SetActive(false);
            weaponInventoryWindow.SetActive(false);
            equipmentScreenWindow.SetActive(false);
            skillScreenWindow.SetActive(false);
            itemStatsWindow.SetActive(false);
            headArmourInventoryWindow.SetActive(false);
            chestArmourInventoryWindow.SetActive(false);
            handArmourInventoryWindow.SetActive(false);
            legArmourInventoryWindow.SetActive(false);
        }
        public void ResetAllSelectedSlots()
        {
            rightHandSlot01Selected = false;
            rightHandSlot02Selected = false;
            leftHandSlot01Selected = false;
            leftHandSlot02Selected = false;

            headArmourSlotSelected = false;
            chestArmourSlotSelected = false;
            handArmourSlotSelected = false;
            legArmourSlotSelected = false;
        }

        public void ResetItemStatsWindow()
        {
            itemStatsWindowUI.itemIDText.text = "";
            itemStatsWindowUI.itemNameText.text = "";
            itemStatsWindowUI.itemLoreText.text = "";
            itemStatsWindowUI.itemIconImage.enabled = false;
            itemStatsWindowUI.itemIconImage.sprite = null;
            itemStatsWindowUI.weaponStats.SetActive(false);
            itemStatsWindowUI.armourStats.SetActive(false);
            itemStatsWindowUI.skillStats.SetActive(false);
        }

        public void AttemptToDeleteCharacterSlot()
        {
            if (currentSelectedSlot != CharacterSlot.NO_SLOT)
            {
                deleteCharacterSlotPopUp.SetActive(true);
                deleteCharacterPopUpConfirmButton.Select();
            }
        }

        public void SelectCharacterSlot(CharacterSlot characterSlot)
        {
            currentSelectedSlot = characterSlot;
        }
    }
}