using UnityEngine;
using UnityEngine.UI;

namespace Deceilio.Psychain
{
    public class UI_WeaponInventorySlot : MonoBehaviour
    {
        PlayerManager player; // Reference to the Player Manager script
        public Image icon; // Slot icon for weapon
        public Button weaponSlotButton; // Reference to the weapon slot button

        private WRLD_WEAPON_ITEM item; // Reference to the equipped weapon item

        private void Awake()
        {
            player = FindObjectOfType<PlayerManager>();
        }

        public void AddItem(WRLD_WEAPON_ITEM newItem)
        {
            item = newItem;
            icon.sprite = item.itemIcon;
            icon.enabled = true;
            gameObject.SetActive(true);
        }

        public void ClearInventorySlot()
        {
            item = null;
            icon.sprite = null;
            icon.enabled = false;
            gameObject.SetActive(false);
        }

        public void EquipThisItem()
        {
            // Check if the item is null or not assigned
            if (item == null)
            {
                return;
            }

            // Filter and equip weapons based on their holding type
            switch (item.weaponHoldingType)
            {
                case WeaponHoldingType.RightHanded:
                    if (player.playerUIManager.rightHandSlot01Selected)
                    {
                        EquipItemToSlot(player.playerInventoryManager.weaponsInRightHandSlots, player.playerInventoryManager.currentRightWeaponIndex, false);
                    }
                    else if (player.playerUIManager.rightHandSlot02Selected)
                    {
                        EquipItemToSlot(player.playerInventoryManager.weaponsInRightHandSlots, player.playerInventoryManager.currentRightWeaponIndex + 1, false);
                    }
                    break;

                case WeaponHoldingType.LeftHanded:
                    if (player.playerUIManager.leftHandSlot01Selected)
                    {
                        EquipItemToSlot(player.playerInventoryManager.weaponsInLeftHandSlots, player.playerInventoryManager.currentLeftWeaponIndex, true);
                    }
                    else if (player.playerUIManager.leftHandSlot02Selected)
                    {
                        EquipItemToSlot(player.playerInventoryManager.weaponsInLeftHandSlots, player.playerInventoryManager.currentLeftWeaponIndex + 1, true);
                    }
                    break;

                case WeaponHoldingType.EitherHanded:
                    EquipToEitherHand();
                    break;
            }
        }

        private void EquipToRightHand()
        {
            EquipItemToSlot(player.playerInventoryManager.weaponsInRightHandSlots, player.playerInventoryManager.currentRightWeaponIndex, false);
        }

        private void EquipToLeftHand()
        {
            EquipItemToSlot(player.playerInventoryManager.weaponsInLeftHandSlots, player.playerInventoryManager.currentLeftWeaponIndex, true);
        }

        private void EquipToEitherHand()
        {
            bool itemEquipped = false;

            if (player.playerUIManager.rightHandSlot01Selected)
            {
                itemEquipped = EquipItemToSlot(player.playerInventoryManager.weaponsInRightHandSlots, player.playerInventoryManager.currentRightWeaponIndex, false);
            }
            else if (player.playerUIManager.rightHandSlot02Selected)
            {
                itemEquipped = EquipItemToSlot(player.playerInventoryManager.weaponsInRightHandSlots, player.playerInventoryManager.currentRightWeaponIndex + 1, false);
            }
            else if (player.playerUIManager.leftHandSlot01Selected)
            {
                itemEquipped = EquipItemToSlot(player.playerInventoryManager.weaponsInLeftHandSlots, player.playerInventoryManager.currentLeftWeaponIndex, true);
            }
            else if (player.playerUIManager.leftHandSlot02Selected)
            {
                itemEquipped = EquipItemToSlot(player.playerInventoryManager.weaponsInLeftHandSlots, player.playerInventoryManager.currentLeftWeaponIndex + 1, true);
            }

            // If the item was equipped in any slot, reset other slots
            if (itemEquipped)
            {
                player.playerUIManager.ResetAllSelectedSlots();
            }
        }

        private bool EquipItemToSlot(WRLD_WEAPON_ITEM[] slot, int currentIndex, bool isLeftHand)
        {
            if (slot[currentIndex] != null)
            {
                UnequipItemFromSlot(slot[currentIndex], isLeftHand);
            }

            slot[currentIndex] = item;

            if (isLeftHand)
            {
                player.playerInventoryManager.leftHandWeapon = item;
            }
            else
            {
                player.playerInventoryManager.rightHandWeapon = item;
            }

            player.playerWeaponSlotManager.LoadWeaponOnSlot(item, isLeftHand);
            player.playerUIManager.equipmentWindowUI.LoadWeaponsOnEquipmentScreen(player.playerInventoryManager);

            return true;
        }


        private void UnequipItemFromSlot(WRLD_WEAPON_ITEM itemToUnequip, bool isLeftHand)
        {
            int index = isLeftHand ?
                System.Array.IndexOf(player.playerInventoryManager.weaponsInLeftHandSlots, itemToUnequip) :
                System.Array.IndexOf(player.playerInventoryManager.weaponsInRightHandSlots, itemToUnequip);

            if (index != -1)
            {
                if (isLeftHand)
                {
                    player.playerInventoryManager.weaponsInLeftHandSlots[index] = null;
                }
                else
                {
                    player.playerInventoryManager.weaponsInRightHandSlots[index] = null;
                }
            }
        }
    }
}