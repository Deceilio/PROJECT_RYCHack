using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deceilio.Psychain
{
    public class WRLD_SKILL_DAMAGE_HITBOX : WRLD_DAMAGE_HITBOX
    {
        CharacterManager skillCharacterTarget; // Targeted character to damage from skill
        [Header("VFX OBJECTS")]
        public GameObject impactParticles; // GameObject for the skill impact vfx
        public GameObject projectileParticles; // GameObject for the skill's projectile vfx
        public GameObject muzzleParticles; // GameObject for the skill's muzzle vfx 
    
        bool hasCollided = false; // Checks if the gameobject is collided or not

        Vector3 impactNormal; // Used to rate the impact vfx
        Rigidbody rigidBody; // Rigidbody for the gameobject

        protected override void Awake()
        {
            base.Awake();
            rigidBody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            projectileParticles = Instantiate(projectileParticles, transform.position, transform.rotation);
            projectileParticles.transform.parent = transform;

            if(muzzleParticles)
            {
                muzzleParticles = Instantiate(muzzleParticles, transform.position, transform.rotation);
                Destroy(muzzleParticles, 2f); // Destroy muzzle particles after 2 seconds
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(!hasCollided)
            {
                skillCharacterTarget = collision.transform.GetComponent<CharacterManager>();

                if(skillCharacterTarget != null && skillCharacterTarget.characterStatsManager.entityTeamIDNumber != entityTeamIDNumber)
                {             
                    TakeDamageState takeDamageState = Instantiate(WRLD_CHARACTER_STATES_MANAGER.instance.takeDamageState);
                    takeDamageState.physicalDamage = physicalDamage;
                    takeDamageState.fireDamage = fireDamage;
                    takeDamageState.poiseDamage = poiseDamage;
                    takeDamageState.contactPoint = contactPoint;
                    takeDamageState.angleHitFrom = angleHitFrom;
                    skillCharacterTarget.characterStatesManager.ProcessStateInstantly(takeDamageState);
                }

                hasCollided = true;
                impactParticles = Instantiate(impactParticles, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal));
                                
                Destroy(projectileParticles);
                Destroy(impactParticles, 1f);
                Destroy(gameObject, 2f);
            }
        }
    }
}