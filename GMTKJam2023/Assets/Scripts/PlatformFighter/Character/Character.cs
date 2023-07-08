using System;
using MHR.StateMachine;
using PlatformFighter.Player;
using PlatformFighter.UI;
using UnityEngine;

namespace PlatformFighter.Character
{
    public class Character : MonoBehaviour
    {
        public static event Action LandedOnPlayerPlatform;
        public static event Action<bool> Died;
        
        public CharacterDefinition Definition => _definition;
        public Rigidbody2D Rigidbody2D => _rigidbody2D;
        [HideInInspector] public Vector2 MovementDirection;
        public LayerMask GroundLayerMask => _groundLayerMask;
        
        [SerializeField] private CharacterDefinition _definition;
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private CharacterDisplay _characterDisplay;
        [Header("Layer Masks")]
        [SerializeField] private LayerMask _groundLayerMask;
        [SerializeField] private LayerMask _playerPlatformLayerMask;
        [SerializeField] private LayerMask _damageLayerMask;
        [Header("Knockback and Damage")]
        [SerializeField] private float _baseKnockbackForce;
        [Header("States")]
        [SerializeField] private CharacterImmobileState _immobileState;
        [SerializeField] private CharacterDefaultState _defaultState;
        [SerializeField] private CharacterJumpState _jumpState;
        [SerializeField] private CharacterStunState _stunState;
        
        private StateMachine _stateMachine;
        private bool _colliding;
        private bool _collidingWithDamage;
        private float _damagePercentage = 0.0f;
        private bool _lastDamageWasFromPlayer = false;

        private void Awake()
        {
            _characterDisplay.Initialize(_definition);
            
            _immobileState.SetCharacter(this);
            _defaultState.SetCharacter(this);
            _jumpState.SetCharacter(this);
            _stunState.SetCharacter(this);
            
            _stateMachine = new StateMachine(_immobileState,
                new Connection(_immobileState, _defaultState),
                new Connection(_defaultState, _jumpState),
                new Connection(_jumpState, _defaultState),
                new Connection(_jumpState, _jumpState),
                new Connection(_defaultState, _stunState),
                new Connection(_jumpState, _stunState),
                new Connection(_stunState, _defaultState)
            );
        }

        private void OnEnable()
        {
            _defaultState.DecidedToJump += OnDecidedToJump;
            _jumpState.DoneJumping += OnDoneJumping;
            _stunState.DoneBeingStunned += OnDoneBeingStunned;
        }

        private void OnDisable()
        {
            _defaultState.DecidedToJump += OnDecidedToJump;
            _jumpState.DoneJumping -= OnDoneJumping;
            _stunState.DoneBeingStunned -= OnDoneBeingStunned;
        }

        private void OnDecidedToJump()
        {
            _stateMachine.TryChangeState(_jumpState);
        }

        private void OnDoneJumping()
        {
            _stateMachine.TryChangeState(_defaultState);
        }

        private void OnDoneBeingStunned()
        {
            _stateMachine.TryChangeState(_defaultState);
        }

        private void Update()
        {
            _stateMachine.CurrentState.ProcessState();

            LookForDamage();
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

        private void LookForDamage()
        {
            Collider2D tmp = Physics2D.OverlapBox(transform.position, Vector2.one, 0.0f, _damageLayerMask);
                
            if (tmp != null)
            {
                if (!_collidingWithDamage)
                {
                    _lastDamageWasFromPlayer = tmp.GetComponentInParent<PlayerPlatform>() != null;

                    _stateMachine.TryChangeState(_stunState);
                    
                    Vector2 playerPosition = FindObjectOfType<PlayerPlatform>().transform.position;
                    TakeDamage(25.0f, new Vector2(playerPosition.x, transform.position.y));
                }

                _collidingWithDamage = true;
                return;
            }

            _collidingWithDamage = false;
        }

        private void TakeDamage(float damage, Vector2 source)
        {
            _damagePercentage += damage;

            Vector2 direction = ((Vector2)transform.position - source).normalized;
            float knockback = (_baseKnockbackForce + _damagePercentage) * _definition.WeightModifier;
            Rigidbody2D.AddForce(direction * knockback);
            Rigidbody2D.AddForce(Vector2.up * (knockback * 0.75f));
            
            _characterDisplay.UpdateDamageSlider(_damagePercentage);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("DeathPlane"))
            {
                _rigidbody2D.velocity = Vector2.zero;
                _damagePercentage = 0.0f;
                transform.position = Vector2.zero + (Vector2.up * 5);
                
                _characterDisplay.UpdateDamageSlider(_damagePercentage);
                
                Died?.Invoke(_lastDamageWasFromPlayer);

                _lastDamageWasFromPlayer = false;
            }
        }
    }
}