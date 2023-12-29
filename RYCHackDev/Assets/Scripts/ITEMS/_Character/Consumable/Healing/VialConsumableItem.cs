using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deceilio.Psychain
{
    [CreateAssetMenu(menuName = "Psychain/Items/Consumables/Vial")]
    public class VialConsumableItem : WRLD_CONSUMABLE_ITEM
    {
        // NOTE - Vial is the term we used in this game for flask/potion bottle
        // NOTE - Vial of vigour meant for health points
        // NOTE - Vial of Zenith meant for skill points

        [Header("Vial Type")]
        public bool vigourVial; // Reference to the healing flask/potion
        public bool zenithVial; // Reference to the skill points flask
 
        [Header("Recovery Amount")]
        public int healthRecoveryAmount; // Recovery amount for health used by vival of vigour 
        public int skillPointRecoveryAmount; // Recovery amount for skill points used by vial of zenith

        [Header("Recovery FX")]
        public GameObject recoveryFX; // Particle effects for using recovery cials

        [Header("GAME OBJECT")]
        public float timeUntilDestroyed = 1; // Wait time until destroy the game object

        public override void AttemptToConsumeItem(CharacterManager character)
        {
            base.AttemptToConsumeItem(character);
            GameObject vial = Instantiate(itemModel, character.characterWeaponSlotManager.rightHandSlot.transform);
            // BELOW CODE: Play recovery fx if we drink without being hit
            character.characterStatesManager.currentParticleFX = recoveryFX;
            
            if(vigourVial)
            {
                // BELOW CODE: Add health 
                character.characterStatesManager.amountToBeHealed = healthRecoveryAmount;
            }
            else if(zenithVial)
            {
                // BELOW CODE: Add skill points
                character.characterStatesManager.amountToBeRecoverSkillPoints = skillPointRecoveryAmount;
            }

            character.characterStatesManager.instantiatedFXModel = vial;
            // BELOW CODE: Instantiate vial in hand of the player and play drinking animation
            character.characterWeaponSlotManager.rightHandSlot.UnloadWeapon();
            Destroy(vial.gameObject, timeUntilDestroyed);
        }
    }
}