using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

namespace Deceilio.Psychain
{
    public class PlayerInputManager : MonoBehaviour
    {
        PlayerControls playerControls; // Reference to the PlayerControls 
        PlayerManager player; // Reference to the Player Manager script

        [Header("PLAYER MOVEMENT INPUT")]
        [SerializeField] Vector2 movementInput; // Storing the data for refrencing the input values of the movement
        public float verticalInput; // Vertical input value for the player movement
        public float horizontalInput; // Horizontal input value for the player movement
        public float moveAmount; // The amount of movement value for the player input

        [Header("CAMERA INPUT")]
        private Vector2 cameraInput; // Vector2 for the player camera input
        public float mouseX; // Mouse X value for the player input
        public float mouseY; // Mouse Y value for the player input
        
        [Header("PLAYER ACTION INPUT")]
        [SerializeField] bool tapRBInput; // Input for the tap RB input (light attack) - (right bumper/shoulder)
        [SerializeField] bool tapRTInput; // Input for the tap RT input (heavy attack) - (right trigger)
        [SerializeField] bool tapLBInput; // Input for the tap LB input (use skill) - (left bumper/shoulder)
        [SerializeField] bool tapLTInput; // Input for the tap LT input (parry) - (left trigger)
        [SerializeField] bool holdRBInput; // Input for the hold RB input (critical attack) - (right bumper/shoulder)
        [SerializeField] bool holdRTInput; // Input for the hold RT input (charge attack) - (right trigger)
        [SerializeField] bool holdLBInput; // Input for the hold LB input (blocking) - (left bumper/shoulder)
        [SerializeField] bool holdLTInput; // Input for the hold LT input () - (left trigger)
        public bool tapXInput; // Input for conusming potions
        public bool tapYInput; // Input for examine or open items
        public bool holdYInput; // Input for 2 hand switch
        public bool dodgeInput = false; // Checks if the input for the dodge/roll is pressed or not
        [SerializeField] bool sprintInput = false; // Checks if the input for the sprint is pressed or not
        [SerializeField] bool jump_Input = false; // Checks if the input for the jump is pressed or not
        public bool lockOnInput; // Checks if the input for the lock on is pressed or not
        public bool rightStickRightInput; // Input for changing the lock on target on the right
        public bool rightStickLeftInput;// Input for changing the lock on target on the left
        [SerializeField] bool dPadUp; // Input for the up arrow input for sword/inventory change
        [SerializeField] bool dPadDown; // Input for the down arrow input for skill change
        [SerializeField] bool dPadLeft; // Input for the left arrow input for sword/inventory change for left weapon
        [SerializeField] bool dPadRight; // Input for the right arrow input for sword/inventory change for right weapon
        public bool inventoryInput; // Input for enabling and disabling the inventory

        [Header("Queued Input Flags")]
        public bool input_Has_Been_Queued; // Check if the input has been queued or not
        public float current_Queued_Input_Timer; // Reference to the current queued input timer
        public float default_Queued_Input_Time; // Reference to the default time for queued process
        public bool queued_Tap_RB_Input; // This will check the queue process for tap RB input
        public bool queued_Tap_RT_Input; // This will check the queue process for tap RT input

        [Header("INPUT FLAGS")]
         public bool inventoryFlag; // Checker flag for inventory
         public bool lockOnFlag; // Checker flag for lock on
        
        [Header("DODGE INPUT DATA")]
        public float rollInputTimer; // This will decide if we dash or sprint
        
