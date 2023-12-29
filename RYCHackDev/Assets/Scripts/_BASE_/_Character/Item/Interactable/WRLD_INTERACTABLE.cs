using UnityEngine;

namespace Deceilio.Psychain
{
    public class WRLD_INTERACTABLE : MonoBehaviour
    {
        // This is unique id for this item spawn in the game world, each item placed in the world should have it's own unique id
        [Header("WORLD ITEM ID")]
        public int itemPickUpID;
        public bool hasBeenPickedUp; // Bool to check if the item is already been picked up or not

        [Header("INTERACTS")]
        public float radius = 0.6f; // Radius of the wire sphere
        public string interactableText; // Pickup item text when interact
        protected virtual void Awake()
        {
            
        }
        protected virtual void Start()
        {
            // BELOW CODE: If the save data does not contain this item, we must never picked it up, so we add to the list and list is not pickedup
            // if (!WorldSaveGameManager.instance.currentCharacterSaveData.itemsInWorld.ContainsKey(itemPickUpID))
            // {
            //     WorldSaveGameManager.instance.currentCharacterSaveData.itemsInWorld.Add(itemPickUpID, false);
            // }

            //hasBeenPickedUp = WorldSaveGameManager.instance.currentCharacterSaveData.itemsInWorld[itemPickUpID];

            // if (hasBeenPickedUp)
            // {
            //     gameObject.SetActive(false);
            // }
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
        public virtual void Interact(PlayerManager player)
        {
            // BELOW CODE: Notify the character data the item has been pickedup from the world, so it does not spawn again
            // if (WorldSaveGameManager.instance.currentCharacterSaveData.itemsInWorld.ContainsKey(itemPickUpID))
            // {
            //     WorldSaveGameManager.instance.currentCharacterSaveData.itemsInWorld.Remove(itemPickUpID);
            // }

            // BELOW CODE: Saves the pickup to our save data so it does not spawn again when we reload the area
            //WorldSaveGameManager.instance.currentCharacterSaveData.itemsInWorld.Add(itemPickUpID, true);

            hasBeenPickedUp = true;
        }
    }
}