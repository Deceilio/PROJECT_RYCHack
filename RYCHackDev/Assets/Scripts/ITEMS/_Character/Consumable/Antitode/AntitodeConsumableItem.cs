using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace Deceilio.Psychain
{
    [CreateAssetMenu(menuName = "Psychain/Items/Consumables/Antitode")]
    public class AntitodeConsumableItem : WRLD_CONSUMABLE_ITEM
    {
        [Header("RECOVERY FX")]
        public GameObject antitodeConsumeFX; // VFX played when you consume antitode item

        [Header("CURE AMOUNT(0-100)")]
        public int cureAmount; // Cure poison amount of antitode
 
        [Header("CURE FX")]
        public bool curePoison; // Checks to cure poison or not

        [Header("GAME OBJECT")]
        public float timeUntilDestroyed = 1; // Wait time until destroy the game object
        //Cure Paralysis
        //Cure Blood
        //Cure Curse

        public override void AttemptToConsumeItem(CharacterManager character)
        {
            base.AttemptToConsumeItem(character);
            if(curePoison)
            {
                GameObject antitode = Instantiate(itemModel, character.characterWeaponSlotManager.rightHandSlot.transform);
                character.characterStatesManager.currentParticleFX = antitodeConsumeFX;
                character.characterStatesManager.amountToBeHealed = cureAmount;
                character.characterStatesManager.instantiatedFXModel = antitode;
                character.characterWeaponSlotManager.rightHandSlot.UnloadWeapon();
                if (cureAmount == 0)
                {
                    character.characterStatsManager.isPoisoned = false;
                    character.characterStatesManager.RemoveTimedStateParticle(StateParticleType.poison);
                }
                if (character != null)
                {
                   PlayerManager player = character as PlayerManager;

                   player.playerStatesManager.poisonAmountBar.SetPoisonAmount(cureAmount);
                }

                character.characterWeaponSlotManager.rightHandSlot.UnloadWeapon();
                Destroy(antitode.gameObject, timeUntilDestroyed);
            }
        }
    }
}