        private void Awake() 
        {
            player = GetComponent<PlayerManager>();    
        }
        public void OnEnable()
        {
            if (playerControls == null)
            {
                playerControls = new PlayerControls();
                
                // BELOW CODE: Calling movement input
                playerControls.PlayerMovement.Movement.performed +=
                    i => movementInput = i.ReadValue<Vector2>();
                
                // BELOW CODE: Calling camera input
                playerControls.PlayerCamera.Camera.performed +=
                    i => cameraInput = i.ReadValue<Vector2>();

                // BELOW CODE: Calling dodge input 
                playerControls.PlayerActions.Dodge.performed += i => dodgeInput = true;

                // BELOW CODE: Calling jump input
                playerControls.PlayerActions.Jump.performed += i => jump_Input = true;

                // BELOW CODE: Inventory UI Calling 
                playerControls.PlayerUI.Inventory.performed += i => inventoryInput = true;

                // BELOW CODE: Calling use item input
                playerControls.PlayerActions.TapX.performed += i => tapXInput = true;

                // BELOW CODE: Calling examine or open item input
                playerControls.PlayerActions.TapY.performed += i => tapYInput = true;

                // BELOW CODE: Calling tap RB input (light attack)
                playerControls.PlayerActions.TapRB.performed += i => tapRBInput = true;

                // BELOW CODE: Calling tap RT input (heavy attack)
                playerControls.PlayerActions.TapRT.performed += i => tapRTInput = true;

                // BELOW CODE: Calling tap LB input (use skills)
                playerControls.PlayerActions.TapLB.performed += i => tapLBInput = true;

                // BELOW CODE: Calling tap LT input ()
                playerControls.PlayerActions.TapLT.performed += i => tapLTInput = true;

                // BELOW CODE: Calling hold RB input (critical attack) 
                playerControls.PlayerActions.HoldRB.performed += i => holdRBInput = true; 
                playerControls.PlayerActions.HoldRB.canceled += i => holdRBInput = false; 

                // BELOW CODE: Calling hold RT input (charge attack) 
                playerControls.PlayerActions.HoldRT.performed += i => holdRTInput = true;
                playerControls.PlayerActions.HoldRT.canceled += i => holdRTInput = false; 

                // BELOW CODE: Calling hold LB input (blocking) 
                playerControls.PlayerActions.HoldLB.performed += i => holdLBInput = true; 
                playerControls.PlayerActions.HoldLB.canceled += i => holdLBInput = false;

                // BELOW CODE: Calling hold LT input ()
                playerControls.PlayerActions.HoldLT.performed += i => holdLTInput = true; 
                playerControls.PlayerActions.HoldLT.canceled += i => holdLTInput = false;

                // BELOW CODE: Calling sprint input
                playerControls.PlayerActions.Sprint.performed += i => sprintInput = true; 
                playerControls.PlayerActions.Sprint.canceled += i => sprintInput = false;
                
                // BELOW CODE: Calling right weapon slot input
                playerControls.PlayerUI.DPadRight.performed += i => dPadRight = true;

                // BELOW CODE: Calling left weapon slot input 
                playerControls.PlayerUI.DPadLeft.performed += i => dPadLeft = true;

                // BELOW CODE: Calling consumable slot input 
                playerControls.PlayerUI.DPadUp.performed += i => dPadUp = true;

                // BELOW CODE: Calling skill slot input 
                playerControls.PlayerUI.DPadDown.performed += i => dPadDown = true;
            
                // BELOW CODE: Lock on input calling 
                playerControls.PlayerCamera.LockOn.performed += i => lockOnInput = true;

                // BELOW CODE: Lock on right input calling 
                playerControls.PlayerCamera.LockOnTargetRight.performed += i => rightStickRightInput = true;

                // BELOW CODE: Lock on left input calling 
                playerControls.PlayerCamera.LockOnTargetLeft.performed += i => rightStickLeftInput = true;            
            
                // BELOW CODE: 2 handed input calling 
                playerControls.PlayerActions.HoldY.performed += i => holdYInput = true;
                playerControls.PlayerActions.HoldY.canceled += i => holdYInput = false;

                //Qued RB Calling
                playerControls.PlayerActions.QuedTapRB.performed += i => QueInput(ref queued_Tap_RB_Input);
                //Qued Rt Calling
                playerControls.PlayerActions.QuedTapRT.performed += i => QueInput(ref queued_Tap_RT_Input);
            }
            
            playerControls.Enable();
        }
        private void OnApplicationFocus(bool focus) 
        {
            if(enabled)
            {
                if(focus)
                {
                    playerControls.Enable();
                }
                else
                {
                    playerControls.Disable();
                }
            }
        }
        public void UseAllInputs()
        {
            if(player.isDead)
                return;

            UseMovementInput();
            UseDodgeInput();
            UseSprintInput();
            UseJumpInput();

            UseTapRBInput();
            UseTapRTInput();
            UseTapLTInput();
            UseTapLBInput();
            UseHoldRBInput();
            UseHoldRTInput();
            UseHoldLBInput();
            UseHoldLTInput();
            UseQueuedInput();

            UseInventoryInput();
            UseQuickSlotsInput();
            UseConsumableInput();

            UseLockOnInput();
            UseTwoHandInput();
        }
        public void UseMovementInput()
        {
            horizontalInput = movementInput.x;
            verticalInput = movementInput.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));

