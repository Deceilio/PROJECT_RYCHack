using UnityEngine;

namespace Deceilio.Psychain
{
    [System.Serializable]
    public class CharacterSaveData
    {
        [Header("SCENE INDEX")]
        public int sceneIndex = 1; // The index number of the cene

        [Header("CHARACTER INFO")]
        public string characterName = "Nix"; // Name of the character
        public int characterLevel; // Level of the character

        [Header("TIME PLAYED")]
        public float secondsPlayed; // Number of gameplay time displayed in seconds

        [Header("CHARACTER WEAPONS")]
        public int currentRightHandWeaponID; // The ID of the current right hand weapon which character is using
        public int currentLeftHandWeaponID; // The ID of the current left hand weapon which character is using

        [Header("CHARACTER ARMOURS")]
        public int currentHeadArmourItemId; // Item ID for current head armour
        public int currentChestArmourItemId; // Item ID for current chest armour
        public int currentLegArmourItemId; // Item ID for current leg armour
        public int currentHandArmourItemId; // Item ID for current hand armour

        [Header("WORLD COORDINATES")]
        public float xPosition;  // X transform position of the character
        public float yPosition;  // Y transform position of the character
        public float zPosition;  // Z transform position of the character

        [Header("ITEM PICKED UP FROM WORLD")]
        public WRLD_SERIALIZABLE_DICTIONARY<int, bool> itemsInWorld; // The int is the world item id, and the bool is the status of the item, if it has been picked up already.
    
        public CharacterSaveData()
        {
            itemsInWorld = new WRLD_SERIALIZABLE_DICTIONARY<int, bool>();
        }
    }
}