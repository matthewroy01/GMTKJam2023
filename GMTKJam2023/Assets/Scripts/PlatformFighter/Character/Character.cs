using System;
using MHR.StateMachine;
using UnityEngine;

namespace PlatformFighter.Character
{
    public class Character : MonoBehaviour
    {
        public static event Action LandedOnPlayerPlatform;
        
        public CharacterDefinition Definition => _definition;
        public Rigidbody2D Rigidbody2D => _rigidbody2D;
        public Vector2 MovementDirection;
        public LayerMask GroundLayerMask => _groundLayerMask;
        
        [SerializeField] private CharacterDefinition _definition;
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private LayerMask _groundLayerMask;
        [SerializeField] private LayerMask _playerPlatformLayerMask;
        [Header("States")]
        [SerializeField] private CharacterImmobileState _immobileState;
        [SerializeField] private CharacterDefaultState _defaultState;
        [SerializeField] private CharacterJumpState _jumpState;
        
        private StateMachine _stateMachine;
        private bool _colliding;

        private void Awake()
        {
            _immobileState.SetCharacter(this);
            _defaultState.SetCharacter(this);
            _jumpState.SetCharacter(this);
            
            _stateMachine = new StateMachine(_immobileState,
                new Connection(_immobileState, _defaultState),
                new Connection(_defaultState, _jumpState),
                new Connection(_jumpState, _defaultState),
                new Connection(_jumpState, _jumpState)
            );
        }

        private void OnEnable()
        {
            _defaultState.DecidedToJump += OnDecidedToJump;
            _jumpState.DoneJumping += OnDoneJumping;
        }

        private void OnDisable()
        {
            _defaultState.DecidedToJump += OnDecidedToJump;
            _jumpState.DoneJumping -= OnDoneJumping;
        }

        private void OnDecidedToJump()
        {
            _stateMachine.TryChangeState(_jumpState);
        }

        private void OnDoneJumping()
        {
            _stateMachine.TryChangeState(_defaultState);
        }

        private void Update()
        {
            _stateMachine.CurrentState.ProcessState();
        }

        private void FixedUpdate()
        {
            _stateMachine.CurrentState.ProcessStateFixed();
            
            LookForPlayerPlatform();
        }

        public void UnlockMovement()
        {
            _stateMachine.TryChangeState(_defaultState);
        }

        private void LookForPlayerPlatform()
        {
            if (_rigidbody2D.velocity.y >= 0.0f)
                return;

            if (Physics2D.OverlapBox(transform.position + (Vector3.down * 0.5f), new Vector2(0.8f, 0.25f),
                    0.0f, _playerPlatformLayerMask))
            {
                if (!_colliding)
                {
                    _stateMachine.TryChangeState(_jumpState);
                    LandedOnPlayerPlatform?.Invoke();
                }
                
                _colliding = true;
                return;
            }

            _colliding = false;
        }
    }
}