using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deceilio.Psychain
{
    [CreateAssetMenu(menuName = "Psychain/Character States/Instant States/Take Blocked Damage")]
    public class TakeBlockedDamageState : WRLD_CHARACTER_STATE
    {
        [Header("CHARACTER CAUSING DAMAGE")]
        public CharacterManager characterCausingDamage; // Reference to the Character Manager script to get which character is causing damage

        [Header("DAMAGE")]
        public float physicalDamage = 0; // Physical damage value
        public float fireDamage = 0; // Fire damage value
        public float staminaDamage = 0; // Stamina damage value
        public float poiseDamage = 0; // Poise damage value

        [Header("ANIMATIONS")]
        public string blockAnimation; // Animation used for block

        public override void ProcessState(CharacterManager character)
        {
            // BELOW CODE: If the character is dead, return without any logic
            if (character.isDead)
                return;

            // BELOW CODE: If the character is invulnerable, no damage is taken
            if (character.isInvulerable)
                return;

            // LOGIC: Damage hitbox logic

            // BELOW CODE: Calculate blocked damage after defense
            CalculateDamage(character);
            // BELOW CODE: Calculate stamina damage after defense
            CalculateStaminaDamage(character);
            // BELOW CODE: Check block animation based on the amount of poise damage 
            CheckBlockAnimationBasedOnPoiseDamage(character);
            // BELOW CODE: Play block sfx
            PlayBlockSoundFX(character);
            // BELOW CODE: If the character is an A.I character, assign them as a damage character as a target
            AssignNewAITarget(character);

            if(character.isDead)
            {
                character.characterAnimatorManager.PlayTargetActionAnimation("Dead_01", true);
            }
            else 
            {
                if(character.characterStatsManager.currentStamina <= 0)
                {
                    character.characterAnimatorManager.PlayTargetActionAnimation("Guard Break", true);
                    character.canBeRiposted = true;
                    //character.characterSoundFXManager.PlayGuardBreakFX();
                    character.isBlocking = false;
                }    
                else 
                {
                    character.characterAnimatorManager.PlayTargetActionAnimation(blockAnimation, true);
                    character.isAttacking = false;          
                }
            }
        }

        public void CalculateDamage(CharacterManager character)
        {
            if(characterCausingDamage != null)
            {
                // BELOW CODE: Before calculating damage defense, we check the attacking characters damage modifiers
                physicalDamage = Mathf.RoundToInt(physicalDamage * (characterCausingDamage.characterStatsManager.physicalDamagePercentageModifier / 100));
                fireDamage = Mathf.RoundToInt(fireDamage * (characterCausingDamage.characterStatsManager.fireDamagePercentageModifier / 100));
            }
            
            character.characterAnimatorManager.EraseHandIKForWeapon();

            float totalPhysicalDamageAbsorption = 1 - 
                (1 - character.characterStatsManager.physicalDamageAbsorptionHead / 100) *
                (1 - character.characterStatsManager.physicalDamageAbsorptionChest / 100) *
                (1 - character.characterStatsManager.physicalDamageAbsorptionLegs / 100) *
                (1 - character.characterStatsManager.physicalDamageAbsorptionHands / 100);

            physicalDamage = Mathf.RoundToInt(physicalDamage - (physicalDamage * totalPhysicalDamageAbsorption));

            float totalFireDamageAbsorption = 1 - 
                (1 - character.characterStatsManager.fireDamageAbsorptionHead / 100) *
                (1 - character.characterStatsManager.fireDamageAbsorptionChest / 100) *
                (1 - character.characterStatsManager.fireDamageAbsorptionLegs / 100) *
                (1 - character.characterStatsManager.fireDamageAbsorptionHands / 100);

            fireDamage = Mathf.RoundToInt(fireDamage - (fireDamage * totalFireDamageAbsorption));

            physicalDamage = physicalDamage - Mathf.RoundToInt(physicalDamage * (character.characterStatsManager.physicalAbsorptionPercentageModifier / 100));
            fireDamage = fireDamage - Mathf.RoundToInt(fireDamage * (character.characterStatsManager.fireAbsorptionPercentageModifier / 100));

            Debug.Log("Total" + totalPhysicalDamageAbsorption + "%"); 

            float finalDamage = physicalDamage + fireDamage; // + Magic damage, Dark damage, etc if we need it.

            character.characterStatsManager.currentHealth = Mathf.RoundToInt(character.characterStatsManager.currentHealth - finalDamage);

            Debug.Log("Total Damage Dealth is " + finalDamage);

            if (character.characterStatsManager.currentHealth <= 0)
            {
                character.characterStatsManager.currentHealth = 0;
                character.isDead = true;
            }

            // TO-DO: Play blocking sound effects
        }

        public void CalculateStaminaDamage(CharacterManager character)
        {
            float staminaDamageAbsorption = staminaDamage * (character.characterStatsManager.blockingStabilityRating / 100);
            float staminaDamageAfterAbsorption = staminaDamage - staminaDamageAbsorption;
            character.characterStatsManager.currentStamina -= staminaDamageAfterAbsorption;
        }

        public void CheckBlockAnimationBasedOnPoiseDamage(CharacterManager character)
        {
            // BELOW CODE: One handed block animation
            if(!character.isTwoHandingWeapon)
            {
                // BELOW CODE: Poise bracket < 25     -  Small damage
                if (poiseDamage <= 24 && poiseDamage >= 0)
                {
                    blockAnimation = "Block_01"; // OH_Block_Guard_Ping_01;
                    return;
                }
                // BELOW CODE: Poise bracket > 25 < 50 - Medium damage
                else if (poiseDamage <= 49 && poiseDamage >= 25)
                {
                    // TO-DO: Change the animation when you add different type of animation type like medium, large and colosaal
                    blockAnimation = "Block_01"; // OH_Block_Guard_Light_01;
                    return;
                }
                // BELOW CODE: Poise bracket > 50 < 75 - Large damage
                else if (poiseDamage <= 74 && poiseDamage >= 50)
                {
                    // TO-DO: Change the animation when you add different type of animation type like medium, large and colosaal
                    blockAnimation = "Block_01"; // OH_Block_Guard_Medium_01;
                    return;
                }
                // BELOW CODE: Poise bracket > 75   -    Colosaal damage
                else if (poiseDamage >= 75)
                {
                    // TO-DO: Change the animation when you add different type of animation type like medium, large and colosaal
                    blockAnimation = "Block_01"; // OH_Block_Guard_Heavy_01;
                    return;
                }
            }
            // BELOW CODE: Two handed block animation
            else 
            {
                // BELOW CODE: Poise bracket < 25     -  Small damage
                if (poiseDamage <= 24 && poiseDamage >= 0)
                {
                    blockAnimation = "Block_01"; // TH_Block_Guard_Ping_01;
                    return;
                }
                // BELOW CODE: Poise bracket > 25 < 50 - Medium damage
                else if (poiseDamage <= 49 && poiseDamage >= 25)
                {
                    // TO-DO: Change the animation when you add different type of animation type like medium, large and colosaal
                    blockAnimation = "Block_01"; // TH_Block_Guard_Light_01;
                    return;
                }
                // BELOW CODE: Poise bracket > 50 < 75 - Large damage
                else if (poiseDamage <= 74 && poiseDamage >= 50)
                {
                    // TO-DO: Change the animation when you add different type of animation type like medium, large and colosaal
                    blockAnimation = "Block_01"; // TH_Block_Guard_Medium_01;
                    return;
                }
                // BELOW CODE: Poise bracket > 75   -    Colosaal damage
                else if (poiseDamage >= 75)
                {
                    // TO-DO: Change the animation when you add different type of animation type like medium, large and colosaal
                    blockAnimation = "Block_01"; // TH_Block_Guard_Heavy_01;
                    return;
                }
            }
        }

        private void PlayBlockSoundFX(CharacterManager character)
        {
            // BELOW CODE: If the character with right handed weapon
            if(character.isTwoHandingWeapon)
            {
                character.characterSoundFXManager.PlayRandomSoundFXFromArray(character.characterInventoryManager.rightHandWeapon.blockEffectSound);
            }
            // BELOW CODE: If the character with off (left) handed weapon
            else 
            {
                character.characterSoundFXManager.PlayRandomSoundFXFromArray(character.characterInventoryManager.leftHandWeapon.blockEffectSound);
            }         
        }
        private void AssignNewAITarget(CharacterManager character)
        {
            AICharacterManager enemyCharacter = character as AICharacterManager;

            if(enemyCharacter != null && characterCausingDamage != null)
            {
                enemyCharacter.currentTarget = characterCausingDamage;
            }
        }
    }
}