using MHR.StateMachine;
using PlatformFighter.Input;
using UnityEngine;

namespace PlatformFighter.Player
{
    public class PlayerPlatform : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [Header("States")]
        [SerializeField] private PlayerImmobileState _immobileState;
        [SerializeField] private PlayerDefaultState _defaultState;

        private Vector2 _movementDirection;
        private StateMachine _stateMachine;

        private void Awake()
        {
            _stateMachine = new StateMachine(_immobileState, 
                new Connection(_immobileState, _defaultState),
                new Connection(_defaultState, _immobileState)
                );
        }
        
        private void Update()
        {
            DoInput();
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