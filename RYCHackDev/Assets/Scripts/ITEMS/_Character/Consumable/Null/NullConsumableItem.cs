using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deceilio.Psychain
{
    [CreateAssetMenu(menuName = "Psychain/Items/Consumables/Null")]
    public class NullConsumableItem : WRLD_CONSUMABLE_ITEM
    {
        public override void AttemptToConsumeItem(CharacterManager character)
        {
            //base.AttemptToConsumeItem(character);
        }
    }
}