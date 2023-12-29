using UnityEngine;

namespace Deceilio.Psychain
{
    [CreateAssetMenu(menuName = "Psychain/Items/Weapons/Melee Weapon")]
    public class MeleeWeaponItem : WRLD_WEAPON_ITEM
    {
        [Header("WEAPON BUFF")]
        [Tooltip("Is this weapon can be buffed?")]
        public bool canBeBuffed = true; // Checks if the weapon can be buffed or not
    }
}