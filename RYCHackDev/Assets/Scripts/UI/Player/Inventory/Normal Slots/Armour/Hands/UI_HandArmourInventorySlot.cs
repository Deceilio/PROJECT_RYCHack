using UnityEngine;
using UnityEngine.UI;

namespace Deceilio.Psychain
{
    public class UI_HandArmourInventorySlot : MonoBehaviour
    {
        PlayerManager player; //Reference to the Player Manager script

        public Image icon; // Slot icon
        HandArmour item; // Reference to armour item 
        private void Awake()
        {
            player = FindObjectOfType<PlayerManager>();
        }
        public void AddItem(HandArmour newItem)
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
            if (player.playerUIManager.handArmourSlotSelected)
            {
                // BELOW CODE: Add the current equipped hand armour (if any) to our hand inventory
                if (player.playerEquipmentManager.currentHandArmour != null)
                {
                    player.playerEquipmentManager.handArmourInventory.Add(player.playerEquipmentManager.currentHandArmour);
                }
                // BELOW CODE: Remove the current equipped hand armour and replace it with our new hand armour
                player.playerEquipmentManager.currentHandArmour = item;
                // BELOW CODE: Remove our newly equipped hand armour from our inventory
                player.playerEquipmentManager.handArmourInventory.Remove(item);
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