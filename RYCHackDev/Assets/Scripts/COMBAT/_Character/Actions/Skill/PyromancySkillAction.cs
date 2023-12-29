using UnityEngine;

namespace Deceilio.Psychain
{
    [CreateAssetMenu(menuName = "Psychain/Item Actions/Skills/Cast Pyro Action")]
    public class PyromancySkillAction : WRLD_ITEM_ACTION
    {
        public override void PerformAction(CharacterManager character)
        {
            if(character.isPerformingAction)
                return;

            if(character.characterSkillManager.currentSkillBeingUsed != null && character.characterSkillManager.currentSkillBeingUsed.isPyroSkill)
            {
                if(character.characterStatsManager.currentSkillPoints >= character.characterSkillManager.currentSkillBeingUsed.skillPointCost)
                {
                    character.characterSkillManager.currentSkillBeingUsed.AttemptToCastSkill(character);
                }
                else
                {
                    character.characterAnimatorManager.PlayTargetActionAnimation("Shrug_01", true);
                }
            }
        }
    }
}