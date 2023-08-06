using System;
using System.Collections.Generic;
using MHR.StateMachine;
using PlatformFighter.Input;
using PlatformFighter.UI;
using UnityEngine;
using UnityEngine.UI;

namespace PlatformFighter.Player
{
    public class PlayerPlatform : MonoBehaviour
    {
        public static event Action<float> HealthUpdated;
        public Rigidbody2D Rigidbody2D => _rigidbody2D;
        public Vector2 MovementDirection => _movementDirection;
        public Transform ArtContainer => _artContainer;
        [HideInInspector] public float CurrentEnergy;
        
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
        [Header("Energy")]
        [SerializeField] private Slider _slider;
        [SerializeField] private float _energyDrainPerFrame;
        [SerializeField] private float _energyRegenerationPerFrame;

        private Vector2 _movementDirection;
        private StateMachine _stateMachine;
        private int _currentHealth;
        private const float ENERGY_MAX = 100.0f;
        private Vector2 _defaultPosition;

        private void Awake()
        {
            _currentHealth = _maxHealth;
            CurrentEnergy = ENERGY_MAX;
            
            _immobileState.SetPlayerPlatform(this);
            _defaultState.SetPlayerPlatform(this);
            _flipState.SetPlayerPlatform(this);
            _defendState.SetPlayerPlatform(this);
            
            _stateMachine = new StateMachine(_immobileState,
                new Connection(_immobileState),
                new Connection(_defaultState),
                new Connection(_defaultState, _immobileState),
                new Connection(_defaultState, _flipState),
                new Connection(_defaultState, _defendState)
                );

            _defaultPosition = transform.position;
        }
        
        private void OnEnable()
        {
            InputManager.Instance.Ability1Pressed += OnAbility1Pressed;
            _flipState.DoneFlipping += OnDoneFlipping;
            _defendState.DoneDefending += OnDoneDefending;

            Character.Character.LandedOnPlayerPlatform += Character_OnLandedOnPlayerPlatform;
        }

        private void OnDisable()
        {
            InputManager.Instance.Ability1Pressed -= OnAbility1Pressed;
            _flipState.DoneFlipping -= OnDoneFlipping;
            _defendState.DoneDefending -= OnDoneDefending;
            
            Character.Character.LandedOnPlayerPlatform -= Character_OnLandedOnPlayerPlatform;
        }

        private void OnAbility2Pressed()
        {
            _stateMachine.TryChangeState(_flipState);
        }

        private void OnAbility1Pressed()
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

            if (_stateMachine.CurrentState != _defendState)
            {
                RegenerateEnergy();
            }
        }

        private void DoInput()
        {
            _movementDirection = new Vector2(InputManager.Instance.InputDirectionHorizontal, 0.0f);
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
            if (_stateMachine.CurrentState == _defendState)
                return;
            
            if (_currentHealth > 0)
                _hearts[_currentHealth - 1].Lose();
            
            _currentHealth--;
            
            HealthUpdated?.Invoke(_currentHealth);
        }

        public void DrainEnergy()
        {
            if (CurrentEnergy - _energyDrainPerFrame <= 0.0f)
            {
                CurrentEnergy = 0.0f;
                _stateMachine.TryChangeState(_defaultState);
            }
            
            CurrentEnergy -= _energyDrainPerFrame;
            
            UpdateEnergySlider();
        }

        private void RegenerateEnergy()
        {
            if (CurrentEnergy + _energyRegenerationPerFrame >= ENERGY_MAX)
            {
                CurrentEnergy = ENERGY_MAX;
            }

            CurrentEnergy += _energyRegenerationPerFrame;
            
            UpdateEnergySlider();
        }

        private void UpdateEnergySlider()
        {
            _slider.value = CurrentEnergy / ENERGY_MAX;
        }
        
        public void ResetPosition()
        {
            transform.position = _defaultPosition;
            _currentHealth = _maxHealth;
            
            _hearts.ForEach(heart => heart.Restore());
        }

        public void ChangeToImmobileState()
        {
            _stateMachine.TryChangeState(_immobileState);
        }
    }
}