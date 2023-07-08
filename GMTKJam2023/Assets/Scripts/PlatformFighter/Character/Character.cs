using System;
using MHR.StateMachine;
using UnityEngine;

namespace PlatformFighter.Character
{
    public class Character : MonoBehaviour
    {
        public CharacterDefinition Definition => _definition;
        public Rigidbody2D Rigidbody2D => _rigidbody2D;
        public Vector2 MovementDirection;
        public LayerMask GroundLayerMask => _groundLayerMask;
        
        [SerializeField] private CharacterDefinition _definition;
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private LayerMask _groundLayerMask;
        [Header("States")]
        [SerializeField] private CharacterImmobileState _immobileState;
        [SerializeField] private CharacterDefaultState _defaultState;
        [SerializeField] private CharacterJumpState _jumpState;
        
        private StateMachine _stateMachine;

        private void Awake()
        {
            _immobileState.SetCharacter(this);
            _defaultState.SetCharacter(this);
            _jumpState.SetCharacter(this);
            
            _stateMachine = new StateMachine(_immobileState,
                new Connection(_immobileState, _defaultState),
                new Connection(_defaultState, _jumpState),
                new Connection(_jumpState, _defaultState)
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
        }

        public void UnlockMovement()
        {
            _stateMachine.TryChangeState(_defaultState);
        }
    }
}