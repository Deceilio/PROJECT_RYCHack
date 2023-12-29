using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deceilio.Psychain
{
    [CreateAssetMenu(menuName = "Psychain/Item Actions/Defense/Blocking Action")]
    public class BlockingAction : WRLD_ITEM_ACTION
    {
        public override void PerformAction(CharacterManager character)
        {
            if(character.isPerformingAction)
                return;

            if(character.isBlocking)
                return;

            character.characterCombatManager.SetBlockingAbsorptionsFromBlockingWeapon();
            character.isBlocking = true;
        }
    }
}
