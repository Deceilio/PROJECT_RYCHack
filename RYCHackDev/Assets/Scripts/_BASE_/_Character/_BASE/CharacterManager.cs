using UnityEngine;

namespace Deceilio.Psychain
{
    public class CharacterManager : MonoBehaviour
    {
        [HideInInspector] public Animator animator; // Reference to the animator component
        [HideInInspector] public CharacterController characterController; // Reference to the Character Controller component
        [HideInInspector] public CharacterAnimatorManager characterAnimatorManager; // Reference to the Character Animator Manager Script
        [HideInInspector] public CharacterInventoryManager characterInventoryManager; // Reference to the Character Inventory Script
        [HideInInspector] public CharacterCombatManager characterCombatManager; // Reference to the Character Combat Manager script
        [HideInInspector] public CharacterStatsManager characterStatsManager; // Reference to the Character Stats Script
        [HideInInspector] public CharacterWeaponSlotManager characterWeaponSlotManager; // Reference to the Character Weapon Slot Script
        [HideInInspector] public CharacterSkillManager characterSkillManager; // Reference to the Character Skill Script
        [HideInInspector] public CharacterLocomotionManager characterLocomotionManager; // Reference to the Character Locomotion Manager script
        [HideInInspector] public CharacterStatesManager characterStatesManager; // Reference to the Character States Manager script 
        [HideInInspector] public CharacterEquipmentManager characterEquipmentManager; // Reference to the Character Equipment Manager script 
        [HideInInspector] public CharacterSoundFXManager characterSoundFXManager; // Reference to the Character Sound FX Manager script    
        
        [Header("CHARACTER DATA")]
        public bool isBoss; // Checks if the character is boss character or not
        public bool isAICharacter; // Checks if the character is AI character or not

        [Header("LOCK ON TRANSFORM")]
        public Transform lockOnTransform; // Transform for lock on function

        [Header("RAYCASTS")]
        public Transform criticalAttackRayCastStartPoint; // Transform position of raycast used for the critical attacks
        
        [Header("MOVEMENT FLAGS")]
        public bool canMove = true; // Checks if the character can move or not
        public bool canRotate = true; // Checks if the character can rotate or not
        public bool canRoll = true; // Checks if the character can roll or not
        public bool isPerformingAction = false; // Checks if the character is performing action or not
        public bool isSprinting = false; // Checks if the character is sprinting or not
        public bool applyRootMotion = false;
        public bool isJumping = false;
        public bool isGrounded = true; // Checks if the character is touching the ground
        public bool isInvulerable; // Checks if the character is vulnerable or not
        public bool isDead; // Checks if the character is dead or not
        public bool isTwoHandingWeapon; // Checks if the character is two handing the weapon

        [Header("COMBO FLAGS")]
        public bool canDoCombo; // Checks if the character can do the combo attack
        public bool isUsingRightHand; // Checks if the character is using right hand
        public bool isUsingLeftHand; // Checks if the character is using left hand
        public bool canBeRiposted; // Checks if the character can riposte other character
        public bool canBeBackstabbed; // Checks if the character can backstab other character
        public bool canBeParried; // Checks if the character can parried other character 
        public bool isPerformingFullyChargedAttack; // Checks if the character is performing full charged Attack
        public bool isBeingBackstabbed; // Checks if the character is being backstabbed
        public bool isBeingRiposted; // Checks if the character is being riposted 
        public bool isPerformingBackstab; // Checks if the character is performing backstab
        public bool isPerformingRiposte; // Checks if the character is performing riposte
        public bool isParrying; // Checks if the character is parrying or not
        public bool isBlocking; // Checks if the character is blocking or not
        public bool isAttacking; // Check if the character is attacking or not
        
        [Header("SKILL FLAGS")]
        public bool isFiringSkill; // Checks if the character is firing skill or not

        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();   
            characterController = GetComponent<CharacterController>();
            characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
            characterStatsManager = GetComponent<CharacterStatsManager>();
            characterWeaponSlotManager = GetComponent<CharacterWeaponSlotManager>();
            characterCombatManager = GetComponent<CharacterCombatManager>();  
            characterEquipmentManager = GetComponent<CharacterEquipmentManager>();
            characterSkillManager = GetComponent<CharacterSkillManager>();
            characterInventoryManager = GetComponent<CharacterInventoryManager>();
            characterLocomotionManager = GetComponent<CharacterLocomotionManager>();
            characterStatesManager = GetComponent<CharacterStatesManager>();
            characterSoundFXManager = GetComponent<CharacterSoundFXManager>();
        }
        protected virtual void Update()
        {
            animator.SetBool("isGrounded", isGrounded); 
            animator.SetBool("isDead", isDead);
            animator.SetBool("isBlocking", isBlocking);
            animator.SetBool("isTwoHandingWeapon", isTwoHandingWeapon);
            isFiringSkill = animator.GetBool("isFiringSkill");
            canDoCombo = animator.GetBool("canDoCombo");
            isInvulerable = animator.GetBool("isInvulnerable");
            isPerformingFullyChargedAttack = animator.GetBool("isPerformingFullyChargedAttack");
            characterStatesManager.ProcessAllTimedStates();
        }
        protected virtual void FixedUpdate()
        {
            characterAnimatorManager.CheckHandIKWeight(characterWeaponSlotManager.rightHandIKTarget, characterWeaponSlotManager.leftHandIKTarget, isTwoHandingWeapon);
        }
        public virtual void UpdateWhichHandPlayerIsUsing(bool usingRightHand)
        {
            if(usingRightHand)
            {
                isUsingRightHand = true;
                isUsingLeftHand = false;
            }
            else
            {
                isUsingLeftHand = true;
                isUsingRightHand = false;
            }
        }
    }
}