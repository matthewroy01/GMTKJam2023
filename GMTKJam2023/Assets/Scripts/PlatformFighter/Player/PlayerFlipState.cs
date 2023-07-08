using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformFighter.Player
{
    public class PlayerFlipState : PlayerState
    {
        public event Action DoneFlipping;

        [SerializeField] private float _dashSpeed;
        [SerializeField] private float _flipDuration;
        [SerializeField] private Animator _animator;
        [SerializeField] private List<TrailRenderer> _trailRenderers = new();

        private Vector2 _dashDirection;
        private float _timer;
        
        public override void EnterState()
        {
            _dashDirection = PlayerPlatform.MovementDirection;
            _timer = 0.0f;
            
            _animator.SetTrigger(PlayerPlatform.MovementDirection.x >= 0.0f ? "Flip" : "FlipBack");

            ToggleTrailRenderers(true);
        }

        public override void ExitState()
        {
            
        }

        public override void ProcessState()
        {
            if (_timer < _flipDuration)
            {
                _timer += Time.deltaTime;
                return;
            }
            
            ToggleTrailRenderers(false);
            
            DoneFlipping?.Invoke();
        }

        public override void ProcessStateFixed()
        {
            PlayerPlatform.Rigidbody2D.velocity = _dashDirection * _dashSpeed;
        }

        private void ToggleTrailRenderers(bool toggle)
        {
            _trailRenderers.ForEach(trailRenderer => trailRenderer.emitting = toggle);
        }
    }
}