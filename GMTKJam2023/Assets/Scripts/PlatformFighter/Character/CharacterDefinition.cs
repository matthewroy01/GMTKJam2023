using System;
using NaughtyAttributes;
using UnityEngine;

namespace PlatformFighter.Character
{
    [CreateAssetMenu(fileName = "New Character Definition", menuName = "ScriptableObjects/Character Definition", order = 1)]
    public class CharacterDefinition : ScriptableObject
    {
        public string Name => _name;
        public Sprite Portrait => _portrait;
        public Color PortraitTint => _portraitTint;
        public float MovementSpeed => _movementSpeed;
        public float JumpForce => _jumpForce;
        public int NumJumps => _numJumps;
        public float WeightModifier => _weightModifier;
        public float AirSpeedMultiplier => _airSpeedMultiplier;
        public float AttackDamage => _attackDamage;
        public float AttackWindupDuration => _attackWindupDuration;
        public float AttackActiveDuration => _attackActiveDuration;
        public float AttackCooldownDuration => _attackCooldownDuration;
        public Vector2 AttackHitboxOffset => _attackHitboxOffset;

        [SerializeField] private string _name;
        [SerializeField] private Sprite _portrait;
        [SerializeField] private Color _portraitTint;
        [SerializeField] private float _movementSpeed;
        [SerializeField] private float _jumpForce;
        [SerializeField, Range(1, 10)] private int _numJumps;
        [SerializeField] private float _weightModifier = 1.0f;
        [SerializeField] private float _airSpeedMultiplier = 1.0f;
        [Header("Attack")]
        [SerializeField] private float _attackDamage;
        [SerializeField] private float _attackWindupDuration;
        [SerializeField] private float _attackActiveDuration;
        [SerializeField] private float _attackCooldownDuration;
        [SerializeField] private Vector2 _attackHitboxOffset;
    }
}