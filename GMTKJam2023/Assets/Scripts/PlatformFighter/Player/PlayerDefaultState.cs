using System;
using PlatformFighter.Input;
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
            PlayerPlatform.Rigidbody2D.velocity =
                new Vector2(PlayerPlatform.MovementDirection.x * _movementSpeed, 0.0f);
            //PlayerPlatform.Rigidbody2D.AddForce(PlayerPlatform.MovementDirection * _movementSpeed);
        }
    }
}