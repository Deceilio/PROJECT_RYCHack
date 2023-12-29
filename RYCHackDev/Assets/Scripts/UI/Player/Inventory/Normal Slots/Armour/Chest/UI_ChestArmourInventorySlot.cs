using UnityEngine;
using UnityEngine.UI;

namespace Deceilio.Psychain
{
    public class UI_ChestArmourInventorySlot : MonoBehaviour
    {
        PlayerManager player; // Reference to the Player Manager script

        public Image icon; // Slot icon
        ChestArmour item; // Reference to armour item 

        private void Awake()
        {
            player = FindObjectOfType<PlayerManager>();
        }
        public void AddItem(ChestArmour newItem)
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
            // BELOW CODE: Remove the current item
            if (player.playerUIManager.chestArmourSlotSelected)
            {
                //BELOW CODE: Add the current equipped chest armour (if any) to our chest inventory
                if (player.playerEquipmentManager.currentChestArmour != null)
                {
                    player.playerEquipmentManager.chestArmourInventory.Add(player.playerEquipmentManager.currentChestArmour);
                }
                // BELOW CODE: Remove the current equipped chest armour and replace it with our new chest armour
                player.playerEquipmentManager.currentChestArmour = item;
                // BELOW CODE: Remove our newly equipped chest armour from our inventory
                player.playerEquipmentManager.chestArmourInventory.Remove(item);
                // BELOW CODE: Load the new armour
                player.playerEquipmentManager.EquipAllArmourModels();
            }
            else // Condition if you didn't selected the equipment slot instead you clicked on inventory, so it should reset
            {
                return;
            }

            // BELOW CODE: Update the new armour to reflect on the ui/armour screen
            player.playerUIManager.equipmentWindowUI.LoadArmourOnEquipmentScreen(player.playerEquipmentManager);
            player.playerUIManager.ResetAllSelectedSlots();
        }
    }
}
