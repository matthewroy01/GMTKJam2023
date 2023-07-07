using System;
using MHR.StateMachine;
using PlatformFighter.Input;
using UnityEngine;

namespace PlatformFighter.Player
{
    [RequireComponent(typeof(PlayerImmobileState), typeof(PlayerDefaultState), typeof(Rigidbody2D))]
    public class PlayerPlatform : MonoBehaviour
    {
        public Rigidbody2D Rigidbody2D => _rigidbody2D;
        public Vector2 MovementDirection => _movementDirection;
        
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [Header("States")]
        [SerializeField] private PlayerImmobileState _immobileState;
        [SerializeField] private PlayerDefaultState _defaultState;

        private Vector2 _movementDirection;
        private StateMachine _stateMachine;

        private void Awake()
        {
            _immobileState.SetPlayerPlatform(this);
            _defaultState.SetPlayerPlatform(this);
            
            _stateMachine = new StateMachine(_immobileState, 
                new Connection(_immobileState, _defaultState),
                new Connection(_defaultState, _immobileState)
                );
        }
        
        private void Update()
        {
            DoInput();
            
            _stateMachine.CurrentState.ProcessState();
        }

        private void FixedUpdate()
        {
            _stateMachine.CurrentState.ProcessStateFixed();
        }

        private void DoInput()
        {
            _movementDirection = InputManager.Instance.InputDirection;
        }

        public void UnlockMovement()
        {
            _stateMachine.TryChangeState(_defaultState);
        }
    }
}