using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deceilio.Psychain
{
    [CreateAssetMenu(menuName = "Psychain/Character States/Timed States/Poison/Poison Build Up")]
    public class PoisonBuildUpState : WRLD_CHARACTER_STATE
    {        
        [SerializeField] float basePoisonBuildUpAmount = 7; // The amount of poison build up given before resistances are calculated, per game tick
        [SerializeField] float poisonAmount = 100; // The amount of poison time the character receives if they are poisoned
        [SerializeField] int poisonDamagePerTick = 5;  // The amount of damage taken from the poison if it's build up to 100%
        
        public override void ProcessState(CharacterManager character)
        {
            PlayerManager player = character as PlayerManager;

            float finalPoisonBuildUp = 0; // Posion build up after we factor in our player resistances

            if(character.characterStatsManager.poisonResistance > 0)
            {
                // BELOW CODE: If our character has 100 or more poison resistance they are basically immune
                if(character.characterStatsManager.poisonResistance >= 100)
                {
                    finalPoisonBuildUp = 0;
                }
                else
                {
                    float resistancePercentage = character.characterStatsManager.poisonResistance / 100;

                    finalPoisonBuildUp = basePoisonBuildUpAmount - (basePoisonBuildUpAmount * resistancePercentage);
                }
            }

            // BELOW CODE: Each tick we add the build up amount to the characters overall build up
            character.characterStatsManager.poisonBuildUp += finalPoisonBuildUp;

            // BELOW CODE: If the character is already poisoned, remove all posion build up effects
            if (character.characterStatsManager.isPoisoned)
            {
                character.characterStatesManager.timedEffects.Remove(this);
            }

            // BELOW CODE: If our build is 100 or more, poison the character
            if(character.characterStatsManager.poisonBuildUp >= 100)
            {
                // BELOW CODE: Poison the character
                character.characterStatsManager.isPoisoned = true;
                character.characterStatsManager.poisonAmount = poisonAmount;
                character.characterStatsManager.poisonBuildUp = 0;

                if (player != null)
                {
                    player.playerStatesManager.poisonAmountBar.SetPoisonAmount(Mathf.RoundToInt(poisonAmount));
                }

                // BELOW CODE: Poison effect
                // BELOW CODE: It will always instantiate a copy of a scriptable, so the original is never edited
                // BELOW CODE: If the original is edited, and every character uses an original, they will share the same values
                // BELOW CODE: (for example - if one is poisoned for 5 seconds all poisoned scriptables will read 5 seconds)
                PoisonedState poisonedState = Instantiate(WRLD_CHARACTER_STATES_MANAGER.instance.poisonedState);
                poisonedState.poisonDamage = poisonDamagePerTick;
                character.characterStatesManager.timedEffects.Add(poisonedState);
                character.characterStatesManager.timedEffects.Remove(this);
                character.characterSoundFXManager.PlaySoundFX(WRLD_CHARACTER_STATES_MANAGER.instance.poisonSFX);

                character.characterStatesManager.AddTimedStateParticle(Instantiate(WRLD_CHARACTER_STATES_MANAGER.instance.poisonFX));
            }

            character.characterStatesManager.timedEffects.Remove(this);
        }
    }
}
