using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deceilio.Psychain
{
    public class WRLD_CHARACTER_STATIC_STATE : WRLD_CHARACTER_STATE
    {
        // BELOW CODE: Static states will going to be used to apply an effect to a player when equipping an item
        public virtual void AddStaticState(CharacterManager character)
        {

        }

        // BELOW CODE: Remove the states after item has been removed
        public virtual void RemoveStaticState(CharacterManager character)
        {

        }
    }
}
