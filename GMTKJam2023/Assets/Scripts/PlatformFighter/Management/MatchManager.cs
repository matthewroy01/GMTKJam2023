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
        [SerializeField] private TextMeshProUGUI _countdownText;
        [SerializeField] private CanvasGroup _countdownCanvasGroup;
        [SerializeField] private float _initialDelay;
        [SerializeField] private float _timeBetweenNumbers;
        [Header("Score")]
        [SerializeField] private TextMeshProUGUI _scoreText;
        [Header("Time")]
        [SerializeField] private TextMeshProUGUI _timeText;
        [SerializeField] private int _startingTime;
        [Header("Results Screen")]
        [SerializeField] private CanvasGroup _resultsCanvasGroup;
        [SerializeField] private TextMeshProUGUI _timeRemainingText;
        [SerializeField] private TextMeshProUGUI _finalScoreText;
        [SerializeField] private GameObject _newBestTime;
        [SerializeField] private GameObject _newBestScore;
        [SerializeField] private CanvasGroup _buttonsCanvasGroup;

        private int _score = 0;
        private float _time;
        private Tween _scorePunchScaleTween;
        private Coroutine _countdownRoutine;
        private bool _gameRunning;
        private Coroutine _gameRunningRoutine;
        
        private void Awake()
        {
            _time = _startingTime;

            SetTimerText();
            
            Countdown();
        }

        private void OnEnable()
        {
            Character.Character.Died += Character_OnDied;
            PlayerPlatform.HealthUpdated += PlayerPlatform_OnHealthUpdated;
        }

        private void OnDisable()
        {
            Character.Character.Died -= Character_OnDied;
            PlayerPlatform.HealthUpdated -= PlayerPlatform_OnHealthUpdated;
        }

        private void Character_OnDied(bool increaseScore, Vector2 position)
        {
            GameObject tmp = Instantiate(_koParts, position, Quaternion.identity).gameObject;

            tmp.transform.up = Vector2.zero - position;
            
            if (!increaseScore || !_gameRunning)
                return;
            
            _score++;
            
            _scorePunchScaleTween?.Kill();
            _scoreText.rectTransform.localScale = Vector3.one;

            _scoreText.text = _score.ToString();
            _scorePunchScaleTween = _scoreText.rectTransform.DOPunchScale(Vector2.one * 0.25f, 0.25f, 0, 0.0f);
        }

        private void PlayerPlatform_OnHealthUpdated(float health)
        {
            if (health <= 0.0f)
            {
                EndGame();
            }
        }

        private void Update()
        {
            if (!_gameRunning)
                return;
            
            if (_time - Time.deltaTime < 0.0f)
            {
                _time = 0.0f;
                
                EndGame();
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

            _countdownCanvasGroup.DOFade(1.0f, _initialDelay);

            yield return new WaitForSeconds(_timeBetweenNumbers);
            _countdownText.text = "3";
            _countdownText.DOFade(1.0f, 0.5f).From(0.0f);
            _countdownText.rectTransform.DOScale(Vector3.one * 1.5f, 1.0f).OnComplete(delegate
            {
                _countdownText.rectTransform.localScale = Vector3.one;
            });
            
            yield return new WaitForSeconds(_timeBetweenNumbers);
            _countdownText.text = "2";
            _countdownText.DOFade(1.0f, 0.5f).From(0.0f);
            _countdownText.rectTransform.DOScale(Vector3.one * 1.5f, 1.0f).OnComplete(delegate
            {
                _countdownText.rectTransform.localScale = Vector3.one;
            });
            
            yield return new WaitForSeconds(_timeBetweenNumbers);
            _countdownText.text = "1";
            _countdownText.DOFade(1.0f, 0.5f).From(0.0f);
            _countdownText.rectTransform.DOScale(Vector3.one * 1.5f, 1.0f).OnComplete(delegate
            {
                _countdownText.rectTransform.localScale = Vector3.one;
            });
            
            yield return new WaitForSeconds(_timeBetweenNumbers);
            _countdownText.text = "GO!";
            _countdownText.DOFade(1.0f, 0.1f).From(0.0f);
            _countdownText.rectTransform.DOScale(Vector3.one * 1.5f, 0.5f);

            yield return new WaitForSeconds(0.5f);

            _countdownCanvasGroup.DOFade(0.0f, 0.5f);

            // TODO: add an actual countdown with animation and stuff
            
            _playerPlatform.UnlockMovement();
            _characters.ForEach(character => character.UnlockMovement());

            _gameRunning = true;
        }

        private void SetTimerText()
        {
            TimeSpan t = TimeSpan.FromSeconds(_time);
            _timeText.text = "<mspace=0.6em>" + t.ToString(@"mm\:ss") + "</mspace>";
        }

        private void EndGame()
        {
            _gameRunning = false;
            
            _playerPlatform.ChangeToImmobileState();

            if (_gameRunningRoutine != null)
            {
                StopCoroutine(_gameRunningRoutine);
            }
            _gameRunningRoutine = StartCoroutine(EndGameRoutine());
        }

        private IEnumerator EndGameRoutine()
        {
            _resultsCanvasGroup.blocksRaycasts = _resultsCanvasGroup.interactable = true;
            
            yield return _resultsCanvasGroup.DOFade(1.0f, 1.0f);
            
            TimeSpan t = TimeSpan.FromSeconds(_time);
            _timeRemainingText.text = "Time Remaining: " + t.ToString(@"mm\:ss");
            yield return _timeRemainingText.DOFade(1.0f, 0.5f);
            
            if (PlayerPrefs.GetFloat("BestTime", _startingTime) > _time)
            {
                PlayerPrefs.SetFloat("BestTime", _time);
                _newBestTime.SetActive(true);
                _newBestTime.transform.DOPunchScale(Vector3.one * 0.25f, 0.5f, 0, 0.0f);
            }

            _finalScoreText.text = "Score: " + _score;
            yield return _finalScoreText.DOFade(1.0f, 0.5f);

            if (PlayerPrefs.GetInt("BestScore", -1) < _score)
            {
                PlayerPrefs.SetInt("BestScore", _score);
                _newBestScore.SetActive(true);
                _newBestScore.transform.DOPunchScale(Vector3.one * 0.25f, 0.5f, 0, 0.0f);
            }

            yield return new WaitForSeconds(1.0f);

            yield return _buttonsCanvasGroup.DOFade(1.0f, 0.5f);
            _buttonsCanvasGroup.blocksRaycasts = _buttonsCanvasGroup.interactable = true;
        }

        public void RestartGame()
        {
            _characters.ForEach(character => character.ResetPosition());
            _playerPlatform.ResetPosition();

            _time = _startingTime;
            SetTimerText();
            _score = 0;
            _scoreText.text = _score.ToString();

            _buttonsCanvasGroup.blocksRaycasts = _buttonsCanvasGroup.interactable = false;
            _resultsCanvasGroup.blocksRaycasts = _resultsCanvasGroup.interactable = false;
            
            _resultsCanvasGroup.DOFade(0.0f, 0.5f).OnComplete(
                delegate
                {
                    _newBestTime.SetActive(false);
                    _newBestScore.SetActive(false);
                });
            
            _countdownText.text = "";
            
            Countdown();
        }
    }
}