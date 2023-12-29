using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deceilio.Psychain
{
    [CreateAssetMenu(menuName = "Psychain/Character States/Static States/Modify Damage Type")]
    public class ModifyDamageTypeStaticState : WRLD_CHARACTER_STATIC_STATE
    {
        [Header("Damage Type Effected")]
        [SerializeField] DamageType damageType; // Reference to the particular damage type
        [SerializeField] int modifiedValue; // The Modified damage value

        // BELOW CODE: When adding the state we add the damage type
        public override void AddStaticState(CharacterManager character)
        {
            base.AddStaticState(character);

            switch(damageType)
            {
                case DamageType.Physical: character.characterStatsManager.physicalDamagePercentageModifier += modifiedValue;
                    break;
                case DamageType.Fire: character.characterStatsManager.fireDamagePercentageModifier += modifiedValue;
                    break;
                default:
                    break;
            }
        }

        public override void RemoveStaticState(CharacterManager character)
        {
            base.RemoveStaticState(character);

            switch (damageType)
            {
                case DamageType.Physical:
                    character.characterStatsManager.physicalDamagePercentageModifier -= modifiedValue;
                    break;
                case DamageType.Fire:
                    character.characterStatsManager.fireDamagePercentageModifier -= modifiedValue;
                    break;
                default:
                    break;
            }
        }
    }
}