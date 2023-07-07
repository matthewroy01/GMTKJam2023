using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AudioManagement
{
    [System.Serializable]
    public class ManagedAudio
    {
        [SerializeField] private string _name;
        [SerializeField] private AudioClip _clip;
        [SerializeField] private float _volume = 0.5f;
        [SerializeField] private float _minPitch = 0.9f;
        [SerializeField] private float _maxPitch = 1.1f;
        [SerializeField] private bool _letFinishIfLooping = true;
        public string Name => _name;
        public AudioClip Clip => _clip;
        public float Volume => _volume;
        public float MinPitch => _minPitch;
        public float MaxPitch => _maxPitch;
        public bool LetFinishIfLooping => _letFinishIfLooping;

        public ManagedAudio(AudioClip clip)
        {
            _clip = clip;
        }

        public ManagedAudio(AudioClip clip, float volume)
        {
            _clip = clip;
            _volume = volume;
        }

        public ManagedAudio(AudioClip clip, float volume, float minPitch, float maxPitch)
        {
            _clip = clip;
            _volume = volume;
            _minPitch = minPitch;
            _maxPitch = maxPitch;
        }

        public ManagedAudio(AudioClip clip, float volume, float minPitch, float maxPitch, bool letFinishIfLooping)
        {
            _clip = clip;
            _volume = volume;
            _minPitch = minPitch;
            _maxPitch = maxPitch;
            _letFinishIfLooping = letFinishIfLooping;
        }

        public void AssignAudioSourceValues(AudioSource audioSource, bool looping = false)
        {
            audioSource.clip = _clip;
            audioSource.volume = _volume;
            audioSource.pitch = UnityEngine.Random.Range(_minPitch, _maxPitch);
            audioSource.loop = looping;
        }
    }
}