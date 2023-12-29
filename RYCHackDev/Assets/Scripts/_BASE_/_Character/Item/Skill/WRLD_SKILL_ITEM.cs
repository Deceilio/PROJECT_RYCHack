using UnityEngine;

namespace Deceilio.Psychain
{
    public class WRLD_SKILL_ITEM : WRLD_ITEM
    {
        [Header("SKILL COST")]
        public int skillPointCost; // Cost value to use a skill

        [Header("SKILL TYPE")]
        public string skillType; // Type of the skill

        [Header("SKILL VFX")]
        public GameObject skillWarmUpFX; // Reference to the skill warm up vfx object
        public GameObject skillCastFX; // Reference to the skill cast vfx object
        
        [Header("SKILL ANIMATIONS")]
        public string skillAnimation; // Reference to the skill animation

        [Header("SKILL TYPE")]
        public bool isFaithSkill; // Checks if the particular skill is a faith skill
        public bool isTechSkill; // Checks if the particular skill is a tech skill
        public bool isPyroSkill; // Checks if the particular skill is a pyro skill

        public virtual void AttemptToCastSkill(CharacterManager character)
        {
            Debug.Log("You attempt to cast a skill!");
        }
        public virtual void SuccessfullyCastSkill(CharacterManager character)
        {
            Debug.Log("You successfully cast a skill!");
            PlayerManager player = character as PlayerManager;
            
            if (player != null) 
            {
                player.playerStatsManager.DeductSkillPoints(skillPointCost);
            }
        }
    }
}