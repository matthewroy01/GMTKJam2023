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
            _overlay.rectTransform.DOScale(Vector3.one * 1.5f, 0.4f);
            _overlay.rectTransform.DOShakePosition(0.4f, Vector3.right * 40.0f, 75);
            _overlay.DOFade(0.0f, 0.25f).SetDelay(0.25f);
        }
    }
}
