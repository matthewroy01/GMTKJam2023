using System;
using PlatformFighter.Input;
using UnityEngine;

namespace PlatformFighter.Player
{
    public class PlayerDashState : PlayerState
    {
        public event Action DoneDashing;
        
        [SerializeField] private float _dashSpeed;
        [SerializeField] private float _dashDuration;
        
        private Vector2 _dashDirection;
        private float _timer;
        
        public override void EnterState()
        {
            _dashDirection = PlayerPlatform.MovementDirection;
            _timer = 0.0f;
        }

        public override void ExitState()
        {
            
        }

        public override void ProcessState()
        {
            if (_timer < _dashDuration)
            {
                _timer += Time.deltaTime;
                return;
            }
            
            DoneDashing?.Invoke();
        }

        public override void ProcessStateFixed()
        {
            PlayerPlatform.Rigidbody2D.velocity = _dashDirection * _dashSpeed;
        }
    }
}