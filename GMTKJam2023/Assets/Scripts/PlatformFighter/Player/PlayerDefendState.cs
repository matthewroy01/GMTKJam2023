using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformFighter.Player
{
    public class PlayerDefendState : PlayerState
    {
        public event Action DoneDefending;

        [SerializeField] private float _defendDuration;

        private float _timer;
        
        public override void EnterState()
        {
            PlayerPlatform.Rigidbody2D.velocity = Vector2.zero;
            Physics.IgnoreLayerCollision(6, 7, false);
            
            _timer = 0.0f;
        }

        public override void ExitState()
        {
            Physics.IgnoreLayerCollision(6, 7, true);
        }

        public override void ProcessState()
        {
            if (_timer < _defendDuration)
            {
                _timer += Time.deltaTime;
                return;
            }
            
            DoneDefending?.Invoke();
        }

        public override void ProcessStateFixed()
        {
            
        }
    }
}