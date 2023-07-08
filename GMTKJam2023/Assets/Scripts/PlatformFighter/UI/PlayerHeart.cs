using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace PlatformFighter.UI
{
    public class PlayerHeart : MonoBehaviour
    {
        [SerializeField] private Image _overlay;
        
        public void Restore()
        {
            _overlay.rectTransform.DOScale(Vector3.one, 0.0f);
            _overlay.DOFade(1.0f, 0.0f);
        }

        public void Lose()
        {
            _overlay.rectTransform.DOScale(Vector3.one * 2.0f, 0.5f);
            _overlay.rectTransform.DOShakePosition(0.5f, Vector3.right * 50.0f, 100);
            _overlay.DOFade(0.0f, 0.25f).SetDelay(0.25f);
        }
    }
}
