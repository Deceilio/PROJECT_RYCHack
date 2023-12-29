using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Deceilio.Psychain
{
    [CreateAssetMenu(menuName = "Psychain/AI/A.I Actions/Attack Actions")]
    public class AICharacterAttackActions : AICharacterActions
    {
        // BELOW CODE: Every attack will have an attack score,
        // BELOW CODE: The higher the score is, the more chance to play that attack
        // BELOW CODE: For example: If A.I Characte have 5 attacks, but bite attack is the highest attack score
        // BELOW CODE: Then it is more likely, that attack will play more and play repeatedly, whatever the case.
        [Header("A.I CHARACTER DATA")]
        [Tooltip("Every attack will have an attack score, The higher the score, The higher the score is, the more chance to play that attack.For example: If A.I Character have 5 attacks, but bite attack is the highest attack score, then it is more likely, that attack will play more and play repeatedly, whatever the case.")]
        public int attackScore = 3; // The attack score
        [Tooltip("how long A.I Character should wait after attacking to recover.")]        
        public int recoveryTime; // A.I Character Recover Time after attacking
        [Tooltip("Is A.I Character allowed to do combo?")]
        public bool canCombo; // Checks if the A.I Character can do the combo or not
        [Tooltip("COMBO ATTACK")]  
        public AICharacterAttackActions comboAction; // Use to use this script for Combo

        [Header("A.I CHARACTER ATTACK SETTINGS")]
        [Tooltip("Maximum angle for the particular attack.")]     
        public float maximumAttackAngle = 35; // Maximum angle for the particular attack
        [Tooltip("Minimum angle for the particular attack.")]     
        public float minimumAttackAngle = -35; // Minimum angle for the particular attack

        [Header("A.I CHARACTER MOVEMENT SETTINGS")]
        [Tooltip("Minimum distance required by the A.I Character to attack.")]
        public float minimumDistanceNeededToAttack = 0; // Minimum distance req by A.I Character to attack 
        [Tooltip("Maximum distance required by the A.I Character to attack.")]
        public float maximumDistanceNeededToAttack = 3; // Maximum distance req by A.I Character to attack 
    }
}
