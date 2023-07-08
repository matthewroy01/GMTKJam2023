using System;
using UnityEngine;

namespace PlatformFighter.Character
{
    public class CharacterStunState : CharacterState
    {
        public event Action DoneBeingStunned;
        
        [SerializeField] private float _baseStunDuration;

        private float _timer;
        
        public override void EnterState()
        {
            Character.Rigidbody2D.drag = 0.0f;
        }

        public override void ExitState()
        {
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