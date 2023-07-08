using DG.Tweening;
using PlatformFighter.Character;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PlatformFighter.UI
{
    public class CharacterDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private Image _portrait;
        [SerializeField] private Slider _damageSlider;
        [SerializeField] private Image _damageSliderFill;
        [SerializeField] private RectTransform _damageSliderRectTransform;
        [SerializeField] private Color _minColor;
        [SerializeField] private Color _maxColor;

        public void Initialize(CharacterDefinition definition)
        {
            _nameText.text = definition.Name;
            _portrait.sprite = definition.Portrait;
            _portrait.color = definition.PortraitTint;
        }

        public void UpdateDamageSlider(float percentage)
        {
            float value = Mathf.Min(percentage, 100.0f) / 100.0f;
            _damageSlider.value = value;
            _damageSliderFill.color = Color.Lerp(_minColor, _maxColor, value);

            _damageSliderRectTransform.DOShakePosition(0.25f, Vector2.right * 20.0f, 50);
        }
    }
}