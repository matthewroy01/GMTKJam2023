using System;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace MHRUtil.DOTween.Transform
{
    [Serializable]
    public class PunchScaleTweenHelper : TweenHelper
    {
        [SerializeField] private UnityEngine.Transform _target;
        [SerializeField] private bool _allPunchValuesTheSame = true;
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
            
            Tween = _target.DOPunchScale(_allPunchValuesTheSame ? Vector3.one * _punchFloat : _punchVector,
                _duration, _vibrato, _elasticity);
        }
    }
}