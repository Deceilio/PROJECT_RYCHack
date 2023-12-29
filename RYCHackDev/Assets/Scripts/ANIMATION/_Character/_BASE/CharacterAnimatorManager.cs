using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Animations.Rigging;

namespace Deceilio.Psychain
{
    public class CharacterAnimatorManager : MonoBehaviour
    {
        [HideInInspector] public CharacterManager character; // Reference to the Character Manager script

        int horizontal; // For horizontal value
        int vertical; // For vertical value

        [Header("ANIMATION RIGGING")]
        protected RigBuilder rigBuilder; // RigBuilder component to rig the hands
        public TwoBoneIKConstraint leftHandConstraint; // Reference to the TwoBoneIKConstraint component for left hand
        public TwoBoneIKConstraint rightHandConstraint; // Reference to the TwoBoneIKConstraint component for right hand
        bool handIKWeightsReset = false; // Reset the ik hands weight 

        [Header("DAMAGE ANIMATION")]
        [HideInInspector] public string Damage_Forward_Small_01 = "Damage_Forward_Small_01";
        [HideInInspector] public string Damage_Back_Small_01 = "Damage_Back_Small_01";
        [HideInInspector] public string Damage_Left_Small_01 = "Damage_Left_Small_01";
        [HideInInspector] public string Damage_Right_Small_01 = "Damage_Right_Small_01";

        [HideInInspector] public List<string> Damage_Animations_Small_Forward = new List<string>();
        [HideInInspector] public List<string> Damage_Animations_Small_Backward = new List<string>();
        [HideInInspector] public List<string> Damage_Animations_Small_Left = new List<string>();
        [HideInInspector] public List<string> Damage_Animations_Small_Right = new List<string>();
        
        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
            rigBuilder = GetComponent<RigBuilder>();

