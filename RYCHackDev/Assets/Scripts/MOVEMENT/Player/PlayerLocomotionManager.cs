 using UnityEngine;

namespace Adnan.RYCHack
{
    public class PlayerLocomotionManager : CharacterLocomotionManager
    {
        private PlayerManager player; // Reference to the Player Input Manager script
        
        [Header("PLAYER RIGIDBODY")]
        public Rigidbody rigidBody; // Rigidbody component of the player

        [HideInInspector] public float verticalMovement; // Value of the vertical movement
        [HideInInspector] public float horizontalMovement; // Value of the horizontal movement
        [HideInInspector] public float moveAmount; // Value for the movement amount
      
        [Header("PLAYER STATS")] 
        private Vector3 targetRotationDirection; // Target direction value of the player
        private Vector3 lockOnRotationDirection; // Target direction value when player is locked on
        [SerializeField] private float movementSpeed = 2; // Float value which contains the player's movement speed
        [SerializeField] private float rotationSpeed = 15; // Float value which contains the player's rotation speed
        [SerializeField] private float walkingSpeed = 5; // Float value which contains the player's walking speed
        [SerializeField] private float sprintSpeed; // Float value which contains the player's sprint speed
        
        [Header("JUMP SETTINGS")]
        [SerializeField] private float jumpStaminaCost = 15; // Stamina cost value for reduction of stamina after jumping 
        [SerializeField] private float jumpHeight = 4; // Jump height value for the player
        [SerializeField] private float jumpForwardSpeed = 5; // Forward jump speed for the player
        [SerializeField] private float freeFallSpeed = 2; // Free Fall speed for the player
        private Vector3 jumpDirection; // Direction Value where you will Jump the player
        
        [Header("DODGE SETTINGS")]
        [SerializeField] private int dodgeStaminaCost = 15; // The cost of stamina after every roll
        private Vector3 dodgeDirection; // Direction value where you will roll the player

        [Header("SPRINT SETTINGS")]
        [SerializeField] private float sprintStaminaCost = 0.5f; //T he cost of stamina after every sprinting
        protected override void Awake()
        {
            base.Awake();  
            player = GetComponent<PlayerManager>();
            rigidBody = GetComponent<Rigidbody>();
        }
        protected override void Update()
        {
            base.Update();
            UpdateAnimatorMovementParameters();
        }
        private void UpdateAnimatorMovementParameters()
        {
            player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, player.isSprinting);
        }  
        public void UseAllMovement()
        {
            // BELOW CODE: Grounded movement
            UseGroundedMovement();
            // BELOW CODE: Player rotation [Aerial Movement]
            UseRotation();
            // TO-DO: Jumping movement [Aerial Movement]
            UseJumpingMovement();
            // TO-DO: Player Falling [Aerial Movement]
            UseFreeFallMovement();
        }
        private void GetMovementValues()
        {
            verticalMovement = player.playerInputManager.verticalInput;
            horizontalMovement = player.playerInputManager.horizontalInput;
            moveAmount = player.playerInputManager.moveAmount;
        }
        private void UseGroundedMovement()
        {
            if (!player.canMove || player.isPerformingAction || !player.isGrounded)
                return; // To stop the player from moving while interacting in the falling

            GetMovementValues();
            MovePlayer();
        }
        private void MovePlayer()
        {
            float speed = player.isSprinting ? sprintSpeed : player.playerInputManager.moveAmount > 0.5f ? movementSpeed : walkingSpeed;
            Vector3 moveDirection = CalculateMoveDirection();

            player.characterController.Move(moveDirection * speed * Time.deltaTime);

            if (player.isSprinting)
            {
                player.playerStatsManager.DeductSprintingStamina(Mathf.RoundToInt(sprintStaminaCost));
            }
        }
        private Vector3 CalculateMoveDirection()
        {
            Vector3 cameraForward = player.playerCameraManager.transform.forward;
            Vector3 cameraRight = player.playerCameraManager.transform.right;

            Vector3 moveDirection = (cameraForward * verticalMovement) + (cameraRight * horizontalMovement);
            moveDirection.y = 0;
            moveDirection.Normalize();

            return moveDirection;
        }
        private void UseJumpingMovement()
        {
            if (player.isJumping)
            {
                player.characterController.Move(jumpDirection * jumpForwardSpeed * Time.deltaTime);
            }
        }
        private void UseFreeFallMovement()
        {
            if (!player.isGrounded)
            {
                Vector3 freeFallDirection = CalculateMoveDirection();
                freeFallDirection.y = 0;

                player.characterController.Move(freeFallDirection * freeFallSpeed * Time.deltaTime);
            }
        }
        private void UseRotation()
        {
            if (!player.canRotate)
                return;

            Vector3 rotationDirection = player.playerInputManager.lockOnFlag ? CalculateLockOnRotationDirection() : CalculateRotationDirection();

            Quaternion newRotation = Quaternion.LookRotation(rotationDirection);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);

