using System;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace MHRUtil.DOTween.Transform
{
    [Serializable]
    public class ShakePositionTweenHelper : TweenHelper
    {
        [SerializeField] private UnityEngine.Transform _target;
        [SerializeField] private bool _allShakeValuesTheSame = true;
        [SerializeField] private bool _shakeIsRandomRangeBetweenPositiveAndNegative = false;
        [ShowIf("_allShakeValuesTheSame"), AllowNesting]
        [SerializeField] private float _shakeFloat;
        [HideIf("_allShakeValuesTheSame"), AllowNesting]
        [SerializeField] private Vector3 _shakeVector;
        [SerializeField] private float _duration;
        [SerializeField] private int _vibrato = 10;
        [SerializeField] private float _elasticity = 1.0f;
        
        public override void DoTween()
        {
            TryKillTween();

            Vector3 shake;
            shake.x = _allShakeValuesTheSame ? GetRandomValue(_shakeFloat) : GetRandomValue(_shakeVector.x);
            shake.y = _allShakeValuesTheSame ? GetRandomValue(_shakeFloat) : GetRandomValue(_shakeVector.y);
            shake.z = _allShakeValuesTheSame ? GetRandomValue(_shakeFloat) : GetRandomValue(_shakeVector.z);

            Tween = _target.DOShakePosition(_duration, shake, _vibrato, _elasticity);
        }

        private float GetRandomValue(float baseFloat)
        {
            return _shakeIsRandomRangeBetweenPositiveAndNegative ?
                UnityEngine.Random.Range(Mathf.Abs(baseFloat) * -1.0f, Mathf.Abs(baseFloat)) : baseFloat;
        }
    }
}