            if(moveAmount <= 0.5 && moveAmount > 0)
            {
                moveAmount = 0.5f;
            }
            else if(moveAmount > 0.5 && moveAmount <= 1)
            {
                moveAmount = 1;
            }

            if(player == null)
                return;

            player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, player.isSprinting);

            mouseX = cameraInput.x;
            mouseY = cameraInput.y;
        }
        private void UseSprintInput()
        {
            if(player.playerStatsManager.encumbranceLevel == EncumbranceLevel.Overloaded)
            {
                
            }
            else 
            {
                if(sprintInput)
                {
                    // BELOW CODE: Player started sprinting
                    player.playerLocomotionManager.UseSprinting();
                }
                else
                {
                    player.isSprinting = false;
                }
            }
        }
        private void UseDodgeInput()
        {
            if(player.playerStatsManager.encumbranceLevel == EncumbranceLevel.Overloaded)
            {
                
            }
            else 
            {
                if(dodgeInput)
                {
                    dodgeInput = false;

                    // TO-DO: Do nothing if menu or UI window is open
                    // BELOW CODE: Perform a dodge
                    player.playerLocomotionManager.AttemptToPerformDodge();
                }
            }
        }
        private void UseJumpInput()
        {
            if(jump_Input)
            {
                jump_Input = false;
                player.playerLocomotionManager.AttemptToPerformJump();
            }
        }
        private void UseLockOnInput()
        {
            if(lockOnInput && lockOnFlag == false)
            {
                lockOnInput = false;
                lockOnFlag = true;
                player.playerCameraManager.HandleLockOn();
                if(player.playerCameraManager.nearestLockOnTarget != null)
                {
                    player.playerCameraManager.currentLockOnTarget = player.playerCameraManager.nearestLockOnTarget;
                }
            } 
            else if(lockOnInput && lockOnFlag)
            {
                lockOnInput = false;
                lockOnFlag = false;
                player.playerCameraManager.ClearLockOnTargets();
            }

            //BELOW CODE: For left lock on target enable

            if(lockOnFlag && rightStickLeftInput)
            {
               rightStickLeftInput= false;
               player.playerCameraManager.HandleLockOn();
               if(player.playerCameraManager.leftLockOnTarget != null) 
               {
                  player.playerCameraManager.currentLockOnTarget = player.playerCameraManager.leftLockOnTarget;
               }
            }

            // BELOW CODE: For right lock on target enable
            if (lockOnFlag && rightStickRightInput)
            {
               rightStickRightInput = false;
               player.playerCameraManager.HandleLockOn();
               if (player.playerCameraManager.rightLockOnTarget != null)
               {
                  player.playerCameraManager.currentLockOnTarget = player.playerCameraManager.rightLockOnTarget;
               }
            }

            player.playerCameraManager.SetCameraHeight();
        }
        private void UseTwoHandInput()
        {
            if(holdYInput)
            {
                holdYInput = false;
                player.isTwoHandingWeapon = !player.isTwoHandingWeapon;

                if(player.isTwoHandingWeapon)
                {
                     // BELOW CODE: Enable two handing
                    player.isTwoHandingWeapon = true;
                    player.playerWeaponSlotManager.LoadWeaponOnSlot(player.playerInventoryManager.rightHandWeapon, false);
                    if(player.playerInventoryManager.rightHandWeapon.ikEnabled)
                    {
                        player.playerWeaponSlotManager.LoadTwoHandIKTargets(true);
                    }
                    else 
                    {
                        Debug.Log("IK System in the current weapon is disabled!");    
                    }
                }
                else
                {
                    // BELOW CODE: Disable two handing
                    player.isTwoHandingWeapon = false;
                    player.playerWeaponSlotManager.LoadWeaponOnSlot(player.playerInventoryManager.rightHandWeapon, false);
                    player.playerWeaponSlotManager.LoadWeaponOnSlot(player.playerInventoryManager.leftHandWeapon, true);
                    player.playerWeaponSlotManager.LoadTwoHandIKTargets(false);
                }  
            }
        }     
        private void UseTapRBInput()
        {
            player.playerAnimatorManager.EraseHandIKForWeapon();

            // RB input handles the weapon's light attack
            if (tapRBInput)
            {
                tapRBInput = false;

                if(player.playerInventoryManager.rightHandWeapon.oh_Tap_RB_Action != null)
                {
                    player.UpdateWhichHandPlayerIsUsing(true);
                    player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightHandWeapon;
                    player.playerInventoryManager.rightHandWeapon.oh_Tap_RB_Action.PerformAction(player);
                }
            }
        }
        private void UseTapRTInput()
        {
            player.playerAnimatorManager.EraseHandIKForWeapon();

            // RT input handles the weapon's heavy attack
            if (tapRTInput)
            {
                tapRTInput = false;

                if(player.playerInventoryManager.rightHandWeapon.oh_Tap_RT_Action != null)
                {
                    player.UpdateWhichHandPlayerIsUsing(true);
                    player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightHandWeapon;
                    player.playerInventoryManager.rightHandWeapon.oh_Tap_RT_Action.PerformAction(player);                 
                }
            }
        }
        private void UseHoldRBInput()
        {
            if(holdRBInput)
            {
                holdRBInput = false;

                if(player.playerInventoryManager.rightHandWeapon.oh_Hold_RB_Action != null)
                {
                    // BELOW CODE: Use the critical attack (backstab or riposte)
                    player.playerInventoryManager.rightHandWeapon.oh_Hold_RB_Action.PerformAction(player);
                }
            }
        }   
        private void UseHoldRTInput()
        {
            player.animator.SetBool("isChargingAttack", holdRTInput);

            if(holdRTInput)
            {
                holdRTInput = false;
                player.UpdateWhichHandPlayerIsUsing(true);
                player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightHandWeapon;

                if(player.playerInventoryManager.rightHandWeapon.oh_Hold_RT_Action != null)
                {
                    player.playerInventoryManager.rightHandWeapon.oh_Hold_RT_Action.PerformAction(player);
                }
            }
        }   
        private void UseTapLTInput()
        {
            if(tapLTInput)
            {
                tapLTInput = false;

                if(player.isTwoHandingWeapon)
                {
                    if(player.playerInventoryManager.rightHandWeapon.oh_Tap_LT_Action != null)
                    {
                        player.UpdateWhichHandPlayerIsUsing(true);
                        player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightHandWeapon;
                        player.playerInventoryManager.rightHandWeapon.oh_Tap_LT_Action.PerformAction(player);   
                    }
                }
                else
                {
                    if(player.playerInventoryManager.leftHandWeapon.oh_Tap_LT_Action != null)
                    {
                        player.UpdateWhichHandPlayerIsUsing(false);
                        player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.leftHandWeapon;
                        player.playerInventoryManager.leftHandWeapon.oh_Tap_LT_Action.PerformAction(player);   
                    }
                }
            }
        }
        private void UseTapLBInput()
        {
            if(tapLBInput)
            {
                tapLBInput = false;

                if(player.isTwoHandingWeapon)
                {
                    if(player.playerInventoryManager.leftHandWeapon.oh_Tap_LB_Action != null)
                    {
                        player.UpdateWhichHandPlayerIsUsing(false);
                        player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.leftHandWeapon;
                        player.playerInventoryManager.leftHandWeapon.oh_Tap_LB_Action.PerformAction(player);   
                    } 
                } 
                else
                {
                    if(player.playerInventoryManager.rightHandWeapon.oh_Tap_LB_Action != null)
                    {
                        player.UpdateWhichHandPlayerIsUsing(true);
                        player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightHandWeapon;
                        player.playerInventoryManager.rightHandWeapon.oh_Tap_LB_Action.PerformAction(player);
                    }
                }
            }           
        }
        public void UseHoldLBInput()
        {
            if(!player.isGrounded ||
                player.isSprinting ||
                player.isFiringSkill)
            {
                holdLBInput = false;
                return;
            }

            if(holdLBInput)
            {
                if(player.playerInventoryManager.leftHandWeapon.weaponType == WeaponType.SmallShield)
                {
                    if(player.isTwoHandingWeapon)
                    {
                        player.UpdateWhichHandPlayerIsUsing(true);
                        player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightHandWeapon;
                        player.playerInventoryManager.rightHandWeapon.oh_Hold_LB_Action.PerformAction(player);   
                    }
                    else
                    {
                        player.UpdateWhichHandPlayerIsUsing(false);
                        player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.leftHandWeapon;
                        player.playerInventoryManager.leftHandWeapon.oh_Hold_LB_Action.PerformAction(player);   
                    } 
                }
                else 
                {
                    Debug.Log("Shield is not equipped in the left hand!");      
                }  
            }
            else  
            {
                player.isBlocking = false;    
            }
        }
        public void UseHoldLTInput()
        {
            if(holdLTInput)
            {
                holdLTInput = false;

                Debug.LogWarning("This input currently not implemented!");
            }
        }
        public void UseInventoryInput()
        {
            if(inventoryFlag)
            {
                //player.playerUIManager.UpdateUI();
            }

            if(inventoryInput)
            {
                inventoryFlag = !inventoryFlag;

                // if(inventoryFlag)
                // {
                //     player.playerUIManager.OpenSelectWindow();
                //     player.playerUIManager.UpdateUI();
                //     player.playerUIManager.hudWindow.SetActive(false);
                // }
                // else
                // {
                //     player.playerUIManager.CloseSelectWindow();
                //     player.playerUIManager.CloseAllInventoryWindow();
                //     player.playerUIManager.hudWindow.SetActive(true);
                // }
            }
        }
        private void UseQuickSlotsInput()
        {
            // BELOW CODE: For right hand changing weapon
            if(dPadRight)
            {
                dPadRight = false;
                if(player.isTwoHandingWeapon)
                {
                    Debug.Log("Can't chage weapon because player is two handing weapon");
                }
                else 
                {
                    player.playerInventoryManager.ChangeRightWeapon();
                }
            }
            // BELOW CODE: For left hand changing weapon
            if (dPadLeft)
            {
                dPadLeft = false;
                if(player.isTwoHandingWeapon)
                {
                    Debug.Log("Can't chagne weapon because player is two handing weapon");
                }
                else 
                {
                    player.playerInventoryManager.ChangeLeftWeapon();
                }
            }

            // BELOW CODE: For choosing consumable item
            if(dPadUp)
            {
                dPadUp = false;
                player.playerStatesManager.ChangeConsumableItem();
            }

            // BELOW CODE: For choosing skill
            if(dPadDown)
            {
                dPadDown = false;
                player.playerSkillManager.ChangeSkill();
            }
        }
        private void UseConsumableInput()
        {
            if(tapXInput)
            {
                tapXInput = false;

                // BELOW CODE: Use current consumable
                player.playerStatesManager.currentConsumableBeingUsed.AttemptToConsumeItem(player);
            }
        }
        public void CheckForInterctableObject() // As the script name says it check if you interact with an item/weapon and then do something about it :)
        {
            RaycastHit interactHit;
            if(Physics.SphereCast(transform.position, 0.3f, transform.forward, out interactHit, 1f, player.playerCameraManager.ignoreLayers))
            {
                if(interactHit.collider.tag == "Interactable")
                {
                    WRLD_INTERACTABLE interactableObject = interactHit.collider.GetComponent<WRLD_INTERACTABLE>();

                    if(interactableObject != null)
                    {
                        string interactableText = interactableObject.interactableText;

                        // BELOW CODE: It sets the UI's text into interactable text & then set the text pop up to true
                        player.interactableUI.interactableText.text = interactableText;
                        player.interactableUIGameObject.SetActive(true);

                        if (tapYInput)
                        {              
                            interactHit.collider.GetComponent<WRLD_INTERACTABLE>().Interact(player);
                        }
                    }
                }
            }
            else
            {
                if(player.interactableUIGameObject != null)
                {
                    player.interactableUIGameObject.SetActive(false);
                }

                if(player.itemInteractableGameObject != null && tapYInput)
                {
                    player.itemInteractableGameObject.SetActive(false);
                }
            }
        }
        public void QueInput(ref bool queuedInput)
        {
            // Disable all the qued inputs
            // Qued_LB_Input = false;
            // Qued_RT_Input = false;

            // BELOW CODE: Enable the referenced input by reference
            // BELOW CODE: If we are performing an action, we can que an input, otherwise queing is not needed
            if (player.isPerformingAction)
            {
                queuedInput = true;
                current_Queued_Input_Timer = default_Queued_Input_Time;
                input_Has_Been_Queued = true;
            }
        }
        private void UseQueuedInput()
        {
            if (input_Has_Been_Queued)
            {
                if (current_Queued_Input_Timer > 0)
                {
                    current_Queued_Input_Timer = current_Queued_Input_Timer - Time.deltaTime;
                    // BELOW CODE: Try and process queued input
                    ProcessQueuedInput();
                }
                else
                {
                    input_Has_Been_Queued = false;
                    current_Queued_Input_Timer = 0;
                }
            }
        }
        private void ProcessQueuedInput()
        {
            if (queued_Tap_RB_Input)
            {
                tapRBInput = true;
            }
            if (queued_Tap_RT_Input)
            {
                tapRTInput = true;
            }
        }
    }
}