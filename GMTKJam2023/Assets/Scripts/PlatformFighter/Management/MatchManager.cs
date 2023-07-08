using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using PlatformFighter.Player;
using TMPro;
using UnityEngine;

namespace PlatformFighter.Management
{
    public class MatchManager : MonoBehaviour
    {
        [SerializeField] private PlayerPlatform _playerPlatform;
        [SerializeField] private List<Character.Character> _characters = new();
        [SerializeField] private ParticleSystem _koParts;
        [Header("Start of Match Countdown")]
        [SerializeField] private float _initialDelay;
        [SerializeField] private float _timeBetweenNumbers;
        [Header("Score")]
        [SerializeField] private TextMeshProUGUI _scoreText;
        [Header("Time")]
        [SerializeField] private TextMeshProUGUI _timeText;
        [SerializeField] private int _startingTime;

        private int _score = 0;
        private float _time;
        private Tween _scorePunchScaleTween;

        private Coroutine _countdownRoutine;
        
        private void Awake()
        {
            _time = _startingTime;

            SetTimerText();
            
            Countdown();
        }

        private void OnEnable()
        {
            Character.Character.Died += Character_OnDied;
        }

        private void OnDisable()
        {
            Character.Character.Died -= Character_OnDied;
        }

        private void Character_OnDied(bool increaseScore, Vector2 position)
        {
            GameObject tmp = Instantiate(_koParts, position, Quaternion.identity).gameObject;

            tmp.transform.up = Vector2.zero - position;
            
            if (!increaseScore)
                return;
            
            _score++;
            
            _scorePunchScaleTween?.Kill();
            _scoreText.rectTransform.localScale = Vector3.one;

            _scoreText.text = _score.ToString();
            _scorePunchScaleTween = _scoreText.rectTransform.DOPunchScale(Vector2.one * 0.25f, 0.25f, 0, 0.0f);
        }

        private void Update()
        {
            if (_time - Time.deltaTime < 0.0f)
            {
                _time = 0.0f;
                
                // TODO: end the game
            }
            else
            {
                _time -= Time.deltaTime;
            }
            
            SetTimerText();
        }

        private void Countdown()
        {
            if (_countdownRoutine != null)
                StopCoroutine(_countdownRoutine);

            _countdownRoutine = StartCoroutine(CountdownRoutine());
        }

        private IEnumerator CountdownRoutine()
        {
            yield return new WaitForSeconds(_initialDelay);
            
            // TODO: add an actual countdown with animation and stuff
            
            _playerPlatform.UnlockMovement();
            _characters.ForEach(character => character.UnlockMovement());
        }

        private void SetTimerText()
        {
            TimeSpan t = TimeSpan.FromSeconds(_time);
            _timeText.text = "<mspace=0.6em>" + t.ToString(@"mm\:ss") + "</mspace>";
        }
    }
}