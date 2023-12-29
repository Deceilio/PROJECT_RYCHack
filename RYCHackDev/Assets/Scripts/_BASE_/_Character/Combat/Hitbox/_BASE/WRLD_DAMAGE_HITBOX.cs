using UnityEngine;
using System.Collections.Generic;

namespace Deceilio.Psychain
{
    public class WRLD_DAMAGE_HITBOX : MonoBehaviour
    {
        public CharacterManager character; // Reference to the Character Manager script

        [Header("ENTITY TEAM I.D")]
        public int entityTeamIDNumber = 0; // Reference to the entity team id number

        [Header("Poise")]
        public float poiseDamage; // The value of poise damage
        public float attackingPoiseBonus; // The value of poise dealt by attacking with a weapon

        [Header("HITBOX DATA")]
        public Collider damageCollider; // Reference to the damage collider attached to the weapon
        public bool enabledDamageHitboxOnStartup = false; // Enable the damage collider on startup
        public int physicalDamage = 25; // Physical damage of the weapon
        public int fireDamage; // Fire damage of the weapon

        [Header("GUARD BREAK MODIFIER")]
        public int guardBreakModifier = 1; // Reference to the guard break damage modifier
        //public int darkDamage;
        //public int lightningDamage;

        protected Vector3 contactPoint; // Contact point of the damage hitbox
        protected float angleHitFrom; // Which angle the character got hit from
        protected string currentDamageAnimation; // Current damage animation of the weapon 
        //public List<CharacterManager> charactersDamageDuringThisCalculation = new List<CharacterManager>();

        bool shieldHasBeenHit; // Checks if shield has been hit to other character
        bool hasBeenParried; // Checks if the character is being parried or not
        
        protected virtual void Awake() 
        {
            damageCollider = GetComponent<Collider>(); 
            damageCollider.gameObject.SetActive(true);
            damageCollider.isTrigger = true;
            damageCollider.enabled = false; 
            damageCollider.enabled = enabledDamageHitboxOnStartup;

            character = GetComponentInParent<CharacterManager>();
        }
        public void EnableDamageCollider()
        {
            damageCollider.enabled = true;
        }
        public void DisableDamageCollider()
        {
            damageCollider.enabled = false;
        }
        private void OnTriggerEnter(Collider collision)
        {
            //Debug.Log("Chal Bhai Collision Kaam krrha hai. Tune " + "<b>'" + gameObject.name + "'</b>" + " se mara hehe :)");
            if (collision.tag == "Character")
            {
                shieldHasBeenHit = false;
                hasBeenParried = false;

                CharacterManager enemyCharacter = collision.GetComponent<CharacterManager>();

                if(enemyCharacter != null)
                {
                    AICharacterManager aiCharacter = enemyCharacter as AICharacterManager;

                    if(enemyCharacter.characterStatsManager.entityTeamIDNumber == entityTeamIDNumber)
                        return;

                    CheckForParry(enemyCharacter);
                    CheckForBlock(enemyCharacter);

                    if (enemyCharacter.characterStatsManager.entityTeamIDNumber == entityTeamIDNumber)
                        return;

                    if (hasBeenParried)
                        return;

                    if (shieldHasBeenHit)
                        return;

                    enemyCharacter.characterStatsManager.poiseResetTimer = enemyCharacter.characterStatsManager.totalPoiseResetTime;
                    enemyCharacter.characterStatsManager.totalPoiseDefense = enemyCharacter.characterStatsManager.totalPoiseDefense - poiseDamage;

                    Debug.Log("Enemy Poise is currently " % WRLD_COLORIZE_EDITOR.Red % WRLD_FONT_FORMAT.Bold + enemyCharacter.characterStatsManager.totalPoiseDefense);

                    // TO-DO: Play the blood effects
                    contactPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                    angleHitFrom = (Vector3.SignedAngle(character.transform.forward, enemyCharacter.transform.forward, Vector3.up));
                    DealDamage(enemyCharacter);

                    if(aiCharacter != null)
                    {
                        // BELOW CODE: If the target is A.I, the A.I receives a new target, the character dealing damage to it
                        Debug.Log(character.gameObject.name);
                        aiCharacter.currentTarget = character;
                    }
                }           
            }

            if (collision.tag == "Invisible Wall")
            {
                WRLD_INVISIBLE_WALL invisibleWall = collision.GetComponent<WRLD_INVISIBLE_WALL>();
                invisibleWall.wallHasBeenHit = true;
            }   
        }

