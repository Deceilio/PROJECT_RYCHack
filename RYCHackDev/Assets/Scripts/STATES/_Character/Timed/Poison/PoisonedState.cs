using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deceilio.Psychain
{
    [CreateAssetMenu(menuName = "Psychain/Character States/Timed States/Poison/Poison")]
    public class PoisonedState : WRLD_CHARACTER_STATE
    {
        public float poisonDamage = 1; // Base poison damage value
        
        public override void ProcessState(CharacterManager character)
        {
            PlayerManager player = character as PlayerManager;

            if(character.characterStatsManager.isPoisoned)
            {
                if(character.characterStatsManager.poisonAmount > 0)
                {
                    character.characterStatsManager.poisonAmount -= 1;
                    // BELOW CODE: Damage the player
                    Debug.Log("DAMAGE" % WRLD_COLORIZE_EDITOR.Red % WRLD_FONT_FORMAT.Bold);
                    TakeDamageState takeDamageState = Instantiate(WRLD_CHARACTER_STATES_MANAGER.instance.takeDamageState);
                    takeDamageState.physicalDamage = poisonDamage;
                    character.characterStatesManager.ProcessStateInstantly(takeDamageState);

                    if(player != null)
                    {
                        player.playerStatesManager.poisonAmountBar.SetPoisonAmount(Mathf.RoundToInt(character.characterStatsManager.poisonAmount));
                    }
                }
                else
                {
                    character.characterStatsManager.isPoisoned = false;
                    character.characterStatsManager.poisonAmount = 0;

                    if(player != null)
                    {
                        player.playerStatesManager.poisonAmountBar.SetPoisonAmount(0);
                    }
                }
            }
            else
            {
                character.characterStatesManager.timedEffects.Remove(this);
                character.characterStatesManager.RemoveTimedStateParticle(StateParticleType.poison);
            }
        }
    }
}