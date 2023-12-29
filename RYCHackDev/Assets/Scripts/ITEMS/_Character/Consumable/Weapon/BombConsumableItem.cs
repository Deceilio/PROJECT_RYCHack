using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deceilio.Psychain
{
    [CreateAssetMenu(menuName = "Psychain/Items/Consumables/Bomb")]
    public class BombConsumableItem : WRLD_CONSUMABLE_ITEM
    {
        [Header("VELOCITY")]
        public int upwardVelocity = 50; // Upward velocity of the bomb
        public int forwardVelocity = 50; // Forward velocity of the bomb
        public int bombMass = 1; // Mass of the bomb

        [Header("BOMB MODEL")]
        public GameObject bombModel; // Model of the bomb

        [Header("BASE DAMAGE")]
        public int baseDamage = 200; // Bomb base damage
        public int explosiveDamage = 75; // Explosive damage from bomb

        public override void AttemptToConsumeItem(CharacterManager character)
        {
            if(currentItemAmount > 0)
            {
                character.characterWeaponSlotManager.rightHandSlot.UnloadWeapon(); // This is used to disable the weapons in the hands
                character.characterAnimatorManager.PlayTargetActionAnimation(consumeAnimation, isPerformingAction, true, false, false);
                GameObject model = Instantiate(itemModel, character.characterWeaponSlotManager.rightHandSlot.transform.position, Quaternion.identity, character.characterWeaponSlotManager.rightHandSlot.transform); 
                character.characterStatesManager.instantiatedFXModel = model;
                Debug.Log("Item Consumed Successfully!");
            }
            else
            {
                character.characterAnimatorManager.PlayTargetActionAnimation("Shrug", true);
                Debug.Log("No item found!");
            }
        }
    }
}