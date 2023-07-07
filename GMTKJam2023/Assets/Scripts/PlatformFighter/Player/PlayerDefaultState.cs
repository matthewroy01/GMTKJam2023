using UnityEngine;

namespace PlatformFighter.Player
{
    public class PlayerDefaultState : PlayerState
    {
        [SerializeField] private float _movementSpeed;
        
        public override void EnterState()
        {
            
        }

        public override void ExitState()
        {
            
        }

        public override void ProcessState()
        {
            
        }

        public override void ProcessStateFixed()
        {
            Movement();
        }

        private void Movement()
        {
            PlayerPlatform.Rigidbody2D.velocity = PlayerPlatform.MovementDirection * _movementSpeed;
        }
    }
}