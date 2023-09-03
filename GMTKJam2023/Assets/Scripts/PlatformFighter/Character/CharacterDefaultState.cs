using System;
using UnityEngine;

namespace PlatformFighter.Character
{
    public class CharacterDefaultState : CharacterState
    {
        public event Action DecidedToJump;
        public event Action DecidedToAttack;

        [SerializeField] private Transform _debugSphere;

        private const float RANDOM_CHECK_SECONDS = 0.2f;
        private const float DELAY_AFTER_DECISION = 1.05f;
        private float _timer;
        private bool _delayed;
        
        public override void EnterState()
        {
            _timer = 0.0f;
            Character.MovementDirection = new Vector2(UnityEngine.Random.Range(0, 2) == 0 ? 1.0f : -1.0f, 0.0f);
        }

        public override void ExitState()
        {
            
        }

        public override void ProcessState()
        {
            if (_delayed)
            {
                if (_timer < DELAY_AFTER_DECISION)
                {
                    _timer += Time.deltaTime;
                    return;
                }

                _delayed = false;
                _timer = 0.0f;
            }
            else
            {
                if (_timer < RANDOM_CHECK_SECONDS)
                {
                    _timer += Time.deltaTime;
                    return;
                }
            }
            
            MakeDecision();
            
            _timer = 0.0f;
        }

        public override void ProcessStateFixed()
        {
            if (_debugSphere != null)
            {
                _debugSphere.position = new Vector3(transform.position.x + Character.MovementDirection.x, transform.position.y - 0.5f, 0.0f);
            }
            
            CheckGround();
            
            Character.Rigidbody2D.AddForce(Character.MovementDirection * Character.Definition.MovementSpeed);
        }

        private void CheckGround()
        {
            if (Physics2D.OverlapCircle(new Vector2(transform.position.x + Character.MovementDirection.x, transform.position.y - 0.5f), 0.1f, Character.GroundLayerMask))
                return;
            
            ChangeDirection();
            _delayed = true;
            _timer = 0.0f;
        }

        private void MakeDecision()
        {
            int random = UnityEngine.Random.Range(0, 11);

            if (random < 3)
            {
                DecidedToAttack?.Invoke();
            }
            else if (random < 6)
            {
                DecidedToJump?.Invoke();
            }
            else if (random < 9)
            {
                ChangeDirection();
            }
            else
            {
                // do nothing
            }
            
            _delayed = true;
        }

        private void ChangeDirection()
        {
            Character.MovementDirection *= -1.0f;
        }
    }
}