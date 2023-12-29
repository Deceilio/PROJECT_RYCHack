using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deceilio.Psychain
{
    public class AICharacterWeaponSlotManager : CharacterWeaponSlotManager
    {

        public override void GrantWeaponAttackingPoiseBonus()
        {
            character.characterStatsManager.totalPoiseDefense = character.characterStatsManager.totalPoiseDefense + character.characterStatsManager.attackingPoiseBonus;
        }
        public override void ResetWeaponAttackingPoiseBonus()
        {
            character.characterStatsManager.totalPoiseDefense = character.characterStatsManager.armourPoiseBonus;
        }
    }
}
