using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deceilio.Psychain
{
    public class BASIC_AmbushState : WRLD_STATE
    {
        public bool isSleeping; // Check if the A.I Character is still sleeping
        public float detectionRadius = 2; // Detecting the player and wake up from sleep state 
        public string sleepAnimation; // Reference to sleeping animation
        public LayerMask detectionLayer; // Selective layer for the detection of an A.I Character
        public string wakeAnimation; // Reference to wake up animation
        public BASIC_PursueTargetState pursueTargetState; // Reference to the Pursue Target State
        private void Awake()
        {
            pursueTargetState = GetComponent<BASIC_PursueTargetState>();
        }
        public override WRLD_STATE Tick(AICharacterManager aiCharacter)
        {
            if(isSleeping && aiCharacter.isPerformingAction == false)
            {
                aiCharacter.aiCharacterAnimatorManager.PlayTargetActionAnimation(sleepAnimation, true);
            }

            Collider[] colliders = Physics.OverlapSphere(aiCharacter.transform.position, detectionRadius, detectionLayer);

            for(int i = 0; i < colliders.Length; i++)
            {
                CharacterManager potentitalTarget = colliders[i].transform.GetComponent<CharacterManager>();

                if(potentitalTarget != null)
                {
                    Vector3 targetDirection = potentitalTarget.transform.position - aiCharacter.transform.position;
                    float viewableAngle = Vector3.Angle(targetDirection, aiCharacter.transform.forward);

                    if(viewableAngle > aiCharacter.minimumDetectionAngle
                        && viewableAngle < aiCharacter.maximumDetectionAngle)
                    {
                        aiCharacter.currentTarget = potentitalTarget;
                        isSleeping = false;
                        aiCharacter.aiCharacterAnimatorManager.PlayTargetActionAnimation(wakeAnimation, true);
                    }
                }    
            }

            if(aiCharacter.currentTarget != null)
            {
                return pursueTargetState;
            }
            else
            {
                return this;
            }
        }
    }
}