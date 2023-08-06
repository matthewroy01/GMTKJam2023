using System;
using DG.Tweening;
using PlatformFighter.Input;
using UnityEngine;

namespace PlatformFighter.Player
{
    public class PlayerDefendState : PlayerState
    {
        public event Action DoneDefending;

        [SerializeField] private float _movementSpeed;
        [SerializeField] private Transform _spikeTop;
        [SerializeField] private Transform _spikeBottom;
        [SerializeField] private float _outTime;
        [SerializeField] private float _inTime;
        [SerializeField] private Collider2D _collider;

        //private float _timer;
        private Tween _spikeTopTween;
        private Tween _spikeBottomTween;
        private bool _buttonReleased;

        private void OnEnable()
        {
            InputManager.Instance.Ability1Released += OnAbility1Released;
        }

        private void OnDisable()
        {
            InputManager.Instance.Ability1Released -= OnAbility1Released;
        }

        private void OnAbility1Released()
        {
            _buttonReleased = true;
        }

        public override void EnterState()
        {
            _buttonReleased = false;
            
            _spikeTopTween?.Kill();
            _spikeBottomTween?.Kill();
            
            _spikeTopTween = _spikeTop.DOLocalMoveY(0.3f, _outTime);
            _spikeBottomTween = _spikeBottom.DOLocalMoveY(-0.3f, _outTime);

            _collider.enabled = true;
            
            Physics.IgnoreLayerCollision(6, 7, false);
            
            //_timer = 0.0f;
        }

        public override void ExitState()
        {
            _spikeTopTween = _spikeTop.DOLocalMoveY(0.0f, _inTime);
            _spikeBottomTween = _spikeBottom.DOLocalMoveY(-0.0f, _inTime);

            _collider.enabled = false;
            
            Physics.IgnoreLayerCollision(6, 7, true);
        }

        public override void ProcessState()
        {
            if (_buttonReleased)
            {
                DoneDefending?.Invoke();
                _buttonReleased = false;
            }
        }

        public override void ProcessStateFixed()
        {
            PlayerPlatform.DrainEnergy();
            Movement();
        }

        private void Movement()
        {
            PlayerPlatform.Rigidbody2D.velocity =
                new Vector2(PlayerPlatform.MovementDirection.x * _movementSpeed, 0.0f);
        }
    }
}