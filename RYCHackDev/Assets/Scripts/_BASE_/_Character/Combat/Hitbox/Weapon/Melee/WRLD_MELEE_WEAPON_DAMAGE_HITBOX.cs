using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deceilio.Psychain
{
    public class WRLD_MELEE_WEAPON_DAMAGE_HITBOX : WRLD_DAMAGE_HITBOX
    {
        [Header("WEAPON BUFFS DAMAGE")]
        public float physicalBuffDamage; // Physical buff damage for the weapon
        public float fireBuffDamage; // Fire buff damage for the weapon
        public float poiseBuffDamage; // Poise buff damage for the weapon

        protected override void DealDamage(CharacterManager enemyCharacter)
        {
            float finalPhysicalDamage = physicalDamage + physicalBuffDamage;
            float finalFireDamage = fireDamage + fireBuffDamage;

            float finalDamage = 0;

            // BELOW CODE: If we using right hand, we compare the right hand weapon damage modifiers
            if (character.isUsingRightHand)
            {
                // BELOW CODE: Get attack type from attacking character
                if (character.characterCombatManager.currentAttackType == AttackType.Light)
                {
                    // BELOW CODE: Apply damage multipliers
                    finalDamage = finalPhysicalDamage * character.characterInventoryManager.rightHandWeapon.lightAttackDamageModifier;
                    finalDamage += finalFireDamage * character.characterInventoryManager.rightHandWeapon.lightAttackDamageModifier;
                }
                else if (character.characterCombatManager.currentAttackType == AttackType.Heavy)
                {
                    // BELOW CODE: Apply damage multipliers
                    finalDamage = finalPhysicalDamage * character.characterInventoryManager.rightHandWeapon.heavyAttackDamageModifier;
                    finalDamage += finalFireDamage * character.characterInventoryManager.rightHandWeapon.heavyAttackDamageModifier;
                }
            }

            // BELOW CODE: If we using left hand, we compare the left hand weapon damage modifiers
            else if (character.isUsingLeftHand)
            {
                // BELOW CODE: Get attack type from attacking character
                if (character.characterCombatManager.currentAttackType == AttackType.Light)
                {
                    // BELOW CODE: Apply damage multipliers
                    finalPhysicalDamage = finalPhysicalDamage * character.characterInventoryManager.leftHandWeapon.lightAttackDamageModifier;
                    finalDamage += finalFireDamage * character.characterInventoryManager.leftHandWeapon.lightAttackDamageModifier;
                }
                else if (character.characterCombatManager.currentAttackType == AttackType.Heavy)
                {
                    // BELOW CODE: Apply damage multipliers   
                    finalPhysicalDamage = finalPhysicalDamage * character.characterInventoryManager.leftHandWeapon.heavyAttackDamageModifier;
                    finalDamage += finalFireDamage * character.characterInventoryManager.leftHandWeapon.heavyAttackDamageModifier;
                }
            }

            TakeDamageState takeDamageState = Instantiate(WRLD_CHARACTER_STATES_MANAGER.instance.takeDamageState);
            takeDamageState.physicalDamage = finalPhysicalDamage;
            takeDamageState.fireDamage = finalFireDamage;
            takeDamageState.poiseDamage = poiseDamage;
            takeDamageState.contactPoint = contactPoint;
            takeDamageState.angleHitFrom = angleHitFrom;
            enemyCharacter.characterStatesManager.ProcessStateInstantly(takeDamageState);
        }
    }
}