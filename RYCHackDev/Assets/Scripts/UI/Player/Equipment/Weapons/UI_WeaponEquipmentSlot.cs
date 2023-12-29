using UnityEngine;
using UnityEngine.UI;

namespace Deceilio.Psychain
{
    public class UI_WeaponEquipmentSlot : MonoBehaviour
    {
        PlayerManager player; // Reference to the Player Manager script
        public Image icon; // Icon image of the weapon equipment
        WRLD_WEAPON_ITEM weapon; // Reference to the weapon item script

        [Header("RIGHT HAND SLOTS")]
        public bool rightHandSlot01; // Checks for the right hand slot 01
        public bool rightHandSlot02; // Checks for the right hand slot 02
         
        [Header("LEFT HAND SLOTS")]
        public bool leftHandSlot01;  // Checks for the left hand slot 01
        public bool leftHandSlot02;  // Checks for the left hand slot 02

        private void Awake()
        {
            player = FindObjectOfType<PlayerManager>();
        }
        public void AddItem(WRLD_WEAPON_ITEM newWeapon)
        {
            //if(player.player)
            if(newWeapon != null)
            {
                weapon = newWeapon;
                icon.sprite = weapon.itemIcon;
                icon.enabled = true;
                gameObject.SetActive(true);
            }
            else
            {
                ClearItem();
            }
        }
        public void ClearItem()
        {
            weapon = null;
            icon.sprite = null;
            icon.enabled= false;
            gameObject.SetActive(false);
        }
        public void SelectThisSlot()
        {
            player.playerUIManager.ResetAllSelectedSlots();

            if(rightHandSlot01)
            {
                player.playerUIManager.rightHandSlot01Selected = true;
            }
            else if (rightHandSlot02)
            {
                player.playerUIManager.rightHandSlot02Selected = true;
            }
            else if (leftHandSlot01)
            {
                player.playerUIManager.leftHandSlot01Selected = true;
            }
            else
            {
                player.playerUIManager.leftHandSlot02Selected = true;
            }

            Debug.Log("ITEM UPDATE LESS GOO!!" % WRLD_COLORIZE_EDITOR.Olive % WRLD_FONT_FORMAT.Bold);

            player.playerUIManager.itemStatsWindowUI.UpdateWeaponItemStats(weapon);
        }
    }
}