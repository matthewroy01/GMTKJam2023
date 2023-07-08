using MHR.StateMachine;
using PlatformFighter.Input;
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
        [SerializeField] private PlayerDashState _dashState;
        [SerializeField] private PlayerFlipState _flipState;

        private Vector2 _movementDirection;
        private StateMachine _stateMachine;

        private void Awake()
        {
            _immobileState.SetPlayerPlatform(this);
            _defaultState.SetPlayerPlatform(this);
            _dashState.SetPlayerPlatform(this);
            _flipState.SetPlayerPlatform(this);
            
            _stateMachine = new StateMachine(_immobileState,
                new Connection(_defaultState),
                new Connection(_defaultState, _immobileState),
                new Connection(_defaultState, _dashState),
                new Connection(_defaultState, _flipState)
                );
        }
        
        private void OnEnable()
        {
            InputManager.Instance.DashPressed += OnDashPressed;
            InputManager.Instance.Ability1Pressed += OnAbility1Pressed;
            _dashState.DoneDashing += OnDoneDashing;
            _flipState.DoneFlipping += OnDoneFlipping;
        }

        private void OnDisable()
        {
            InputManager.Instance.DashPressed -= OnDashPressed;
            InputManager.Instance.Ability1Pressed -= OnAbility1Pressed;
            _dashState.DoneDashing -= OnDoneDashing;
            _flipState.DoneFlipping -= OnDoneFlipping;
        }

        private void OnDashPressed()
        {
            _stateMachine.TryChangeState(_dashState);
        }

        private void OnAbility1Pressed()
        {
            _stateMachine.TryChangeState(_flipState);
        }

        private void OnDoneDashing()
        {
            ReturnToDefaultState();
        }

        private void OnDoneFlipping()
        {
            ReturnToDefaultState();
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
    }
}