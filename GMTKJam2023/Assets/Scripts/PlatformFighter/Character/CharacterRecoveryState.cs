using System;
using UnityEngine;

namespace PlatformFighter.Character
{
    public class CharacterRecoveryState : CharacterState
    {
        public event Action DoneRecovering;
        
        [SerializeField] private float _startup;
        [SerializeField] private float _duration;
        [SerializeField] private float _horizontalRecoveryAmount;
        [SerializeField] private float _verticalRecoveryAmount;
        [SerializeField] private ParticleSystem _fireParts;

        private float _timer = 0.0f;
        private bool _inStartup = false;

        public override void EnterState()
        {
            _timer = 0.0f;
            _inStartup = true;

            Character.Rigidbody2D.velocity = Vector2.zero;
            Character.Rigidbody2D.gravityScale = 0.0f;
            
            _fireParts.Play();
        }

        public override void ExitState()
        {
            Character.Rigidbody2D.gravityScale = 1.0f;
            
            _fireParts.Stop();
        }

        public override void ProcessState()
        {
            if (_inStartup)
            {
                if (_timer < _startup)
                {
                    _timer += Time.deltaTime;
                    return;
                }

                _inStartup = false;
                _timer = 0.0f;
                return;
            }
            else
            {
                if (_timer < _duration)
                {
                    _timer += Time.deltaTime;

                    if (transform.position.x < 0)
                    {
                        Character.Rigidbody2D.velocity =
                            new Vector2(_horizontalRecoveryAmount, _verticalRecoveryAmount);
                    }
                    else
                    {
                        Character.Rigidbody2D.velocity =
                            new Vector2(-_horizontalRecoveryAmount, _verticalRecoveryAmount);
                    }
                    return;
                }
            }
            
            DoneRecovering?.Invoke();
        }

        public override void ProcessStateFixed()
        {
            if (!_inStartup)
            {
                
            }
            
            
        }
    }
}