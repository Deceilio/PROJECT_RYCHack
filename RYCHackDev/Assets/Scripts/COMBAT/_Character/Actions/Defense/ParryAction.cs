using UnityEngine;

namespace Deceilio.Psychain
{
    [CreateAssetMenu(menuName = "Psychain/Item Actions/Defense/Parry Action")]
    public class ParryAction : WRLD_ITEM_ACTION
    {
        public override void PerformAction(CharacterManager character)
        {
            if(character.isPerformingAction)
                return;

            // TO-DO: character.characterAnimatorManager.EraseHandIKForWeapon();

            WRLD_WEAPON_ITEM parryingWeapon = character.characterInventoryManager.currentItemBeingUsed as WRLD_WEAPON_ITEM;

            // BELOW CODE: Check if parrying weapom is a fast parry weapon or a medium parry weapon
            if(parryingWeapon.weaponType == WeaponType.SmallShield)
            {
                // BELOW CODE: Fast parry weapon
                character.characterAnimatorManager.PlayTargetActionAnimation(character.characterCombatManager.shield_parry, true);
            }
            else if(parryingWeapon.weaponType != WeaponType.Shield)
            {
                // BELOW CODE: Normal parry weapon
                character.characterAnimatorManager.PlayTargetActionAnimation(character.characterCombatManager.shield_parry, true);
            }
        }
    }
}