using UnityEngine;
using UnityEngine.UI;

namespace Deceilio.Psychain
{
    public class UI_QuickSlots : MonoBehaviour
    {
        [Header("WEAPON SLOTS")]
        public Image leftWeaponIcon; // Left hand switching weapon icon
        public Image rightWeaponIcon; // Right hand switching weapon icon

        [Header("CONSUMABLE ITEM SLOTS")]
        public Image consumableItemSlotIcon; // Image component for the consumable item slot

        [Header("SKILL SLOTS")]
        public Image skillSlotIcon; // Image component for the skill slot

        public void UpdateWeaponQuickSlotUI(bool isLeft, WRLD_WEAPON_ITEM weapon)
        {
            if (isLeft == false)
            {
                if (weapon.itemIcon != null)
                {
                    rightWeaponIcon.sprite = weapon.itemIcon;
                    rightWeaponIcon.enabled = true;
                }
                else
                {
                    rightWeaponIcon.sprite = null;
                    rightWeaponIcon.enabled = false;
                }
            }
            else
            {
                if (weapon.itemIcon != null)
                {
                    leftWeaponIcon.sprite = weapon.itemIcon;
                    leftWeaponIcon.enabled = true;
                }
                else
                {
                    leftWeaponIcon.sprite = null;
                    leftWeaponIcon.enabled = false;
                }
            }
        }

        public void UpdateConsumableItemQuickSlotUI(WRLD_CONSUMABLE_ITEM consumableItem)
        {
            if (consumableItem.itemIcon != null)
            {
                consumableItemSlotIcon.sprite = consumableItem.itemIcon;
                consumableItemSlotIcon.enabled = true;
            }
            else
            {
                consumableItemSlotIcon.sprite = null;
                consumableItemSlotIcon.enabled = false;
            }
        }

        public void UpdateSkillQuickSlotUI(WRLD_SKILL_ITEM skillItem)
        {
            if (skillItem.itemIcon != null)
            {
                skillSlotIcon.sprite = skillItem.itemIcon;
                skillSlotIcon.enabled = true;
            }
            else
            {
                skillSlotIcon.sprite = null;
                skillSlotIcon.enabled = false;
            }
        }
    }
}