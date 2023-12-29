using System.Collections;
using UnityEngine;

namespace Deceilio.Psychain
{
    public class CharacterCombatManager : MonoBehaviour
    {
        [HideInInspector] public CharacterManager character; // Reference to the Character Manager script 

        [Header("COMBAT TRANSFORMS")]
        public Transform backstabReceiverTransform; // The transform component for the character who is getting backstabbed
        public Transform riposteReceiverTransform; // The transform component for the character who is getting Riposted    
        
        public LayerMask characterLayer; // Layer for the character
        public float criticalAttackRange = 0.7f; // How near the character should be to perform critical action
        
        [Header("ATTACK TYPE")]
        public AttackType currentAttackType; // Reference to the attack type (Example - light/heavy)

        [Header("LAST AMOUNT OF POISE DAMAGE TAKEN")]
        public int previousPoiseDamageTaken; // Last poise damage taken by the character

        [Header("ATTACK ANIMATIONS")]
        [Header("ONE HANDED ATTACK ANIMATIONS")]
        public string oh_Light_Attack_1 = "OH_Sword_Light_Attack_01"; // Name of the first one handed light attack animation
        public string oh_Light_Attack_2 = "OH_Sword_Light_Attack_02"; // Name of the second one handed light attack animation
        public string oh_Light_Attack_3 = "OH_Sword_Light_Attack_03"; // Name of the third one handed light attack animation
        public string oh_Heavy_Attack_1 = "OH_Sword_Heavy_Attack_01"; // Name of the first one handed heavy attack animation
        public string oh_sprint_attack_1 = "OH_Sword_Sprint_Attack_1"; //  Name of the first one handed sprint attack animation
        public string oh_jumping_attack_1 = "OH_Sword_Jumping_Attack_1"; //  Name of the first one handed jump attack animation
        public string oh_charge_attack_1 = "OH_Sword_Charged_Attack_1"; //  Name of the first one handed charge attack animation
        public string oh_charge_attack_2 = "OH_Sword_Charged_Attack_2"; //  Name of the second one handed heavy attack animation

        [Header("TWO HANDED ATTACK ANIMATIONS")]
        public string th_Light_Attack_1 = "TH_Sword_Light_Attack_01"; // Name of the first two handed light attack animation
        public string th_Light_Attack_2 = "TH_Sword_Light_Attack_02"; // Name of the second two handed light attack animation
        public string th_Light_Attack_3 = "TH_Sword_Light_Attack_03"; // Name of the third two handed light attack animation
        public string th_Heavy_Attack_1 = "TH_Sword_Heavy_Attack_01"; // Name of the first two handed heavy attack animation
        public string th_sprint_attack_1 = "OH_Sword_Sprint_Attack_1"; //  Name of the first two handed sprint attack animation
        public string th_jumping_attack_1 = "OH_Sword_Jumping_Attack_1"; //  Name of the first two handed jump attack animation
        public string th_charge_attack_1 = "OH_Sword_Charged_Attack_1"; //  Name of the first two handed charge attack animation
        public string th_charge_attack_2 = "OH_Sword_Charged_Attack_2"; //  Name of the second two handed heavy attack animation

        [Header("DEFENSE ANIMATIONS")]
        public string shield_Block = "Shield_Block_Start_01"; // Name for the shield block animation
        
        [Header("WEAPON ART")]
        public string shield_parry = "Parry_01"; // Name for the shield parry animation

        [Header("CURRENT ATTACKS")]
        public string lastAttack;  // Tells which last attack the player performed
        public string lastAttack2; // Tells which second last attack the player performed

        [Header("CRITICAL DAMAGE")]
        // Damage will be done during an animation event
        // Used in backstab or riposte animations
        private bool checkDealthAttack = false; // Check if character dealth damage to other character
        public int pendingCriticalDamage; // Pending critical damage done by the critical attack

