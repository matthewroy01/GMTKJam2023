using System;
using System.Collections;
using PlatformFighter.Player;
using UnityEngine;

namespace PlatformFighter.Management
{
    public class MatchManager : MonoBehaviour
    {
        [SerializeField] private PlayerPlatform _playerPlatform;

        [Header("Start of Match Countdown")]
        [SerializeField] private float _initialDelay;
        [SerializeField] private float _timeBetweenNumbers;

        private Coroutine _countdownRoutine;
        
        private void Awake()
        {
            Countdown();
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
        }
    }
}