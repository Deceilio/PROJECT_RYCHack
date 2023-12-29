using UnityEngine;

namespace Deceilio.Psychain 
{
    public class BASIC_PursueTargetState : WRLD_STATE
    {
        [Header("COMBAT STANCE STATE")]
        public BASIC_CombatStanceState combatStanceState; // Reference to the Combat Stance State script
        private void Awake()
        {
            combatStanceState = GetComponent<BASIC_CombatStanceState>();
        }
        public override WRLD_STATE Tick(AICharacterManager aiCharacter)
        {
            // LOGIC: Chase the target
            // LOGIC: If within attack range switch to combat stance state
            // LOGIC: If target is out of range, return this stance and continue to chase the target

            UseRotateTowardsTarget(aiCharacter);

            if(aiCharacter.isPerformingAction)
                return this;

            if(aiCharacter.isPerformingFunctions)
            {
                aiCharacter.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                return this;
            }

            if(aiCharacter.distanceFromTarget > aiCharacter.maximumAggroRadius)
            {
                aiCharacter.animator.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
            }
            
            if(aiCharacter.distanceFromTarget <= aiCharacter.maximumAggroRadius)
            {
                return combatStanceState;
            }
            else 
            {
                return this;
            }  
        }

        private void UseRotateTowardsTarget(AICharacterManager aiCharacter)
        {
            // BELOW CODE: Rotate manually
            if(aiCharacter.isPerformingFunctions)
            {
                Vector3 direction = aiCharacter.currentTarget.transform.position - transform.position;
                direction.y = 0;
                direction.Normalize();

                if(direction == Vector3.zero)
                {
                    direction = transform.forward;
                }

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                aiCharacter.transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, aiCharacter.rotationSpeed / Time.deltaTime);
            }
            // BELOW CODE: Rotate with path finding (Navmesh)
            else 
            {
                Vector3 relativeDirection = transform.InverseTransformDirection(aiCharacter.navMeshAgent.desiredVelocity);
                Vector3 targetVelocity = aiCharacter.rigidBody.velocity;

                aiCharacter.navMeshAgent.enabled = true;
                aiCharacter.navMeshAgent.SetDestination(aiCharacter.currentTarget.transform.position);
                aiCharacter.rigidBody.velocity = targetVelocity;
                aiCharacter.transform.rotation = Quaternion.Lerp(aiCharacter.transform.rotation, aiCharacter.navMeshAgent.transform.rotation, aiCharacter.rotationSpeed / Time.deltaTime);
            }
        }
    }
}

