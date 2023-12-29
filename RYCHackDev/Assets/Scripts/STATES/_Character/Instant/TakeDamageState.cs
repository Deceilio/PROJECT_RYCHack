using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deceilio.Psychain
{
    [CreateAssetMenu(menuName = "Psychain/Character States/Instant States/Take Damage")]
    public class TakeDamageState : WRLD_CHARACTER_STATE
    {
        [Header("CHARACTER CAUSING DAMAGE")]
        public CharacterManager characterCausingDamage; // Reference to the Character Manager script to get which character is causing damage

        [Header("DAMAGE")]
        public float physicalDamage = 0; // Physical damage value
        public float fireDamage = 0; // Fire damage value

        [Header("POISE")]
        public float poiseDamage; // Poise damage value
        public bool poiseIsBroken; // Check if the poise if broken or not

        [Header("ANIMATIONS")]
        public bool playDamageAnimation = true; // Checks for playing the damage animation
        public bool manuallySelectDamageAnimation = false; // Checks for the manually selection of damage animation
        public string damageAnimation; // Damage animation name to trigger the animation

        [Header("SFX")]
        public bool willPlayDamageSFX = true; // Check if the character will play the damage SFX or not
        public AudioClip elementalDamageSoundFX; // Sound clip for the elemental damage sound for element Damage (Fire, Magic, Darkness, Lighting)

        [Header("DIRECTION DAMAGE TAKEN FROM")]
        public float angleHitFrom; // Which angle the character got hit from
        public Vector3 contactPoint; // The position where the damage strikes on the character

        public override void ProcessState(CharacterManager character)
        {
            // BELOW CODE: If the character is dead, return without any logic
            if (character.isDead)
                return;

            // BELOW CODE: If the character is invulnerable, no damage is taken
            if (character.isInvulerable)
                return;

            // LOGIC: Damage hitbox logic

            // BELOW CODE: Calculate total damage after defense
            CalculateDamage(character);
            // BELOW CODE: Check which direction the damage came from so we can play the right animation
            CheckWhichDirectionDamageCameFrom(character);
            // BELOW CODE: Play a damage animation
            PlayDamageAnimation(character);
            // BELOW CODE: Play damage sfx
            PlayDamageSoundFX(character);
            // TO-DO: Play blood splat // blood vfx
            //PlayDamageVFX(character);
            // BELOW CODE: If the character is an A.I character, assign them as a damage character as a target
            AssignNewAITarget(character);
        }
        private void CalculateDamage(CharacterManager character)
        {
            // BELOW CODE: Before calculating the damage defense, we check the attacking characters damage modifiers
            if(characterCausingDamage != null)
            {
                physicalDamage = Mathf.RoundToInt(physicalDamage * (characterCausingDamage.characterStatsManager.physicalDamagePercentageModifier / 100));
                fireDamage = Mathf.RoundToInt(fireDamage * (characterCausingDamage.characterStatsManager.fireDamagePercentageModifier / 100));
            }

            character.characterAnimatorManager.EraseHandIKForWeapon();

            float totalPhysicalDamageAbsorption = 1 -
                (1 - character.characterStatsManager.physicalDamageAbsorptionHead / 100) *
                (1 - character.characterStatsManager.physicalDamageAbsorptionChest / 100) *
                (1 - character.characterStatsManager.physicalDamageAbsorptionLegs / 100) *
                (1 - character.characterStatsManager.physicalDamageAbsorptionHands / 100);;

            physicalDamage = Mathf.RoundToInt(physicalDamage - (physicalDamage
                * totalPhysicalDamageAbsorption));

            float totalFireDamageAbsorption = 1 -
                (1 - character.characterStatsManager.fireDamageAbsorptionHead / 100) *
                (1 - character.characterStatsManager.fireDamageAbsorptionChest / 100) *
                (1 - character.characterStatsManager.fireDamageAbsorptionLegs / 100) *
                (1 - character.characterStatsManager.fireDamageAbsorptionHands / 100);

            fireDamage = Mathf.RoundToInt(fireDamage - (fireDamage
                * totalFireDamageAbsorption));

            Debug.Log("Total Physical Damage Absorption: " % WRLD_COLORIZE_EDITOR.Yellow % WRLD_FONT_FORMAT.Bold + totalPhysicalDamageAbsorption + "%" % WRLD_COLORIZE_EDITOR.Yellow % WRLD_FONT_FORMAT.Bold);
            Debug.Log("Total Fire Damage Absorption: " % WRLD_COLORIZE_EDITOR.Red % WRLD_FONT_FORMAT.Bold + totalFireDamageAbsorption + "%" % WRLD_COLORIZE_EDITOR.Red % WRLD_FONT_FORMAT.Bold);

            physicalDamage = physicalDamage - Mathf.RoundToInt(physicalDamage * (character.characterStatsManager.physicalAbsorptionPercentageModifier / 100));
            fireDamage = fireDamage - Mathf.RoundToInt(fireDamage * (character.characterStatsManager.fireAbsorptionPercentageModifier / 100));

            float finalDamage = physicalDamage + fireDamage; // Add, +Magic damage, +Lightning damage, +Dark damage if needed

            Debug.Log("Total Damage Dealth on Armour is: " % WRLD_COLORIZE_EDITOR.Yellow % WRLD_FONT_FORMAT.Bold + finalDamage);

            character.characterStatsManager.currentHealth = Mathf.RoundToInt(character.characterStatsManager.currentHealth - finalDamage);

            // if(!character.isBoss)
            // {
            //     if(character.isAICharacter)
            //     {
            //         character.characterStatsManager.aiCharacterHealthBar.SetCurrentHealth(Mathf.RoundToInt(currentHealth));
            //     }
            //     else 
            //     {
            //         character.characterStatsManager.healthBar.SetCurrentHealth(currentHealth);
            //     }
            // }
            // else if(character.isBoss && character.boss != null)
            // {
            //     aiCharacter.boss.UpdateBossHealthBar(currentHealth, maxHealth);
            // }
            if(character.characterStatsManager.totalPoiseDefense < poiseDamage)
            {
                poiseIsBroken = true;
            }

            if (character.characterStatsManager.currentHealth <= 0)
            {
                character.characterStatsManager.currentHealth = 0;
                character.isDead = true;
            }
        }
        private void CheckWhichDirectionDamageCameFrom(CharacterManager character)
        {
            if (manuallySelectDamageAnimation)
                return;

            if (angleHitFrom >= 145 && angleHitFrom <= 180)
            {
                ChooseDamageAnimationForward(character);
            }
            else if (angleHitFrom <= -145 && angleHitFrom >= -180)
            {
                ChooseDamageAnimationForward(character);
            }
            else if (angleHitFrom >= -45 && angleHitFrom <= 45)
            {
                ChooseDamageAnimationBackward(character);
            }
            else if (angleHitFrom >= -144 && angleHitFrom <= -45)
            {
                ChooseDamageAnimationLeft(character);
            }
            else if (angleHitFrom >= 45 && angleHitFrom <= 144)
            {
                ChooseDamageAnimationRight(character);
            }
        }
        private void ChooseDamageAnimationForward(CharacterManager character)
        {
            // BELOW CODE: Poise bracket < 25     -  Small damage
            if (poiseDamage <= 24 && poiseDamage >= 0)
            {
                damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.Damage_Animations_Small_Forward);
                return;
            }
            // BELOW CODE: Poise bracket > 25 < 50 - Medium damage
            else if (poiseDamage <= 49 && poiseDamage >= 25)
            {
                // TO-DO: Change the animation when you add different type of animation type like medium, large and colosaal
                damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.Damage_Animations_Small_Forward);
                return;
            }
            // BELOW CODE: Poise bracket > 50 < 75 - Large damage
            else if (poiseDamage <= 74 && poiseDamage >= 50)
            {
                // TO-DO: Change the animation when you add different type of animation type like medium, large and colosaal
                damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.Damage_Animations_Small_Forward);
                return;
            }
            // BELOW CODE: Poise bracket > 75   -    Colosaal damage
            else if (poiseDamage >= 75)
            {
                // TO-DO: Change the animation when you add different type of animation type like medium, large and colosaal
                damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.Damage_Animations_Small_Forward);
                return;
            }
        }
        private void ChooseDamageAnimationBackward(CharacterManager character)
        {
            // BELOW CODE: Poise bracket < 25     -  Small damage
            if (poiseDamage <= 24 && poiseDamage >= 0)
            {
                damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.Damage_Animations_Small_Backward);
                return;
            }
            // BELOW CODE: Poise bracket > 25 < 50 - Medium damage
            else if (poiseDamage <= 49 && poiseDamage >= 25)
            {
                // TO-DO: Change the animation when you add different type of animation type like medium, large and colosaal
                damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.Damage_Animations_Small_Backward);
                return;
            }
            // BELOW CODE: Poise bracket > 50 < 75 - Large damage
            else if (poiseDamage <= 74 && poiseDamage >= 50)
            {
                // TO-DO: Change the animation when you add different type of animation type like medium, large and colosaal
                damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.Damage_Animations_Small_Backward);
                return;
            }
            // BELOW CODE: Poise bracket > 75   -    Colosaal damage
            else if (poiseDamage >= 75)
            {
                // TO-DO: Change the animation when you add different type of animation type like medium, large and colosaal
                damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.Damage_Animations_Small_Backward);
                return;
            }
        }
        private void ChooseDamageAnimationLeft(CharacterManager character)
        {
            // BELOW CODE: Poise bracket < 25     -  Small damage
            if (poiseDamage <= 24 && poiseDamage >= 0)
            {
                damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.Damage_Animations_Small_Left);
                return;
            }
            // BELOW CODE: Poise bracket > 25 < 50 - Medium damage
            else if (poiseDamage <= 49 && poiseDamage >= 25)
            {
                // TO-DO: Change the animation when you add different type of animation type like medium, large and colosaal
                damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.Damage_Animations_Small_Left);
                return;
            }
            // BELOW CODE: Poise bracket > 50 < 75 - Large damage
            else if (poiseDamage <= 74 && poiseDamage >= 50)
            {
                // TO-DO: Change the animation when you add different type of animation type like medium, large and colosaal
                damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.Damage_Animations_Small_Left);
                return;
            }
            // BELOW CODE: Poise bracket > 75   -    Colosaal damage
            else if (poiseDamage >= 75)
            {
                // TO-DO: Change the animation when you add different type of animation type like medium, large and colosaal
                damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.Damage_Animations_Small_Left);
                return;
            }
        }
        private void ChooseDamageAnimationRight(CharacterManager character)
        {
            // BELOW CODE: Poise bracket < 25     -  Small damage
            if (poiseDamage <= 24 && poiseDamage >= 0)
            {
                damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.Damage_Animations_Small_Right);
                return;
            }
            // BELOW CODE: Poise bracket > 25 < 50 - Medium damage
            else if (poiseDamage <= 49 && poiseDamage >= 25)
            {
                // TO-DO: Change the animation when you add different type of animation type like medium, large and colosaal
                damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.Damage_Animations_Small_Right);
                return;
            }
            // BELOW CODE: Poise bracket > 50 < 75 - Large damage
            else if (poiseDamage <= 74 && poiseDamage >= 50)
            {
                // TO-DO: Change the animation when you add different type of animation type like medium, large and colosaal
                damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.Damage_Animations_Small_Right);
                return;
            }
            // BELOW CODE: Poise bracket > 75   -    Colosaal damage
            else if (poiseDamage >= 75)
            {
                // TO-DO: Change the animation when you add different type of animation type like medium, large and colosaal
                damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.Damage_Animations_Small_Right);
                return;
            }
        }
        private void PlayDamageSoundFX(CharacterManager character)
        {
            character.characterSoundFXManager.PlayRandomDamageSoundFX();

            if(fireDamage > 0)
            {
                character.characterSoundFXManager.PlaySoundFX(elementalDamageSoundFX); 
            }
        }
        private void PlayDamageAnimation(CharacterManager character)
        {
            // LOGIC: If we are currently playing a damage animation that is heavy and light attack and it hit us
            // LOGIC: If we don't want to play the light damage animation, we want to finish the heavy damage animation

            // BELOW CODE: If the character is performing an action && the previous poise damage is above 0, then the character must be in a damage animation
            if (character.isPerformingAction && character.characterCombatManager.previousPoiseDamageTaken > poiseDamage)
            {
                // BELOW CODE: If the previous poise is above the current poitse, return, so we don't change the damage animation to lighter animation
                return;
            }

            // BELOW CODE: If the character is dead then play the death animation && also close the character weapon's damage hitbox
            if(character.isDead)
            {
                character.characterWeaponSlotManager.CloseDamageCollider();
                character.characterAnimatorManager.PlayTargetActionAnimation("Dead_01", true);
                return;
            }

            // BELOW CODE: If poise is not broken then don't play the damage animation
            if (!poiseIsBroken)
            {
                return;
            }
            else
            {
                // Enable/disable stun lock
                if(playDamageAnimation)
                {
                    character.characterAnimatorManager.PlayTargetActionAnimation(damageAnimation, true);
                }
            }
        }
        // private void PlayDamageVFX(CharacterManager character)
        // {
        //     character.characterStatesManager.PlayBloodSplatterFX(contactPoint);
        // }
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
