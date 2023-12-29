using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deceilio.Psychain
{
    public class WRLD_DESTROY_AFTER_CASTING_SKILL : MonoBehaviour
    {
        CharacterManager characterCastingSkill; // Reference to the character who is casting the skill

        private void Awake()
        {
            characterCastingSkill = GetComponentInParent<CharacterManager>();
        }
        private void Update()
        {
            if(characterCastingSkill.isFiringSkill)
            {
                Destroy(gameObject);
            }
        }
    }
}