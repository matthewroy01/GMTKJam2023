using System;
using DG.Tweening;

namespace MHRUtil.DOTween
{
    public abstract class TweenHelper
    {
        protected Tween Tween;

        protected void TryKillTween()
        {
            Tween?.Kill();
        }

        public abstract void DoTween();

        public void StopTween()
        {
            TryKillTween();
        }
    }
}