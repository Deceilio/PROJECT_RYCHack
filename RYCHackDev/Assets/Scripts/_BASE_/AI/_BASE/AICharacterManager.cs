using UnityEngine;
using UnityEngine.AI;

namespace Deceilio.Psychain
{
    public class AICharacterManager : CharacterManager
    {
        [HideInInspector] public AICharacterBossManager boss; // Reference to the AI Character Boss Manager script
        [HideInInspector] public AICharacterLocomotionManager aiCharacterLocomotionManager; // Reference to the AI Character Locomotion Manager script 
        [HideInInspector] public AICharacterStatsManager aiCharacterStatsManager; // Reference to the AI Character Stats Manager script
        [HideInInspector] public AICharacterAnimatorManager aiCharacterAnimatorManager; // Reference to the AI Character Animator Manager script
        [HideInInspector] public AICharacterCombatManager aiCharacterCombatManager; // Reference to the AI Character Combat Manager script
        [HideInInspector] public AICharacterInventoryManager aiCharacterInventoryManager; // Reference to the AI Character Inventory Manager script
        [HideInInspector] public AICharacterWeaponSlotManager aiCharacterWeaponSlotManager; // Reference to the AI Character Weapon Slot Manager script
        [HideInInspector] public AICharacterSoundFXManager aiCharacterSoundFXManager; // Reference to the AI Character Sound FX Manager script 
        [HideInInspector] public AICharacterStatesManager aiCharacterStatesManager; // Reference to the AI Character States Manager Script
        [HideInInspector] public AICharacterEquipmentManager aiCharacterEquipmentManager; // Reference to the AI Character Equipment Manager Script

        [Header("A.I DATA")]
        public WRLD_STATE currentState; // Current state of the AI Character
        public CharacterManager currentTarget; // Reference to the current target
        public NavMeshAgent navMeshAgent; // Reference to the nav mesh agent component
        public Rigidbody rigidBody; // Reference to the rigidbody component

        [Header("A.I MOVEMENT SETTINGS")]
        public float rotationSpeed = 15; // Rotation speed of the A.I character

        [Header("A.I SETTINGS")]
        public bool isPerformingFunctions; // Checks if the AI Character is performing any functions or not
        public float detectionRadius = 20f; // Size of the detection radius to detect the target
        // The higher, and lower, respectively these angles are , the greator detection field of view of the AI Character
        public float maximumDetectionAngle = 50; // Maximum detection angle for AI Character detection
        public float minimumDetectionAngle = -50; // Minimum detection angle for AI Character detection
        public float currentRecoveryTime = 0; // Recovery time of the current AI Character attack
        public float maximumAggroRadius = 1.5f; // Maximum attack range for the AI Character
        public float stoppingDistance = 1.2f; // Stopping distance for the AI Character

        //These settings only effect AI Character with humanoid states
        [Tooltip("Number from 0-100. 100% will generate the below actions EVERY TIME, 0 will generate the below action 0% of the time.")]
        [Header("ADVANCE A.I SETTINGS")] 
        public bool allowAIToPerformBlock; // Check if the AI Character can perform block or not
        public int blockLikelyHood = 50; // Number from 0-100. 100 will generate
        public bool allowAIToPerformDodge; // Check if the AI Character can perform dodge or not
        public int dodgeLikelyHood = 50; // Chances of using dodge by the AI Character
        public bool allowAIToPerformParry; // Check if the AI Character can perform parry or not
        public int parryLikelyHood = 50; // Chances of using parry by the AI Character

        [Header("AI COMBAT SETTINGS")]
        public bool allowAIToPerformCombos; //Check if the AI Character is allowed to perform combos or not
        public bool isPhaseShifting; // Checks if the phase is shifting or not (only for boss)
        public float comboLikelyHood; // Condition if the combo can happen or not
        public AICombatStyle combatStyle; // Which type of combat does AI Character perform

        [Header("A.I TARGET INFORMATION")]
        public float distanceFromTarget; //The Distance from Player to AI Character
        public Vector3 targetsDirection; //Direction of the Target
        public float viewableAngle; //Viewable Angle of the AI Character

        [Header("COHORT FOLLOW DATA")]
        public float maxDistanceFromCohort; // Max distance A.I can go from our player/companion
        public float minimumDistanceFromCohort; // Minimum distance A.I have to be from our player/companion
        public float returnDistanceFromCohort = 2; // How close A.I can get to our player/companion when we return the A.I
        public float distanceFromCohort; // Distance from the companion/player
        public CharacterManager cohort; // Reference to the Character Manager for particular player/companion
        private void Start()
        {
            rigidBody.isKinematic = false;
        }      
        protected override void Awake() 
        {
            base.Awake();
            rigidBody = GetComponent<Rigidbody>();
            navMeshAgent = GetComponentInChildren<NavMeshAgent>();
            navMeshAgent.enabled = false;
            boss = GetComponent<AICharacterBossManager>();
            aiCharacterLocomotionManager = GetComponent<AICharacterLocomotionManager>();
            aiCharacterStatsManager = GetComponent<AICharacterStatsManager>();
            aiCharacterAnimatorManager = GetComponent<AICharacterAnimatorManager>();
            aiCharacterCombatManager = GetComponent<AICharacterCombatManager>();
            aiCharacterInventoryManager = GetComponent<AICharacterInventoryManager>();
            aiCharacterWeaponSlotManager = GetComponent<AICharacterWeaponSlotManager>();
            aiCharacterSoundFXManager = GetComponent<AICharacterSoundFXManager>();
            aiCharacterStatesManager = GetComponent<AICharacterStatesManager>();
            aiCharacterEquipmentManager = GetComponent<AICharacterEquipmentManager>();
        }
        protected override void Update() 
        {
            base.Update();
            UseRecoveryTimer();
            UseStateMachine();

            isPhaseShifting = animator.GetBool("isPhaseShifting");

            if(currentTarget != null)
            {
                // BELOW CODE: Check for attack range
                distanceFromTarget = Vector3.Distance(currentTarget.transform.position, transform.position);
                targetsDirection = currentTarget.transform.position - transform.position;
                viewableAngle = Vector3.Angle(targetsDirection, transform.forward);
            }

            if(cohort != null)
            {
                distanceFromCohort = Vector3.Distance(cohort.transform.position, transform.position);
            }
        }
        protected override void FixedUpdate()
        {
            base.FixedUpdate();;
        }  
        private void LateUpdate()
        {
            navMeshAgent.transform.localPosition = Vector3.zero;
            navMeshAgent.transform.localRotation = Quaternion.identity;
        }
        private void UseStateMachine()
        {
            if(currentState != null)
            {
                WRLD_STATE nextState = currentState.Tick(this);

                if(nextState != null)
                {
                    SwitchToNextState(nextState);
                }
            }
        }
        private void SwitchToNextState(WRLD_STATE state)
        {
            currentState = state;
        }
        private void UseRecoveryTimer()
        {
            if(currentRecoveryTime > 0)
            {
                currentRecoveryTime -= Time.deltaTime;
            }

            if(isPerformingFunctions)
            {
                if(currentRecoveryTime <= 0)
                {
                    isPerformingFunctions = false;
                }
            }
        }
        private void OnDrawGizmosSelected()
        {
            Vector3 fovLine1 = Quaternion.AngleAxis(maximumDetectionAngle, transform.up) * transform.forward * detectionRadius;
            Vector3 fovLine2 = Quaternion.AngleAxis(minimumDetectionAngle, transform.up) * transform.forward * detectionRadius;
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, fovLine1);
            Gizmos.DrawRay(transform.position, fovLine2);
        }
    }
}

