using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deceilio.Psychain
{
    public class WRLD_CONSUMABLE_ITEM : WRLD_ITEM
    {
        [Header("ITEM QUANTITY")]
        public int maxItemAmount; // How much consumable item character can keep
        public int currentItemAmount; // How much consumable item character have

        [Header("ITEM MODEL")]
        public GameObject itemModel; // Model for the consumable item

        [Header("ITEM ANIMATIONS")]
        public string consumeAnimation; // Animation for the particular consumable item
        public bool isPerformingAction; // Bool to check if character is performing action or not

        public virtual void AttemptToConsumeItem(CharacterManager character)
        {
            if(currentItemAmount > 0)
            {
                character.characterAnimatorManager.PlayTargetActionAnimation(consumeAnimation, isPerformingAction, true, true, true);
                Debug.Log("Item Consumed Successfully!");
            }
            else
            {
                character.characterAnimatorManager.PlayTargetActionAnimation("Shrug", true);
                Debug.Log("No item found!");
            }
        }
        public virtual void SuccessfullyConsumeItem(CharacterManager character)
        {
            currentItemAmount = currentItemAmount - 1;
        }
        public virtual bool CanIUseThisItem(CharacterManager character)
        {
            return true;
        }
    }
}