            transform.rotation = targetRotation;
        }
        private Vector3 CalculateLockOnRotationDirection()
        {
            if (player.isDead)
                return transform.position;

            Vector3 lockOnDirection = player.playerCameraManager.currentLockOnTarget.transform.position - transform.position;
            lockOnDirection.y = 0;
            lockOnDirection.Normalize();
            return lockOnDirection;
        }
        private Vector3 CalculateRotationDirection()
        {
            Vector3 rotationDirection = player.playerInputManager.lockOnFlag ? CalculateLockOnRotationDirection() : CalculateMoveDirection();

            if (rotationDirection == Vector3.zero)
            {
                rotationDirection = transform.forward;
            }

            return rotationDirection;
        }
        public void UseSprinting()
        {
            if (player.isPerformingAction || player.playerStatsManager.currentStamina <= 0)
            {
                // BELOW CODE: If we are performing an action and our stamina is 0 then return
                player.isSprinting = false;
                return;
            }

            // BELOW CODE: If player moving in sprinting value then set sprinting to true
            if(moveAmount >= 0.5)
            {
                player.isSprinting = true;
            }
            else 
            {
                player.isSprinting = false;
            }

            // BELOW CODE: If player is sprinting, then deduct sprinting stamina from player
            if (player.isSprinting)
            {
                player.playerStatsManager.DeductSprintingStamina(sprintStaminaCost);
            }
        }
        public void AttemptToPerformDodge()
        {
            // BELOW CODE: If we are performing an action and our stamina is 0 then return
            if (player.isPerformingAction || !player.canRoll || player.playerStatsManager.currentStamina <= 0)
                return;

            if (player.playerInputManager.moveAmount > 0)
            {
                CalculateDodgeDirection();
                PerformRoll(); // If we are moving then attempting to dodge we perform a roll
            }
            else // If we stationary, we perform  a backstep
            {
                // BELOW CODE: Use backstep animation
                switch (player.playerStatsManager.encumbranceLevel)
                {
                    case EncumbranceLevel.Light:
                        player.playerAnimatorManager.PlayTargetActionAnimation("Backstep", true, true);
                        break;
                    case EncumbranceLevel.Medium:
                        player.playerAnimatorManager.PlayTargetActionAnimation("Backstep", true, true);
                        break;
                    case EncumbranceLevel.Heavy:
                        player.playerAnimatorManager.PlayTargetActionAnimation("Backstep", true, true);
                        break;              
                }

                player.playerAnimatorManager.EraseHandIKForWeapon();
                player.playerStatsManager.DeductStamina(dodgeStaminaCost);
            }
        }
        private void CalculateDodgeDirection()
        {
            dodgeDirection = CalculateMoveDirection();
            dodgeDirection.Normalize();
        }
        private void PerformRoll()
        {
            Quaternion playerRotation = Quaternion.LookRotation(dodgeDirection);
            player.transform.rotation = playerRotation;

            // BELOW CODE: Use roll animation
            switch (player.playerStatsManager.encumbranceLevel)
            {
                case EncumbranceLevel.Light:
                    player.playerAnimatorManager.PlayTargetActionAnimation("Rolling", true, true);
                    break;
                case EncumbranceLevel.Medium:
                    player.playerAnimatorManager.PlayTargetActionAnimation("Rolling", true, true);
                    break;
                case EncumbranceLevel.Heavy:
                    player.playerAnimatorManager.PlayTargetActionAnimation("Heavy_Rolling", true, true);
                    break;              
            }

            player.playerAnimatorManager.EraseHandIKForWeapon();
            player.playerStatsManager.DeductStamina(dodgeStaminaCost);
        }
        public void AttemptToPerformJump()
        {
            // BELOW CODE: If we are performing an action and our stamina is 0 then return
            if (player.isPerformingAction || player.playerStatsManager.currentStamina <= 0 || player.isJumping || !player.isGrounded)
                return;

            // BELOW CODE: Use jump animation
            player.playerAnimatorManager.PlayTargetActionAnimation("Jump", false);
            player.playerAnimatorManager.EraseHandIKForWeapon();
            player.isJumping = true;
            player.playerStatsManager.DeductStamina(jumpStaminaCost);

            CalculateJumpDirection();
            ApplyJumpVelocity();
        }
        private void CalculateJumpDirection()
        {
            jumpDirection = CalculateMoveDirection();

            if (jumpDirection != Vector3.zero)
            {
                // BELOW CODE: If player is sprinting, Jump direction will be at full distance
                if (player.isSprinting)
                {
                    jumpDirection *= 1;
                }
                // BELOW CODE: If player is running, Jump direction will be at half distance
                else if (player.playerInputManager.moveAmount > 0.5f)
                {
                    jumpDirection *= 0.5f;
                }
                // BELOW CODE: If player is running, Jump direction will be at quarter distance
                else
                {
                    jumpDirection *= 0.25f;
                }
            }
        }
        private void ApplyJumpVelocity()
        {
            // BELOW CODE: Apply an upward velocity
            yVelocity.y = Mathf.Sqrt(jumpHeight * -2 * gravityForce);
        }
        
        public void PassThroughFog()
        {
            // BELOW CODE: If we are performing an action and our stamina is 0 then return
            if (player.isPerformingAction || player.playerStatsManager.currentStamina <= 0)
                return;

            CalculateDodgeDirection();
            PerformRoll(); // If we are moving then attempting to dodge we perform a roll
        }
    }
}