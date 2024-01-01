using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deceilio.Psychain
{
    [System.Serializable]
    public class WRLD_DIALOGUE
    {
        public string name;
        [TextArea(3, 10)]
        public string[] sentences;
        public AudioClip[] voiceLines;

        public string[] functionNames; // Add this line for function names

        // Define a delegate to represent the function signature
        [NonSerialized]
        public Action[] functions;
    }
}