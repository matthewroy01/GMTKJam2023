using System;
using UnityEngine;

namespace PlatformFighter.Character
{
    public class CharacterStunState : CharacterState
    {
        public event Action DoneBeingStunned;
        
        [SerializeField] private float _baseStunDuration;
        [SerializeField] private PhysicsMaterial2D _defaultPhysicsMaterial2D;
        [SerializeField] private PhysicsMaterial2D _bouncyPhysicsMaterial2D;

        private float _timer;
        
        public override void EnterState()
        {
            Character.Rigidbody2D.sharedMaterial = _bouncyPhysicsMaterial2D;
            Character.Rigidbody2D.drag = 0.0f;
        }

        public override void ExitState()
        {
            Character.Rigidbody2D.sharedMaterial = _defaultPhysicsMaterial2D;
            Character.Rigidbody2D.drag = 6.0f;
        }

        public override void ProcessState()
        {
            if (_timer < _baseStunDuration)
            {
                _timer += Time.deltaTime;
                return;
            }

            DoneBeingStunned?.Invoke();
        }

        public override void ProcessStateFixed()
        {
            
        }
    }
}