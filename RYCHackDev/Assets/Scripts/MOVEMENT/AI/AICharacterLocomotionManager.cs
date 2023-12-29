using UnityEngine;
using UnityEngine.AI;

namespace Deceilio.Psychain
{
    public class AICharacterLocomotionManager : CharacterLocomotionManager
    {
        AICharacterManager aiCharacter; // Reference to the A.I Character Manager script
        [Header("PLAYER MOVEMENT INPUT")]
        [SerializeField] Vector2 movementInput; // Storing the data for refrencing the input values of the movement
        public float verticalInput; // Vertical input value for the player movement
        public float horizontalInput; // Horizontal input value for the player movement

        protected override void Awake()
        {
            base.Awake();
            aiCharacter = GetComponent<AICharacterManager>();
        }
    }
}