        protected virtual void Awake() 
        {
            character = GetComponent<CharacterManager>();
        }
        public IEnumerator UseLastAttack()
        {
            yield return null;
            lastAttack = lastAttack2;
        }
        IEnumerator ForceMoveCharacterToEnemyBackStabPosition(CharacterManager characterPerformingBackStab)
        {
            for (float timer = 0.05f; timer < 0.5f; timer = timer + 0.05f)
            {
                Quaternion backstabRotation = Quaternion.LookRotation(characterPerformingBackStab.transform.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, backstabRotation, 1);
                transform.parent = characterPerformingBackStab.characterCombatManager.backstabReceiverTransform;
                transform.localPosition = characterPerformingBackStab.characterCombatManager.backstabReceiverTransform.localPosition;
                transform.parent = null;
                yield return new WaitForSeconds(0.05f);
            }
        }
        IEnumerator ForceMoveCharacterToEnemyRipostePosition(CharacterManager characterPerformingRiposte)
        {
            for (float timer = 0.05f; timer < 0.5f; timer = timer + 0.05f)
            {
                Quaternion riposteRotation = Quaternion.LookRotation(-characterPerformingRiposte.transform.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, riposteRotation, 1);
                transform.parent = characterPerformingRiposte.characterCombatManager.riposteReceiverTransform;
                transform.localPosition = characterPerformingRiposte.characterCombatManager.riposteReceiverTransform.localPosition;
                transform.parent = null;
                yield return new WaitForSeconds(0.05f);
            }
        }
        public void GetBackStabbed(CharacterManager characterPerformingBackStab)
        {
            //TO-DO: Play Sound FX
            character.isBeingBackstabbed = true;

            //BELOW CODE: Force Lock position
            StartCoroutine(ForceMoveCharacterToEnemyBackStabPosition(characterPerformingBackStab));

            character.characterAnimatorManager.PlayTargetActionAnimation("Back Stabbed", true);
        }
        public void GetRiposted(CharacterManager characterPerformingRiposte)
        {
            //TO-DO: Play Sound FX
            character.isBeingRiposted = true;

            //BELOW CODE: Force Lock position
            StartCoroutine(ForceMoveCharacterToEnemyRipostePosition(characterPerformingRiposte));

            character.characterAnimatorManager.PlayTargetActionAnimation("Riposted", true);
        }
        public void AttemptBackStabOrRiposte()
        {
            if (character.isPerformingAction)
                return;

            if (character.characterStatsManager.currentStamina <= 0)
                return;

            RaycastHit hit;

            if (Physics.Raycast(character.criticalAttackRayCastStartPoint.transform.position, character.transform.TransformDirection
                (Vector3.forward), out hit, criticalAttackRange, characterLayer))
            {
                CharacterManager enemyCharacter = hit.transform.GetComponent<CharacterManager>();
                Vector3 directionFromCharacterToEnemy = transform.position - enemyCharacter.transform.position;
                float dotValue = Vector3.Dot(directionFromCharacterToEnemy, enemyCharacter.transform.forward);

                Debug.Log("CURRENT DOT VALUE: " % WRLD_COLORIZE_EDITOR.Blue % WRLD_FONT_FORMAT.Bold + dotValue);

                if (enemyCharacter.canBeRiposted)
                {
                    if (dotValue <= 1.2f && dotValue >= 0.6f)
                    {
                        //BELOW CODE: Attempt riposte
                        AttemptRiposte(hit);
                        return;
                    }
                }

                if (enemyCharacter.canBeBackstabbed)
                {
                    if (dotValue >= -0.7f && dotValue <= -0.6f) // <= less than equal to
                    {
                        //BELOW CODE: Attempt backstab
                        AttemptBackstab(hit);
                    }
                }
            }

        }
        private void AttemptBackstab(RaycastHit hit)
        {
            CharacterManager enemyCharacter = hit.transform.GetComponent<CharacterManager>();

            if (enemyCharacter != null)
            {
                if (!enemyCharacter.isBeingBackstabbed || !enemyCharacter.isBeingRiposted)
                {
                    //BELOW CODE: Character will not be damaged whilst being critically damaged
                    character.characterAnimatorManager.EnableIsInvulnerable();
                    character.isPerformingBackstab = true;

                    character.characterAnimatorManager.PlayTargetActionAnimation("Back Stab", true);

                    float criticalDamage = (character.characterInventoryManager.rightHandWeapon.criticalDamageMultiplier
                        * (character.characterInventoryManager.rightHandWeapon.physicalDamage));

                    int roundedCriticalDamage = Mathf.RoundToInt(criticalDamage);
                    enemyCharacter.characterCombatManager.pendingCriticalDamage = roundedCriticalDamage;
                    enemyCharacter.characterCombatManager.GetBackStabbed(character);
                }
            }
        }
        private void AttemptRiposte(RaycastHit hit)
        {
            CharacterManager enemyCharacter = hit.transform.GetComponent<CharacterManager>();

            if (enemyCharacter != null)
            {
                if (!enemyCharacter.isBeingBackstabbed || !enemyCharacter.isBeingRiposted)
                {
                    //BELOW CODE: Character will not be damaged whilst being critically damaged
                    character.characterAnimatorManager.EnableIsInvulnerable();
                    character.isPerformingRiposte = true;

                    character.characterAnimatorManager.PlayTargetActionAnimation("Riposte", true);

                    float criticalDamage = (character.characterInventoryManager.rightHandWeapon.criticalDamageMultiplier
                        * (character.characterInventoryManager.rightHandWeapon.physicalDamage));

                    int roundedCriticalDamage = Mathf.RoundToInt(criticalDamage);
                    enemyCharacter.characterCombatManager.pendingCriticalDamage = roundedCriticalDamage;
                    enemyCharacter.characterCombatManager.GetRiposted(character);
                }
            }
        }
        public virtual void DrainStaminaBasedOnAttack()
        {

        }
        public virtual void SuccessfullyCastSkill()
        {
            character.characterSkillManager.currentSkillBeingUsed.SuccessfullyCastSkill(character);
        }
        public virtual void EnableCanBeParried()
        {
            character.canBeParried = true;
        }
        public virtual void DisableCanBeParried()
        {
            character.canBeParried = false;
        }
        public void ApplyPendingDamage()
        {
            checkDealthAttack = true;
            character.characterStatsManager.TakeDamageNoAnimation(pendingCriticalDamage, 0); // Enable this for damage
        }
        public void OnGUI()
        {
            if(checkDealthAttack == true)
            {
                GUILayout.Label("Character being critically attacked. Deal Damage to kill the character! (TO-DO)");
                StartCoroutine(DisableGUILabel());
            }
            else
            {
                GUILayout.Label("");
            }
        }
        IEnumerator DisableGUILabel()
        {
            yield return new WaitForSeconds(5f);
            checkDealthAttack = false;
        }
        public virtual void SetBlockingAbsorptionsFromBlockingWeapon()
        {
            if(character.isUsingRightHand)
            {
                character.characterStatsManager.blockingPhysicalDamageAbsorption = character.characterInventoryManager.rightHandWeapon.physicalBlockingDamageAbsorption;
                character.characterStatsManager.blockingFireDamageAbsorption = character.characterInventoryManager.rightHandWeapon.fireBlockingDamageAbsorption;
                character.characterStatsManager.blockingStabilityRating = character.characterInventoryManager.rightHandWeapon.stability;
            }
            else if(character.isUsingLeftHand)
            {
                character.characterStatsManager.blockingPhysicalDamageAbsorption = character.characterInventoryManager.leftHandWeapon.physicalBlockingDamageAbsorption;
                character.characterStatsManager.blockingFireDamageAbsorption = character.characterInventoryManager.leftHandWeapon.fireBlockingDamageAbsorption;
                character.characterStatsManager.blockingStabilityRating = character.characterInventoryManager.leftHandWeapon.stability;
            }
        }
    }
}