        protected virtual void CheckForParry(CharacterManager enemyCharacter)
        {
            if(enemyCharacter.isParrying)
            {
                enemyCharacter.GetComponent<CharacterAnimatorManager>().PlayTargetActionAnimation("Parried_01", true);
                hasBeenParried = true;
            }
        }
        protected virtual void CheckForBlock(CharacterManager enemyCharacter)
        {
            Vector3 directionFromPlayerToEnemy = (character.transform.position - enemyCharacter.transform.position);
            float dotValueFromPlayerToEnemy = Vector3.Dot(directionFromPlayerToEnemy, enemyCharacter.transform.forward);

            if(enemyCharacter.isBlocking && dotValueFromPlayerToEnemy > 0.3f)
            {
                shieldHasBeenHit = true;  

                TakeBlockedDamageState takeBlockedDamage = Instantiate(WRLD_CHARACTER_STATES_MANAGER.instance.takeBlockedDamageState);
                takeBlockedDamage.physicalDamage = physicalDamage;
                takeBlockedDamage.fireDamage = fireDamage;
                takeBlockedDamage.poiseDamage = poiseDamage;
                takeBlockedDamage.staminaDamage = poiseDamage;

                enemyCharacter.characterStatesManager.ProcessStateInstantly(takeBlockedDamage);
            }
        }

        protected virtual void DealDamage(CharacterManager enemyCharacter)
        {
            float finalPhysicalDamage = physicalDamage;
            float finalFireDamage = fireDamage;

            //BELOW CODE: If we using right hand, we compare the right hand weapon damage modifiers
            if (character.isUsingRightHand)
            {
                //BELOW CODE: Get attack type from attacking character
                if (character.characterCombatManager.currentAttackType == AttackType.Light)
                {
                    //BELOW CODE: Apply damage multipliers
                    finalPhysicalDamage = finalPhysicalDamage * character.characterInventoryManager.rightHandWeapon.lightAttackDamageModifier;
                    finalFireDamage = finalFireDamage * character.characterInventoryManager.rightHandWeapon.lightAttackDamageModifier;
                }
                else if (character.characterCombatManager.currentAttackType == AttackType.Heavy)
                {
                    //BELOW CODE: Apply damage multipliers
                    finalPhysicalDamage = finalPhysicalDamage * character.characterInventoryManager.rightHandWeapon.heavyAttackDamageModifier;
                    finalFireDamage = finalFireDamage * character.characterInventoryManager.rightHandWeapon.heavyAttackDamageModifier;
                }
            }

            //BELOW CODE: If we using left hand, we compare the left hand weapon damage modifiers
            else if (character.isUsingLeftHand)
            {
                //BELOW CODE: Get attack type from attacking character
                if (character.characterCombatManager.currentAttackType == AttackType.Light)
                {
                    //BELOW CODE: Apply damage multipliers
                    finalPhysicalDamage = finalPhysicalDamage * character.characterInventoryManager.leftHandWeapon.lightAttackDamageModifier;          
                    finalFireDamage = finalFireDamage * character.characterInventoryManager.leftHandWeapon.lightAttackDamageModifier;
                }
                else if (character.characterCombatManager.currentAttackType == AttackType.Heavy)
                {
                    //BELOW CODE: Apply damage multipliers
                    finalPhysicalDamage = finalPhysicalDamage * character.characterInventoryManager.leftHandWeapon.heavyAttackDamageModifier;
                    finalFireDamage = finalFireDamage * character.characterInventoryManager.leftHandWeapon.heavyAttackDamageModifier;
                }
            }

            TakeDamageState takeDamageState = Instantiate(WRLD_CHARACTER_STATES_MANAGER.instance.takeDamageState);
            takeDamageState.physicalDamage = finalPhysicalDamage;
            takeDamageState.fireDamage = finalFireDamage;
            takeDamageState.poiseDamage = poiseDamage;
            takeDamageState.contactPoint = contactPoint;
            takeDamageState.angleHitFrom = angleHitFrom;
            enemyCharacter.characterStatesManager.ProcessStateInstantly(takeDamageState);
        }
    }
}
