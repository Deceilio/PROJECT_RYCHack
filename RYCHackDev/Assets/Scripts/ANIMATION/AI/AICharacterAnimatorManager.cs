using UnityEngine;

namespace Deceilio.Psychain
{
    public class AICharacterAnimatorManager : CharacterAnimatorManager
    {
        AICharacterManager aiCharacter; // Reference to the A.I Character Manager script
        protected override void Awake()
        {
            base.Awake();
            aiCharacter = GetComponent<AICharacterManager>();
        }
        public void InstantiateBossParticleFX()
        {
            WRLD_BOSS_FX_TRANSFORM bossFXTransform = GetComponentInChildren<WRLD_BOSS_FX_TRANSFORM>();

            GameObject phaseFX = Instantiate(aiCharacter.boss.secondPhaseParticleFX, bossFXTransform.transform);
        }
        public override void OnAnimatorMove()
        {
            Vector3 velocity = aiCharacter.animator.deltaPosition;
            aiCharacter.characterController.Move(velocity);

            if (aiCharacter.applyRootMotion)
            {
                aiCharacter.transform.rotation *= aiCharacter.animator.deltaRotation;
            }
        }
    }
}