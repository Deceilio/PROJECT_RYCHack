using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deceilio.Psychain
{
    public class WRLD_CHARACTER_STATE : ScriptableObject
    {
        public int stateID; //ID for the Particular State
        public virtual void ProcessState(CharacterManager character)
        {

        }
    }
}