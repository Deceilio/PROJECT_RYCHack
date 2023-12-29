using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deceilio.Psychain
{
    [CreateAssetMenu(menuName = "Psychain/Items/Consumables/Weapon Buff")]
    public class WeaponBuffsConsumableItem : WRLD_CONSUMABLE_ITEM
    {
        // LOGIC: We use an effect here to make it more modular
        // LOGIC: By using an effect as seperate variable, We can re-use this script for multiple buff items
        [Header("EFFECT")]
        [SerializeField] WeaponBuffsState weaponBuffsState; // Reference to the Weapon Buff State script

        [Header("SFX")]
        [SerializeField] AudioClip buffTriggerSound; // When buff is triggered certain audio will play

        public override void AttemptToConsumeItem(CharacterManager character)
        {
            //BELOW CODE: IF PLAYER CAN'T USE THIS ITEM, RETURN WITHOUT DOING ANYTHING
            if (!CanIUseThisItem(character))
                return;

            if (currentItemAmount > 0)
            {
                character.characterAnimatorManager.PlayTargetActionAnimation(consumeAnimation, isPerformingAction);
            }
            else
            {
                character.characterAnimatorManager.PlayTargetActionAnimation("Shrug", true);
            }
        }
        public override void SuccessfullyConsumeItem(CharacterManager character)
        {
            base.SuccessfullyConsumeItem(character);
            character.characterSoundFXManager.PlaySoundFX(buffTriggerSound);

            WeaponBuffsState weaponBuffs = Instantiate(weaponBuffsState);
            weaponBuffs.isRightHandedBuff = true;
            character.characterStatesManager.rightHandWeaponBuffsState = weaponBuffs;
            character.characterStatesManager.ProcessWeaponBuffs();
        }

        public override bool CanIUseThisItem(CharacterManager character)
        {
            if (character.characterStatesManager.currentConsumableBeingUsed.currentItemAmount <= 0)
                return false;

            MeleeWeaponItem meleeWeapon = character.characterInventoryManager.rightHandWeapon as MeleeWeaponItem;

            if(meleeWeapon != null && meleeWeapon.canBeBuffed)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
