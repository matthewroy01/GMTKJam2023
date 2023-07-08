using System;
using System.Collections.Generic;
using MHR.StateMachine;
using PlatformFighter.Input;
using PlatformFighter.UI;
using UnityEngine;

namespace PlatformFighter.Player
{
    public class PlayerPlatform : MonoBehaviour
    {
        public Rigidbody2D Rigidbody2D => _rigidbody2D;
        public Vector2 MovementDirection => _movementDirection;
        public Transform ArtContainer => _artContainer;
        
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private Transform _artContainer;
        [Header("States")]
        [SerializeField] private PlayerImmobileState _immobileState;
        [SerializeField] private PlayerDefaultState _defaultState;
        [SerializeField] private PlayerFlipState _flipState;
        [SerializeField] private PlayerDefendState _defendState;
        [Header("Health")]
        [SerializeField] private int _maxHealth;
        [SerializeField] private List<PlayerHeart> _hearts = new();

        private Vector2 _movementDirection;
        private StateMachine _stateMachine;
        private int _currentHealth;

        private void Awake()
        {
            _currentHealth = _maxHealth;
            
            _immobileState.SetPlayerPlatform(this);
            _defaultState.SetPlayerPlatform(this);
            _flipState.SetPlayerPlatform(this);
            _defendState.SetPlayerPlatform(this);
            
            _stateMachine = new StateMachine(_immobileState,
                new Connection(_defaultState),
                new Connection(_defaultState, _immobileState),
                new Connection(_defaultState, _flipState),
                new Connection(_defaultState, _defendState)
                );
        }
        
        private void OnEnable()
        {
            InputManager.Instance.Ability1Pressed += OnAbility1Pressed;
            InputManager.Instance.Ability2Pressed += OnAbility2Pressed;
            _flipState.DoneFlipping += OnDoneFlipping;
            _defendState.DoneDefending += OnDoneDefending;

            Character.Character.LandedOnPlayerPlatform += Character_OnLandedOnPlayerPlatform;
        }

        private void OnDisable()
        {
            InputManager.Instance.Ability1Pressed -= OnAbility1Pressed;
            InputManager.Instance.Ability2Pressed -= OnAbility2Pressed;
            _flipState.DoneFlipping -= OnDoneFlipping;
            _defendState.DoneDefending -= OnDoneDefending;
            
            Character.Character.LandedOnPlayerPlatform -= Character_OnLandedOnPlayerPlatform;
        }

        private void OnAbility1Pressed()
        {
            _stateMachine.TryChangeState(_flipState);
        }

        private void OnAbility2Pressed()
        {
            _stateMachine.TryChangeState(_defendState);
        }

        private void OnDoneFlipping()
        {
            ReturnToDefaultState();
        }

        private void OnDoneDefending()
        {
            ReturnToDefaultState();
        }

        private void Character_OnLandedOnPlayerPlatform()
        {
            TakeDamage();
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
            ReturnToDefaultState();
        }

        private void ReturnToDefaultState()
        {
            _stateMachine.TryChangeState(_defaultState);
        }

        private void TakeDamage()
        {
            _hearts[_currentHealth - 1].Lose();
            
            _currentHealth--;
        }
    }
}