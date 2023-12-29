using UnityEngine;
using System.Collections.Generic;

namespace Deceilio.Psychain
{
    public class WRLD_POISON_SURFACE : MonoBehaviour
    {
        public List<CharacterManager> charactersInsidePoisonSurface; // Character inside the poison surface have Character Effects Manager script
        
        private void OnTriggerEnter(Collider other)
        {
            CharacterManager character = other.GetComponent<CharacterManager>();
        
            if(character != null)
            {
                charactersInsidePoisonSurface.Add(character);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            CharacterManager character = other.GetComponent<CharacterManager>();
        
            if(character != null)
            {
                charactersInsidePoisonSurface.Remove(character);
            }
        }
        private void OnTriggerStay(Collider other)
        {
            foreach (CharacterManager character in charactersInsidePoisonSurface)
            {
                if (character.characterStatsManager.isPoisoned)
                    return;

                PoisonBuildUpState poisonBuildUp = Instantiate(WRLD_CHARACTER_STATES_MANAGER.instance.poisonBuildUpState);
            
                foreach(var effect in character.characterStatesManager.timedEffects)
                {
                    if (effect.stateID == poisonBuildUp.stateID)
                        return;
                }
        
                character.characterStatesManager.timedEffects.Add(poisonBuildUp);
            }
        }
    }
}