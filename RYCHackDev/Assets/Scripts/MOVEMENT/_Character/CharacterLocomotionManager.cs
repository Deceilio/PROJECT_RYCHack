using UnityEngine;

namespace Deceilio.Psychain
{
    public class CharacterLocomotionManager : MonoBehaviour
    {
        [HideInInspector] public CharacterManager character; // Reference to the Character Manager script
        
        [Header("MOVEMENT")]
        public Vector3 moveDirection; // Vector3 for the player's move directions values

        [Header("GRAVITY SETTINGS")]
        [SerializeField] protected float gravityForce = -25; //G ravity force for the Jumping
        [SerializeField] public LayerMask groundLayer; // Layer mask for ignoring the ground check
        [SerializeField] float groundCheckSphereRadius = 1f; // Radius of the Sphere for the ground check
        [SerializeField] protected Vector3 yVelocity; // The force at which our character is pulled up or down (Jumping or Falling)
        [SerializeField] protected float groundedYVelocity = -20; // The force at which our character is sticking to the ground while they are grounded
        [SerializeField] private protected float fallStartYVelocity = -5; // The force at which our character begins to fall when they are ungrounded (Rises as they fall longer)
        protected bool fallingVelocityHasBeenSet = false; // Checks if the falling velocity has been set or not
        public float inAirTimer = 0; // Timer to depect the character in the air
        
        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }
        protected virtual void Start()
        {

        }
        protected virtual void Update()
        {
            UseGroundCheck();

            if(character.isGrounded)
            {
                // BELOW CODE: If character is not attempting to jump or move forward
                if(yVelocity.y < 0)
                {
                    inAirTimer = 0;
                    fallingVelocityHasBeenSet = false;
                    yVelocity.y = groundedYVelocity;
                }
            }
            else
            {
                // BELOW CODE: If character is not jumping, and character falling velocity has not been set
                if(character.isJumping && !fallingVelocityHasBeenSet)
                {
                    fallingVelocityHasBeenSet = true;
                    yVelocity.y = fallStartYVelocity;
                }

                inAirTimer = inAirTimer + Time.deltaTime;
                character.animator.SetFloat("inAirTimer", inAirTimer);

                yVelocity.y += gravityForce * Time.deltaTime;
            }

            // BELOW CODE: There should also be some force applied to y velocity
            character.characterController.Move(yVelocity * Time.deltaTime); 
        }
        public virtual void UseGroundCheck()
        {
            character.isGrounded = Physics.CheckSphere(character.transform.position, groundCheckSphereRadius, groundLayer);
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawSphere(transform.position, groundCheckSphereRadius);
        }
    }
}