            horizontal = Animator.StringToHash("Horizontal");
            vertical = Animator.StringToHash("Vertical");
        }
        protected virtual void Start()
        {
            Damage_Animations_Small_Forward.Add(Damage_Forward_Small_01);
            //Damage_Animations_Small_Forward.Add(Damage_Forward_Small_02); //If you have variations of Damage Animation do like thid
            Damage_Animations_Small_Backward.Add(Damage_Back_Small_01);
            Damage_Animations_Small_Left.Add(Damage_Left_Small_01);
            Damage_Animations_Small_Right.Add(Damage_Right_Small_01);
        }
        public string GetRandomDamageAnimationFromList(List<string> animationList)
        {
            int randomValue = Random.Range(0, animationList.Count);

            return animationList[randomValue];
            //if (animationList[randomValue] == lastDamageAnimationPlayed) //If you don't want same animation to played you can use this method
        }
        public virtual void OnAnimatorMove()
        {
            if(character.isPerformingAction == false)
                return;
                
            if(character.applyRootMotion)
            {
                // BELOW CODE: Take the rotation from particular animation and apply to the character rotation
                Vector3 velocity = character.animator.deltaPosition;
                character.characterController.Move(velocity);
                character.transform.rotation *= character.animator.deltaRotation;
            }
        }
        public void UpdateAnimatorMovementParameters(float horizontalMovement, float verticalMovement, bool isSprinting)
        {
            // BELOW CODE: Adding the values
            float horizontalAmount = horizontalMovement;
            float verticalAmount = verticalMovement;  

            if(isSprinting)
            {
                verticalAmount = 2;
            }

            character.animator.SetFloat(horizontal, horizontalAmount, 0.1f, Time.deltaTime);
            character.animator.SetFloat(vertical, verticalAmount, 0.1f, Time.deltaTime);
            // BELOW CODE: Adding the values (Different method)
            //float snappedHorizontal = 0;
            //float snappedVertical = 0;

            #region Horizontal
            // BELOW CODE: CHAIN AROUND THE HORIZONTAL MOVEMENT
            //if (horizontalMovement > 0 && horizontalMovement <= 0.5f)
            //{
            //    snappedHorizontal = 0.5f;
            //}
            //else if(horizontalMovement > 0.5f && horizontalMovement <= 1)
            //{
            //    snappedHorizontal = 1;
            //}
            //else if(horizontalMovement < 0 && horizontalMovement >= -0.5f)
            //{
            //    snappedHorizontal = -0.5f;
            //}
            //else if(horizontalMovement < -0.5f && horizontalMovement >= -1)
            //{
            //    snappedHorizontal = -1;
            //}
            //else
            //{
            //    snappedHorizontal = 0;
            //}
            #endregion

            #region Vertical
            // BELOW CODE: CHAIN AROUND THE VERTICAL MOVEMENT
            //if (verticalMovement > 0 && verticalMovement <= 0.5f)
            //{
            //    snappedVertical = 0.5f;
            //}
            //else if (verticalMovement > 0.5f && verticalMovement <= 1)
            //{
            //    snappedVertical = 1;
            //}
            //else if (verticalMovement < 0 && verticalMovement >= -0.5f)
            //{
            //    verticalMovement = -0.5f;
            //}
            //else if (verticalMovement < -0.5f && verticalMovement >= -1)
            //{
            //    snappedVertical = -1;
            //}
            //else
            //{
            //    snappedVertical = 0;
            //}

            #endregion
            //character.animator.SetFloat("Horizontal", snappedHorizontal);
            //character.animator.SetFloat("Vertical", snappedVertical);
        }
        public void PlayTargetActionAnimation(
            string targetAnim,
            bool isPerformingAction,
            bool applyRootMotion = true,
            bool canRotate = false,
            bool canMove = false,
            bool canRoll = false)
        {
            character.applyRootMotion = applyRootMotion;
            character.animator.CrossFade(targetAnim, 0.2f);
            // BELOW CODE: Can be used to stop character from attempting a new action
            // BELOW CODE: Example if you get damaged and perform damage animation
            // BELOW CODE: The below flag will turn true id player is stunned
            // BELOW CODE: We can then check for the flag before attempting a new action
            character.isPerformingAction = isPerformingAction;
            character.canRotate = canRotate;
            character.canMove = canMove;
            character.canRoll = canRoll;
        }
        public void EnableCanMove()
        {
            character.canMove = true;
        }
        public void StartRotation()
        {
            character.canRotate = true;
        }
        public void StopRotation()
        {
            character.canRotate = false;
        }
        public void EnableCanRoll()
        {
            character.canRoll = true;
        }
        public virtual void EnableCombo()
        {
            character.animator.SetBool("canDoCombo", true);
        }
        public virtual void DisableCombo()
        {
            character.animator.SetBool("canDoCombo", false);
        }
        public void EnableIsInvulnerable()
        {
            character.animator.SetBool("isInvulnerable", true);
        }
        public void DisableIsInvulnerable()
        {
            character.animator.SetBool("isInvulnerable", false);
        }
        public void DisableCollision()
        {
            character.characterController.enabled = false;
        }
        public void EnableCollision()
        {
            character.characterController.enabled = true;
        }
        public void EnableIsParrying()
        {
            character.isParrying = true;
        }
        public void DisableIsParrying()
        {
            character.isParrying = false;
        }
        public virtual void SuccessfullyUseCurrentConsumable()
        {
            if(character.characterStatesManager.currentConsumableBeingUsed != null)
            {
                character.characterStatesManager.currentConsumableBeingUsed.SuccessfullyConsumeItem(character);
            }
        }
        public void AwardLinksOnDeath()
        {
            if (character.isBoss)
            {
                PlayerManager player = FindObjectOfType<PlayerManager>();
                UI_LinkCount linksCountUI = FindObjectOfType<UI_LinkCount>();

                if (player != null)
                {
                    player.playerStatsManager.AddLinks(character.characterStatsManager.linksAwaradedOnDeath);
                }

                if (linksCountUI != null)
                {
                    linksCountUI.ChangeIncreaseAmount(character.characterStatsManager.linksAwaradedOnDeath);
                }
            }
        }
        public virtual void SetHandIKForWeapon(RightHandIKTarget rightHandIKTarget, LeftHandIKTarget leftHandIKTarget, bool isTwoHandingWeapon)
        {
            // BELOW CODE: Check if the character is two handing the weapon or not
            if(isTwoHandingWeapon)
            {
                // BELOW CODE: If yes, then apply hand ik if needed
                // BELOW CODE: Assign the hand ik to targets
                rightHandConstraint.data.target = rightHandIKTarget.transform;
                rightHandConstraint.data.targetPositionWeight = 1; // Assign this from a weapon variable if we want
                rightHandConstraint.data.targetRotationWeight = 1;

                leftHandConstraint.data.target = leftHandIKTarget.transform;
                leftHandConstraint.data.targetPositionWeight = 1; // Assign this from a weapon variable if we want
                leftHandConstraint.data.targetRotationWeight = 1;                
            }
            else 
            {
                // BELOW CODE: If not, then disable handle ik for now
                rightHandConstraint.data.target = null;
                leftHandConstraint.data.target = null;
            }

            rigBuilder.Build();
        }
        public virtual void CheckHandIKWeight(RightHandIKTarget rightHandIK, LeftHandIKTarget leftHandIK, bool isTwoHandingWeapon)
        {
            if(character.isPerformingAction)
                return;

            if(handIKWeightsReset)
            {
                handIKWeightsReset = false;

                if(rightHandConstraint.data.target != null)
                {
                    rightHandConstraint.data.target = rightHandIK.transform;
                    rightHandConstraint.data.targetPositionWeight = 1;
                    leftHandConstraint.data.targetRotationWeight = 1; 
                }

                if(leftHandConstraint.data.target != null)
                {
                    leftHandConstraint.data.target = leftHandIK.transform;
                    leftHandConstraint.data.targetPositionWeight = 1;
                    leftHandConstraint.data.targetRotationWeight = 1; 
                }
            }
        }
        public virtual void EraseHandIKForWeapon()
        {
            // BELOW CODE: Reset all hand ik weights to 0
            handIKWeightsReset = true;

            if(rightHandConstraint.data.target != null)
            {
                rightHandConstraint.data.targetPositionWeight = 0;
                leftHandConstraint.data.targetRotationWeight = 0; 
            }

            if(leftHandConstraint.data.target != null)
            {
                leftHandConstraint.data.targetPositionWeight = 0;
                leftHandConstraint.data.targetRotationWeight = 0; 
            }
        }
    }
}
