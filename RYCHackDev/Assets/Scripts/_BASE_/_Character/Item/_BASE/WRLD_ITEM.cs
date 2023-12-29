using UnityEngine;

namespace Deceilio.Psychain
{
    public class WRLD_ITEM : ScriptableObject
    {
        [Header("ITEM INFORMATION")]
        [Tooltip("The ID for the particular item")]
        public int itemID; // Access to particular item id for items
        [Tooltip("The sprite icon for the particular item")]
        public Sprite itemIcon; // Access to particular item icon
        [Tooltip("The name of the particular item")]
        public string itemName; // Access to particular item name    
        [Tooltip("The lore of the particular item")]                    
        [TextArea] public string itemLore; // Item lore and item description 
    }
}
