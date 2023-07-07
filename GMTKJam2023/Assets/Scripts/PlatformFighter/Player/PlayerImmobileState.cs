using UnityEngine;

namespace PlatformFighter.Player
{
    public class PlayerImmobileState : PlayerState
    {
        public override void EnterState()
        {
            PlayerPlatform.Rigidbody2D.velocity = Vector2.zero;
        }

        public override void ExitState()
        {
            
        }

        public override void ProcessState()
        {
            
        }

        public override void ProcessStateFixed()
        {
            
        }
    }
}