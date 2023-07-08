﻿using System;
using DG.Tweening;
using UnityEngine;

namespace PlatformFighter.Character
{
    public class CharacterJumpState : CharacterState
    {
        public event Action DoneJumping;
        
        [SerializeField] private float _jumpDuration;
        [SerializeField] private ParticleSystem _jumpParts;

        private float _initialTimer;
        private float _timer;
        private bool _grounded;
        private const float INITIAL_DELAY = 0.1f;
        private int _numJumps = 0;
        
        public override void EnterState()
        {
            _initialTimer = 0.0f;
            _timer = 0.0f;
            _numJumps = 0;

            JumpForce();
        }

        public override void ExitState()
        {
            
        }

        public override void ProcessState()
        {
            if (_initialTimer < INITIAL_DELAY)
            {
                _initialTimer += Time.deltaTime;
                return;
            }

            // TODO: also jump more than once if we can
            if (!_grounded)
            {
                if (_numJumps < Character.Definition.NumJumps)
                {
                    if (_timer < _jumpDuration)
                    {
                        _timer += Time.deltaTime;
                        return;
                    }
                    
                    JumpForce();
                    _timer = 0.0f;
                }

                return;
            }
            
            DoneJumping?.Invoke();
        }

        public override void ProcessStateFixed()
        {
            CheckGround();
            
            Character.Rigidbody2D.AddForce(Character.MovementDirection * Character.Definition.MovementSpeed * Character.Definition.AirSpeedMultiplier);
        }

        private void CheckGround()
        {
            _grounded = Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - 0.5f), 0.1f,
                Character.GroundLayerMask);
        }

        private void JumpForce()
        {
            Character.Rigidbody2D.velocity = new Vector2(Character.Rigidbody2D.velocity.x, 0.0f);
            Character.Rigidbody2D.AddForce(Vector2.up * Character.Definition.JumpForce);
            
            _jumpParts.Play();

            _numJumps++;
        }
    }
}