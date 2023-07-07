using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MHR.GameObjectManagement;
using UnityEditorInternal;

namespace AudioManagement.SFX
{
    public class SFXManager : MonoBehaviour
    {
        [SerializeField] private int _maxAudioSources;
        [SerializeField] private int _maxLoopingAudioSources;
        [SerializeField] private AudioData _audioData;
        private static SFXManager _instance;
        private List<AudioSource> _audioSources = new List<AudioSource>();
        private List<AudioSource> _loopingAudioSources = new List<AudioSource>();
        private Queue<ManagedAudio> _managedAudio = new Queue<ManagedAudio>();
        private Queue<ManagedAudio> _loopingManagedAudio = new Queue<ManagedAudio>();
        public static SFXManager Instance => _instance;

        private void OnEnable()
        {
            /*if (_instance == null)
            {
                GameObjectUtility.DontDestroyOnLoad<SFXManager>(this);*/
                _instance = this;
            //}

            AddAudioSources(false);
            AddAudioSources(true);
        }

        private void AddAudioSources(bool looping)
        {
            int max = looping ? _maxLoopingAudioSources : _maxAudioSources;

            for (int i = 0; i < max; ++i)
            {
                AudioSource tmp = gameObject.AddComponent<AudioSource>();
                InternalEditorUtility.SetIsInspectorExpanded(tmp, false);
                tmp.loop = looping;
                
                if (looping)
                {
                    _loopingAudioSources.Add(tmp);
                }
                else
                {
                    _audioSources.Add(tmp);
                }
            }
        }

        private void Update()
        {
            PlaySounds();
            PlayLoopingSounds();
        }

        public void QueueSound(string name)
        {
            ManagedAudio audioToPlay = _audioData.GetAudio(name);
            if (audioToPlay == null || _managedAudio.Contains(audioToPlay))
            {
                return;
            }

            _managedAudio.Enqueue(audioToPlay);
        }

        public void QueueLoopingSound(string name)
        {
            ManagedAudio audioToPlay = _audioData.GetAudio(name);
            if (audioToPlay == null || _managedAudio.Contains(audioToPlay))
            {
                return;
            }

            _loopingManagedAudio.Enqueue(audioToPlay);
        }

        public void StopLoopingSound(string name)
        {
            ManagedAudio audioToStop = _audioData.GetAudio(name);
            if (audioToStop == null)
            {
                return;
            }

            foreach (AudioSource audioSource in _loopingAudioSources)
            {
                if (audioSource.clip == audioToStop.Clip)
                {
                    if (audioToStop.LetFinishIfLooping)
                    {
                        audioSource.loop = false;
                    }
                    else
                    {
                        audioSource.Stop();
                    }

                    return;
                }
            }
        }

        private void PlaySounds()
        {
            while (_managedAudio.Count > 0)
            {
                ManagedAudio tmp = _managedAudio.Dequeue();
                GetFreeAudioSource(tmp)?.Play();
            }
        }

        private void PlayLoopingSounds()
        {
            while (_managedAudio.Count > 0)
            {
                ManagedAudio tmp = _managedAudio.Dequeue();
                GetFreeAudioSource(tmp, true)?.Play();
            }
        }

        private AudioSource GetFreeAudioSource(ManagedAudio managedAudio, bool looping = false)
        {
            foreach (AudioSource audioSource in _audioSources)
            {
                if (!audioSource.isPlaying)
                {
                    managedAudio.AssignAudioSourceValues(audioSource, looping);
                    return audioSource;
                }
            }

            Debug.LogWarning("Ran out of AudioSources and couldn't play sound! Consider increasing the max number of AudioSource components.");
            return null;
        }
    }
}