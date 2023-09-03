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
        public static event Action<bool, Vector2> Died;
        
        public CharacterDefinition Definition => _definition;
        public Rigidbody2D Rigidbody2D => _rigidbody2D;
        [HideInInspector] public Vector2 MovementDirection;
        public LayerMask GroundLayerMask => _groundLayerMask;
        
        [SerializeField] private CharacterDefinition _definition;
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private CharacterDisplay _characterDisplay;
        [SerializeField] private float _recoveryThreshold;
        [SerializeField] private ParticleSystem _hitParts;
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
        [SerializeField] private CharacterRecoveryState _recoveryState;
        [SerializeField] private CharacterAttackState _attackState;
        
        private StateMachine _stateMachine;
        private bool _colliding;
        private bool _collidingWithDamage;
        private float _damagePercentage = 0.0f;
        private bool _lastDamageWasFromPlayer = false;
        private Vector2 _defaultPosition;

        private void Awake()
        {
            _characterDisplay.Initialize(_definition);
            
            _immobileState.SetCharacter(this);
            _defaultState.SetCharacter(this);
            _jumpState.SetCharacter(this);
            _stunState.SetCharacter(this);
            _recoveryState.SetCharacter(this);
            _attackState.SetCharacter(this);
            
            _stateMachine = new StateMachine(_immobileState,
                new Connection(_immobileState),
                new Connection(_immobileState, _defaultState),
                new Connection(_defaultState, _jumpState),
                new Connection(_jumpState, _defaultState),
                new Connection(_jumpState, _jumpState),
                new Connection(_defaultState, _stunState),
                new Connection(_jumpState, _stunState),
                new Connection(_recoveryState, _stunState),
                new Connection(_stunState, _defaultState),
                new Connection(_defaultState, _recoveryState),
                new Connection(_jumpState, _recoveryState),
                new Connection(_recoveryState, _defaultState),
                new Connection(_stunState, _stunState),
                new Connection(_defaultState, _attackState),
                new Connection(_attackState, _defaultState)
            );

            _defaultPosition = transform.position;
        }

        private void OnEnable()
        {
            _defaultState.DecidedToJump += OnDecidedToJump;
            _defaultState.DecidedToAttack += OnDecidedToAttack;
            _jumpState.DoneJumping += OnDoneJumping;
            _stunState.DoneBeingStunned += OnDoneBeingStunned;
            _recoveryState.DoneRecovering += OnDoneRecovering;
            _attackState.DoneAttacking += OnDoneAttacking;
        }

        private void OnDisable()
        {
            _defaultState.DecidedToJump -= OnDecidedToJump;
            _defaultState.DecidedToAttack -= OnDecidedToAttack;
            _jumpState.DoneJumping -= OnDoneJumping;
            _stunState.DoneBeingStunned -= OnDoneBeingStunned;
            _recoveryState.DoneRecovering -= OnDoneRecovering;
            _attackState.DoneAttacking -= OnDoneAttacking;
        }

        private void OnDecidedToJump()
        {
            _stateMachine.TryChangeState(_jumpState);
        }

        private void OnDecidedToAttack()
        {
            _stateMachine.TryChangeState(_attackState);
        }

        private void OnDoneJumping()
        {
            _stateMachine.TryChangeState(_defaultState);
        }

        private void OnDoneBeingStunned()
        {
            _stateMachine.TryChangeState(_defaultState);
        }

        private void OnDoneRecovering()
        {
            _stateMachine.TryChangeState(_defaultState);
        }

        private void OnDoneAttacking()
        {
            _stateMachine.TryChangeState(_defaultState);
        }

        private void Update()
        {
            _stateMachine.CurrentState.ProcessState();

            TryRecovering();
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
                    
                    Vector2 closestPoint = tmp.ClosestPoint(transform.position);
                    Instantiate(_hitParts, closestPoint, Quaternion.identity);
                    TakeDamage(25.0f, closestPoint);
                }

                _collidingWithDamage = true;
                return;
            }

            _collidingWithDamage = false;
        }

        private void TakeDamage(float damage, Vector2 source)
        {
            _damagePercentage += damage;

            bool below = transform.position.y < source.y;
            float downwardsBonus = below ? 2.0f : 1.0f;

            Vector2 direction = ((Vector2)transform.position - source).normalized;
            float knockback = (_baseKnockbackForce + _damagePercentage) * _definition.WeightModifier;
            Rigidbody2D.AddForce(direction * knockback);
            Rigidbody2D.AddForce((below ? Vector2.down : Vector2.up) * (knockback * 0.75f * downwardsBonus));
            
            _characterDisplay.UpdateDamageSlider(_damagePercentage);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("DeathPlane"))
            {
                Died?.Invoke(_lastDamageWasFromPlayer, transform.position);

                _rigidbody2D.velocity = Vector2.zero;
                _damagePercentage = 0.0f;
                transform.position = Vector2.zero + (Vector2.up * 5);
                
                _characterDisplay.UpdateDamageSlider(_damagePercentage);
                
                _lastDamageWasFromPlayer = false;
            }
        }

        private void TryRecovering()
        {
            if (_stateMachine.CurrentState == _recoveryState || _stateMachine.CurrentState == _stunState)
                return;
            
            if (transform.position.y < _recoveryThreshold)
            {
                _stateMachine.TryChangeState(_recoveryState);
            }
        }

        public void ResetPosition()
        {
            _stateMachine.TryChangeState(_immobileState);
            Rigidbody2D.velocity = Vector2.zero;
            transform.position = _defaultPosition;
        }

        public float GetDirectionFacingMultiplier()
        {
            return MovementDirection.x >= 0.0f ? 1.0f : -1.0f;
        }
    }
}