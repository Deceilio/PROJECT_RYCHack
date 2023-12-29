using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Deceilio.Psychain
{
    public class PlayerInventoryManager : CharacterInventoryManager
    {
        [Header("WEAPON INVENTORY")]
        public List<WRLD_WEAPON_ITEM> weaponInventory; // List of weapon items in a weapon inventory

        [Header("SUB-WEAPON INVENTORY")]
        [SerializeField] private List<WRLD_WEAPON_ITEM> rightHandedWeapons = new List<WRLD_WEAPON_ITEM>(); // List of right handed weapon items from weapon inventory
        [SerializeField] private List<WRLD_WEAPON_ITEM> leftHandedWeapons = new List<WRLD_WEAPON_ITEM>(); // List of left handed weapon items from weapon inventory
        [SerializeField] private List<WRLD_WEAPON_ITEM> eitherHandedWeapons = new List<WRLD_WEAPON_ITEM>(); // List of right handed weapon items from weapon inventory

        // BELOW CODE: Call this method to filter weapons based on weaponHoldingType
        public void FilterWeaponsByHoldingType()
        {
            rightHandedWeapons.Clear();
            leftHandedWeapons.Clear();
            eitherHandedWeapons.Clear();

            foreach (var weapon in weaponInventory)
            {
                switch (weapon.weaponHoldingType)
                {
                    case WeaponHoldingType.RightHanded:
                        rightHandedWeapons.Add(weapon);
                        break;

                    case WeaponHoldingType.LeftHanded:
                        leftHandedWeapons.Add(weapon);
                        break;

                    case WeaponHoldingType.EitherHanded:
                        eitherHandedWeapons.Add(weapon);
                        break;
                }
            }
        }

        public void ChangeRightWeapon()
        {
            if(character.isGrounded)
            {
                currentRightWeaponIndex = currentRightWeaponIndex + 1;

                if (currentRightWeaponIndex > weaponsInRightHandSlots.Length - 1)
                {
                    currentRightWeaponIndex = -1;
                    rightHandWeapon = character.characterWeaponSlotManager.unArmedWeapon;
                    character.characterWeaponSlotManager.LoadWeaponOnSlot(character.characterWeaponSlotManager.unArmedWeapon, false);
                }
                else if (weaponsInRightHandSlots[currentRightWeaponIndex] != null)
                {
                    rightHandWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
                    character.characterWeaponSlotManager.LoadWeaponOnSlot(weaponsInRightHandSlots[currentRightWeaponIndex], false);
                }
                else
                {
                    currentRightWeaponIndex = currentRightWeaponIndex + 1;
                }
            }
            else
                return;
        }
        public void ChangeLeftWeapon()
        {
            if(character.isGrounded)
            {
                currentLeftWeaponIndex = currentLeftWeaponIndex + 1;

                if (currentLeftWeaponIndex > weaponsInLeftHandSlots.Length - 1)
                {
                    currentLeftWeaponIndex = -1;
                    leftHandWeapon = character.characterWeaponSlotManager.unArmedWeapon;
                    character.characterWeaponSlotManager.LoadWeaponOnSlot(character.characterWeaponSlotManager.unArmedWeapon, true);
                }
                else if (weaponsInLeftHandSlots[currentLeftWeaponIndex] != null)
                {
                    leftHandWeapon = weaponsInLeftHandSlots[currentLeftWeaponIndex];
                    character.characterWeaponSlotManager.LoadWeaponOnSlot(weaponsInLeftHandSlots[currentLeftWeaponIndex], true);
                }
                else
                {
                    currentLeftWeaponIndex = currentLeftWeaponIndex + 1;
                }
            }
            else
                return;
        }
   }
}