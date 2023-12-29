using UnityEngine;

namespace Deceilio.Psychain
{
    public abstract class WRLD_STATE : MonoBehaviour
    {
        public abstract WRLD_STATE Tick(AICharacterManager aiCharacter);
    }
}