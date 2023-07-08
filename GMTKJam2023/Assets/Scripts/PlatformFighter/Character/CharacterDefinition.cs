using NaughtyAttributes;
using UnityEngine;

namespace PlatformFighter.Character
{
    [CreateAssetMenu(fileName = "New Character Definition", menuName = "ScriptableObjects/Character Definition", order = 1)]
    public class CharacterDefinition : ScriptableObject
    {
        public float MovementSpeed => _movementSpeed;
        public float JumpForce => _jumpForce;
        public int NumJumps => _numJumps;
        public float WeightModifier => _weightModifier;
        public float AirSpeedMultiplier => _airSpeedMultiplier;
        
        [SerializeField] private float _movementSpeed;
        [SerializeField] private float _jumpForce;
        [SerializeField, Range(1, 10)] private int _numJumps;
        [SerializeField] private float _weightModifier = 1.0f;
        [SerializeField] private float _airSpeedMultiplier = 1.0f;
    }
}