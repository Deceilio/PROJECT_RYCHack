using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deceilio.Psychain
{
    public class AICharacterActions : ScriptableObject
    {
        [Header("A.I CHARACTER ACTION ANIMATION")]
        [Tooltip("Animation for the A.I Character's action")]
        public string actionAnimation; // Animation for a particular Action, can be attack or something else...
    }
} 