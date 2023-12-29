using UnityEngine;

namespace Deceilio.Psychain
{
    [CreateAssetMenu(menuName = "Psychain/Item Actions/Attack/Critical Attack Action")]
    public class CriticalAttackActions : WRLD_ITEM_ACTION
    {
        public override void PerformAction(CharacterManager character)
        {
            if (character.isPerformingAction)
                return;
                
            character.characterCombatManager.AttemptBackStabOrRiposte();
        }
    }
}