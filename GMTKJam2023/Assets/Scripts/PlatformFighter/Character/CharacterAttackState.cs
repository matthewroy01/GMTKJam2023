using System;
using System.Collections;
using UnityEngine;

namespace PlatformFighter.Character
{
    public class CharacterAttackState : CharacterState
    {
        public event Action DoneAttacking;

        [SerializeField] private Collider2D _hitboxCollider;
        
        private Coroutine _attackRoutine;
        
        public override void EnterState()
        {
            _attackRoutine = StartCoroutine(AttackRoutine());
        }

        public override void ExitState()
        {
            StopCoroutine(_attackRoutine);

            _hitboxCollider.enabled = false;
        }

        public override void ProcessState()
        {
            
        }

        public override void ProcessStateFixed()
        {
            
        }

        private IEnumerator AttackRoutine()
        {
            yield return new WaitForSeconds(Character.Definition.AttackWindupDuration);

            Vector3 offset = Character.Definition.AttackHitboxOffset;
            offset.x *= Character.GetDirectionFacingMultiplier();
            _hitboxCollider.transform.position = transform.position + offset;
            _hitboxCollider.gameObject.SetActive(true);
            _hitboxCollider.enabled = true;
            // TODO: activate attack visuals

            yield return new WaitForSeconds(Character.Definition.AttackActiveDuration);

            _hitboxCollider.enabled = false;
            _hitboxCollider.gameObject.SetActive(false);

            yield return new WaitForSeconds(Character.Definition.AttackCooldownDuration);
            
            DoneAttacking?.Invoke();
        }
    }
}
