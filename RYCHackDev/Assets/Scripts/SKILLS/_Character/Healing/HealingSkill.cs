using UnityEngine;

namespace Deceilio.Psychain
{
    [CreateAssetMenu(menuName = "Psychain/Skills/Healing Skill")]
    public class HealingSkill : WRLD_SKILL_ITEM
    {
        [Header("SKILL PROPERTIES")]
        public int healAmount; // Amount to heal from
        public override void AttemptToCastSkill(CharacterManager character)
        {
            base.AttemptToCastSkill(character);
            GameObject instantiatedWarmUpSkillFX = Instantiate(skillWarmUpFX, character.transform);
            character.characterAnimatorManager.PlayTargetActionAnimation(skillAnimation, true);
            Debug.Log("Attempting to cast skill...");
        }
        
        public override void SuccessfullyCastSkill(CharacterManager character)
        {
            base.SuccessfullyCastSkill(character);
            GameObject instantiatedSkillFX = Instantiate(skillCastFX, character.transform);
            character.characterStatsManager.currentHealth = character.characterStatsManager.currentHealth + healAmount;
            character.characterStatsManager.HealPlayer(healAmount);
            Debug.Log("Skill cast successful!");
        }
    }
}
