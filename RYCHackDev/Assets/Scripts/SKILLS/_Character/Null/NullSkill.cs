using UnityEngine;

namespace Deceilio.Psychain
{
    [CreateAssetMenu(menuName = "Psychain/Skills/_NULL")]
    public class NullSkill : WRLD_SKILL_ITEM
    {
        public override void AttemptToCastSkill(CharacterManager character)
        {
            Debug.Log("Attempting to cast empty/null skill...");
        }
        
        public override void SuccessfullyCastSkill(CharacterManager character)
        {
            Debug.Log("Null Skill cast successful! Nothing happened lol!");
        }
    }
}
