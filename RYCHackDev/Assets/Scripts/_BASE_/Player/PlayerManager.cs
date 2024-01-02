using UnityEngine;
using UnityEngine.SceneManagement;

namespace Adnan.RYCHack
{
    public class PlayerManager : CharacterManager
    {
        [HideInInspector] public PlayerCameraManager playerCameraManager; // Reference to the Player Camera Manager script
        [HideInInspector] public PlayerInputManager playerInputManager; // Reference to the Player Input Manager script
        [HideInInspector] public PlayerLocomotionManager playerLocomotionManager; // Reference to the Player Locomotion Manager script
        [HideInInspector] public PlayerAnimatorManager playerAnimatorManager; // Reference to the Player Animator Manager script      
        [HideInInspector] public PlayerSoundFXManager playerSoundFXManager; // Reference to the Player Sound FX Manager script   
        
        protected override void Awake() // Assigning all the script automatically when the game is load
        {
            base.Awake();
            // Fixed camera issue by using FindObjectOfType instead of CameraHandler.singelton; Hurray!!
            playerCameraManager = FindObjectOfType<PlayerCameraManager>(); 
            playerInputManager = GetComponent<PlayerInputManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            playerSoundFXManager = GetComponent<PlayerSoundFXManager>();;
        }
        protected override void Update() // Calling interaction and states, etc in Update
        {
            base.Update();
            playerInputManager.UseAllInputs();
            playerLocomotionManager.UseAllMovement();
        } 
        public void LateUpdate() // Disabling input in late update
        {    
            playerInputManager.tapYInput = false;
            playerInputManager.inventoryInput = false;

            float delta = Time.fixedDeltaTime;
            
            if (playerCameraManager != null)
            {
                playerCameraManager.FollowTarget(delta);
                playerCameraManager.UseCameraRotation(delta, playerInputManager.mouseX, playerInputManager.mouseY);
            }
        }
        public void OpenChestInteraction(Transform playerStandsHereWhenOpeningChest) // Interaction for chest items
        {
            playerLocomotionManager.rigidBody.velocity = Vector3.zero; // Stops the player from ice skating
            transform.position = playerStandsHereWhenOpeningChest.transform.position;
            playerAnimatorManager.PlayTargetActionAnimation("Open Chest", true);
        }
        public GameObject FindInActiveObjectByName(string name)
        {
            Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
            for (int i = 0; i < objs.Length; i++)
            {
                if (objs[i].hideFlags == HideFlags.None)
                {
                    if (objs[i].name == name)
                    {
                        return objs[i].gameObject;
                    }
                }
            }
            return null;
        }
        public void PassThroughFogWallInteraction(Transform fogWallEntrance)
        {
             playerLocomotionManager.rigidBody.velocity = Vector3.zero; // Stops the player from ice skating

             Vector3 rotationDirection = fogWallEntrance.transform.forward;
             Quaternion turnRotation = Quaternion.LookRotation(rotationDirection);
             transform.rotation = turnRotation; // Rotate over time so it does not look rigid

             playerAnimatorManager.PlayTargetActionAnimation("Pass Through Fog", true, true, true, true);
        }
    }
}