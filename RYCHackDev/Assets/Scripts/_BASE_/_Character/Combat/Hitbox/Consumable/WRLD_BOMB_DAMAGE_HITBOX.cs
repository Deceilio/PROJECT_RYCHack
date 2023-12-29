using UnityEngine;

namespace Deceilio.Psychain
{
    public class WRLD_BOMB_DAMAGE_HITBOX : WRLD_DAMAGE_HITBOX
    {
        [Header("EXPLOSIVE DAMAGE & RADIUS")]
        public int explosiveRadius = 1; // Explosive radius for the bomb
         public int explosionSplashDamage; // Damage from splash of explosion
        public int explosionDamage; // explosive damage of the bobm
        // magicExplosiveDamage, lightingExplosiveDamage, darkExplosiveDamage, etc, if we need it

        [Header("BOMB HITBOX DATA")]
        public Rigidbody bombRigidBody; // Rigidbody component of the bomb
        private bool hasCollided = false; // Checks if the bomb collided or not
        public GameObject impactParticles; // Particles effects after colliding with the bomb
    
        protected override void Awake()
        {
            damageCollider = GetComponent<Collider>();
            bombRigidBody = GetComponent<Rigidbody>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(!hasCollided)
            {
                hasCollided = true;
                impactParticles = Instantiate(impactParticles, transform.position, Quaternion.identity);
                Explode();

                CharacterManager character = collision.transform.GetComponent<CharacterManager>();
                
                if(character != null)
                {
                    // BELOW CODE: Check for friendly fire
                    if(character.characterStatsManager.entityTeamIDNumber != entityTeamIDNumber)
                    {
                        TakeDamageState takeDamageState = Instantiate(WRLD_CHARACTER_STATES_MANAGER.instance.takeDamageState);
                        takeDamageState.physicalDamage = physicalDamage;
                        takeDamageState.fireDamage = fireDamage;
                        takeDamageState.poiseDamage = poiseDamage;
                        takeDamageState.contactPoint = contactPoint;
                        takeDamageState.angleHitFrom = angleHitFrom;
                        character.characterStatesManager.ProcessStateInstantly(takeDamageState);
                    }
                }

                Destroy(impactParticles, 5f);
                Destroy(transform.parent.parent.gameObject);
            }
        }

        private void Explode()
        {
            Collider[] characters = Physics.OverlapSphere(transform.position, explosiveRadius);

            foreach(Collider objectivesInExplosion in characters)
            {
                CharacterManager character = objectivesInExplosion.GetComponentInParent<CharacterManager>();
                
                if(character != null)
                {
                    if(character.characterStatsManager.entityTeamIDNumber != entityTeamIDNumber)
                    {
                        TakeDamageState takeDamageState = Instantiate(WRLD_CHARACTER_STATES_MANAGER.instance.takeDamageState);
                        takeDamageState.physicalDamage = physicalDamage;
                        takeDamageState.fireDamage = fireDamage;
                        takeDamageState.poiseDamage = poiseDamage;
                        takeDamageState.contactPoint = contactPoint;
                        takeDamageState.angleHitFrom = angleHitFrom;
                        character.characterStatesManager.ProcessStateInstantly(takeDamageState);
                    }
                }
            }
        }
    }
}