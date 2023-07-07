using System;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace MHRUtil.DOTween.Transform
{
    [Serializable]
    public class PunchPositionTweenHelper : TweenHelper
    {
        [SerializeField] private UnityEngine.Transform _target;
        [SerializeField] private bool _allPunchValuesTheSame = true;
        [SerializeField] private bool _punchIsRandomRangeBetweenPositiveAndNegative = false;
        [ShowIf("_allPunchValuesTheSame"), AllowNesting]
        [SerializeField] private float _punchFloat;
        [HideIf("_allPunchValuesTheSame"), AllowNesting]
        [SerializeField] private Vector3 _punchVector;
        [SerializeField] private float _duration;
        [SerializeField] private int _vibrato = 10;
        [SerializeField] private float _elasticity = 1.0f;
        
        public override void DoTween()
        {
            TryKillTween();

            Vector3 punch;
            punch.x = _allPunchValuesTheSame ? GetRandomValue(_punchFloat) : GetRandomValue(_punchVector.x);
            punch.y = _allPunchValuesTheSame ? GetRandomValue(_punchFloat) : GetRandomValue(_punchVector.y);
            punch.z = _allPunchValuesTheSame ? GetRandomValue(_punchFloat) : GetRandomValue(_punchVector.z);

            Tween = _target.DOPunchPosition(punch, _duration, _vibrato, _elasticity);
        }

        private float GetRandomValue(float baseFloat)
        {
            return _punchIsRandomRangeBetweenPositiveAndNegative ?
                UnityEngine.Random.Range(Mathf.Abs(baseFloat) * -1.0f, Mathf.Abs(baseFloat)) : baseFloat;
        }
    }
}