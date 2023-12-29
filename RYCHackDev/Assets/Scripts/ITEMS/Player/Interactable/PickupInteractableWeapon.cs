using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Deceilio.Psychain
{
    public class PickupInteractableWeapon : WRLD_INTERACTABLE
    {
        [Header("ITEM")]
        public WRLD_WEAPON_ITEM weapon; // The particular weapon to pickup
        
        protected override void Awake()
        {
            base.Awake();
        }
        protected override void Start()
        {
            base.Start();
        }
        public override void Interact(PlayerManager player)
        {
            base.Interact(player);
            PickUpItem(player); // Pickup the item and add it to the player's inventory
        }
        private void PickUpItem(PlayerManager player)
        {
            // PlayerInventoryManager playerInventory;
            // PlayerLocomotionManager playerLocomotion;
            // PlayerAnimatorManager playerAnimator;
            // playerInventory = player.GetComponent<PlayerInventoryManager>();
            // playerLocomotion = player.GetComponent<PlayerLocomotionManager>();
            // playerAnimator = player.GetComponentInChildren<PlayerAnimatorManager>();

            //player.playerLocomotionManager.rigidBody.velocity = Vector3.zero; // Stops the player from moving while picking up the weapon
            player.playerAnimatorManager.PlayTargetActionAnimation("Pick Up Item", true); // Plays the animation of looting the weapon        
            player.playerInventoryManager.weaponInventory.Add(weapon); // It will add weapon to the weapon inventory
            player.itemInteractableGameObject.GetComponentInChildren<TextMeshProUGUI>().text = weapon.itemName; // This will show the name of the weapon you picked up
            player.itemInteractableGameObject.GetComponentInChildren<RawImage>().texture = weapon.itemIcon.texture; // This will show the weapon icon which you picked up
            player.itemInteractableGameObject.SetActive(true); // It will gives you the name of the weapon you picked
            Destroy(gameObject); //Destroying the weapon after pickup
        }
    }
}