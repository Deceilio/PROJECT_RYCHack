namespace Deceilio.Psychain
{
    public class AICharacterCombatManager : CharacterCombatManager
    {
        AICharacterManager aiCharacter; // Reference to the A.I Character Manager script
        protected override void Awake()
        {
            base.Awake();
            aiCharacter = GetComponent<AICharacterManager>();
        }
    }
}