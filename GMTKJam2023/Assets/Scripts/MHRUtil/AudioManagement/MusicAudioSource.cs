using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AudioManagement.Music
{
    public class MusicAudioSource
    {
        private string _name;
        private float _intendedVolume;
        private AudioSource _source;
        public string Name => _name;
        public float IntendedVolume => _intendedVolume;
        public bool IsPlaying => _source.isPlaying;

        public MusicAudioSource(AudioSource source)
        {
            if (source == null)
            {
                Debug.LogWarning("AudioSource passed into constructor was null.");
                return;
            }

            _source = source;
            _source.loop = true;
            _name = "";
            _intendedVolume = 0.0f;
        }

        public void AssignValues(ManagedAudio managedAudio)
        {
            managedAudio.AssignAudioSourceValues(_source, true);
            _name = managedAudio.Name;
            _intendedVolume = managedAudio.Volume;
        }

        public void Play()
        {
            _source.Play();
        }

        public void Pause()
        {
            _source.Pause();
        }

        public void Stop()
        {
            _source.Stop();
        }

        public void SetVolume(float target)
        {
            _source.volume = target;
        }

        public void FadeVolume(float target, float t, bool useIntendedVolume)
        {
            float toFadeFrom = useIntendedVolume ? _intendedVolume : 0.0f;
            _source.volume = Mathf.Lerp(toFadeFrom, target, t);
            Debug.Log(_source.volume);
        }
    }
}