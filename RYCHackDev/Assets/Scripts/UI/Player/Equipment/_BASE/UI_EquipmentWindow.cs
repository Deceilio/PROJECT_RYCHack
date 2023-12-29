using UnityEngine;

namespace Deceilio.Psychain
{
    public class UI_EquipmentWindow : MonoBehaviour
    {
        [Header("WEAPONS")]
        public UI_WeaponEquipmentSlot[] weaponEquipmentSlotUI; // List of all the weapon equipment slots

        [Header("ARMOURS")]
        public UI_HeadArmourSlot headArmourSlotUI; // Reference to the head armour slot
        public UI_ChestArmourSlot chestArmourSlotUI; // Reference to the chest armour slot
        public UI_HandArmourSlot handArmourSlotUI; // Reference to the hand armour slot
        public UI_LegArmourSlot legArmourSlotUI; // Reference to the leg armour slot

        public void LoadWeaponsOnEquipmentScreen(PlayerInventoryManager playerInventory)
        {
            for (int i = 0; i < weaponEquipmentSlotUI.Length; i++)
            {
                if (weaponEquipmentSlotUI[i].rightHandSlot01)
                {
                    weaponEquipmentSlotUI[i].AddItem(playerInventory.weaponsInRightHandSlots[0]);
                }
                else if (weaponEquipmentSlotUI[i].rightHandSlot02)
                {
                    weaponEquipmentSlotUI[i].AddItem(playerInventory.weaponsInRightHandSlots[1]);
                }
                else if(weaponEquipmentSlotUI[i].leftHandSlot01)
                {
                    weaponEquipmentSlotUI[i].AddItem(playerInventory.weaponsInLeftHandSlots[0]);
                }
                else
                {
                    weaponEquipmentSlotUI[i].AddItem(playerInventory.weaponsInLeftHandSlots[1]);
                }
            }
        }

        public void LoadArmourOnEquipmentScreen(PlayerEquipmentManager playerEquipment)
        {
            if (playerEquipment.currentHeadArmour != null)
            {
                headArmourSlotUI.AddItem(playerEquipment.currentHeadArmour);
            }
            else
            {
                headArmourSlotUI.ClearItem();
            }

            if (playerEquipment.currentChestArmour != null)
            {
                chestArmourSlotUI.AddItem(playerEquipment.currentChestArmour);
            }
            else
            {
                chestArmourSlotUI.ClearItem();
            }

            if (playerEquipment.currentHandArmour != null)
            {
                handArmourSlotUI.AddItem(playerEquipment.currentHandArmour);
            }
            else
            {
                handArmourSlotUI.ClearItem();
            }

            if (playerEquipment.currentLegArmour != null)
            {
                legArmourSlotUI.AddItem(playerEquipment.currentLegArmour);
            }
            else
            {
                legArmourSlotUI.ClearItem();
            }
        }
    }

}