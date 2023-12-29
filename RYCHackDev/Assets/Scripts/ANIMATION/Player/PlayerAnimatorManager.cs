namespace Deceilio.Psychain
{
    public class PlayerAnimatorManager : CharacterAnimatorManager
    {
        private PlayerManager player; // Reference to the Player Manager script

        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
        }
        public override void OnAnimatorMove()
        {
            base.OnAnimatorMove();
        }
    }
}