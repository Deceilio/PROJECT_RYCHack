using System.Collections;
using UnityEngine;

namespace Deceilio.Psychain
{
    public class OpenInteractableChest : WRLD_INTERACTABLE
    {
        Animator animator; // Reference to animator component  
        OpenInteractableChest openChest; // Reference to open chest scripti

        public Transform playerStandingPosition; // Position of player
        public GameObject itemSpawner; // Reference to the item spawner game object
        protected override void Awake() 
        {
            animator = GetComponent<Animator>();
            openChest = GetComponent<OpenInteractableChest>();
        }
        public override void Interact(PlayerManager player)
        {
            // BELOW CODE: Rotate the player towards the chest
            Vector3 rotationDirection = transform.position - player.transform.position;
            rotationDirection.y = 0;
            rotationDirection.Normalize();
            
            Quaternion newRotation = Quaternion.LookRotation(rotationDirection);
            Quaternion targetRotation = Quaternion.Slerp(player.transform.rotation, newRotation, 300 * Time.deltaTime);
            player.transform.rotation = targetRotation;

            // BELOW CODE: Lock his transform to a certain point infront of the chest and animate the player
            player.OpenChestInteraction(playerStandingPosition);
            // BELOW CODE: Open the chest lid and play chest animation 
            animator.SetBool("openedChest", true);

            // BELOW CODE: Spawn an item inside the chest the player can pick up
            StartCoroutine(SpawnItemsInChest());
        }
        private IEnumerator SpawnItemsInChest()
        {
            yield return new WaitForSeconds(1f);
            GameObject spawnedItem = Instantiate(itemSpawner, transform);
            spawnedItem.name = "spawnedItem";
            Destroy(openChest);
        }
    }
}