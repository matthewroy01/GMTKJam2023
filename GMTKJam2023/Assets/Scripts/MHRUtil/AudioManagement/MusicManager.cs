using System.Collections;
using UnityEngine;
using MHR.GameObjectManagement;

namespace AudioManagement.Music
{
    public class MusicManager : MonoBehaviour
    {
        [SerializeField] private AudioData _audioData;
        private static MusicManager _instance;
        private MusicAudioSource _primaryMusicAudioSource;
        private MusicAudioSource _secondaryMusicAudioSource;
        private bool _usingPrimaryMusicAudioSource = true;
        private Coroutine _fadeMusicCoroutine;
        public static MusicManager Instance => _instance;

        private void OnEnable()
        {
            if (_instance == null)
            {
                GameObjectUtility.DontDestroyOnLoad<MusicManager>(this);
                _instance = this;
            }

            AddMusicAudioSources();
        }

        private void AddMusicAudioSources()
        {
            _primaryMusicAudioSource = new MusicAudioSource(gameObject.AddComponent<AudioSource>());
            _secondaryMusicAudioSource = new MusicAudioSource(gameObject.AddComponent<AudioSource>());
        }

        public void PlayMusic(string name = "", bool fadeSimultaneously = false, bool pauseInsteadOfStop = false, float fadeDuration = 1.0f, float silenceDuration = 0.0f)
        {
            ManagedAudio audioToPlay = _audioData.GetAudio(name);
            if (audioToPlay ==  null)
            {
                return;
            }

            MusicAudioSource musicAudioSource = _usingPrimaryMusicAudioSource ? _primaryMusicAudioSource : _secondaryMusicAudioSource;
            if (GetMusicIsTheSame(name, musicAudioSource))
            {
                FadeMusic(musicAudioSource, pauseInsteadOfStop, fadeDuration);
                return;
            }

            if (!_primaryMusicAudioSource.IsPlaying && !_secondaryMusicAudioSource.IsPlaying)
            {
                _primaryMusicAudioSource.AssignValues(audioToPlay);
                _primaryMusicAudioSource.Play();
                return;
            }

            musicAudioSource.AssignValues(audioToPlay);

            _usingPrimaryMusicAudioSource = !_usingPrimaryMusicAudioSource;

            FadeMusic(audioToPlay.Volume, fadeSimultaneously, pauseInsteadOfStop, fadeDuration, silenceDuration);
        }

        private bool GetMusicIsTheSame(string name, MusicAudioSource musicAudioSource)
        {
            if (musicAudioSource.Name == name)
            {
                return true;
            }
            return false;
        }

        public void StopMusic()
        {

        }

        private void FadeMusic(float volume, bool fadeSimultaneously, bool pauseInsteadOfStop, float fadeDuration, float silenceDuration = 0.0f)
        {
            if (_fadeMusicCoroutine != null)
            {
                StopCoroutine(_fadeMusicCoroutine);
            }

            _fadeMusicCoroutine = StartCoroutine(FadeMusicCoroutine(volume, fadeSimultaneously, pauseInsteadOfStop, fadeDuration, silenceDuration));
        }

        private IEnumerator FadeMusicCoroutine(float volume, bool fadeSimultaneously, bool pauseInsteadOfStop, float fadeDuration, float silenceDuration = 0.0f)
        {
            if (fadeSimultaneously)
            {
                yield return FadeSimultaneously(_usingPrimaryMusicAudioSource ? _secondaryMusicAudioSource : _primaryMusicAudioSource,
                    _usingPrimaryMusicAudioSource ? _primaryMusicAudioSource : _secondaryMusicAudioSource,
                    volume, pauseInsteadOfStop, fadeDuration);
            }
            else
            {
                yield return Fade(_usingPrimaryMusicAudioSource ? _secondaryMusicAudioSource : _primaryMusicAudioSource,
                    _usingPrimaryMusicAudioSource ? _primaryMusicAudioSource : _secondaryMusicAudioSource,
                    volume, pauseInsteadOfStop, fadeDuration, silenceDuration);
            }

            IEnumerator Fade(MusicAudioSource from, MusicAudioSource to, float volume, bool pauseInsteadOfStop, float fadeDuration, float silenceDuration)
            {
                float timer = 0.0f;

                while (timer < fadeDuration)
                {
                    from.FadeVolume(0.0f, timer / fadeDuration, true);

                    timer += Time.deltaTime;
                    yield return null;
                }

                if (from.IsPlaying)
                {
                    if (pauseInsteadOfStop)
                    {
                        from.Pause();
                    }
                    else
                    {
                        from.Stop();
                    }
                }

                timer = 0.0f;

                while (timer < silenceDuration)
                {
                    timer += Time.deltaTime;
                    yield return null;
                }

                timer = 0.0f;

                if (!to.IsPlaying)
                {
                    to.SetVolume(0.0f);
                    to.Play();
                }

                while (timer < fadeDuration)
                {
                    to.FadeVolume(volume, timer / fadeDuration, false);

                    timer += Time.deltaTime;
                    yield return null;
                }
            }

            IEnumerator FadeSimultaneously(MusicAudioSource from, MusicAudioSource to, float volume, bool pauseInsteadOfStop, float fadeDuration)
            {
                float timer = 0.0f;

                if (!to.IsPlaying)
                {
                    to.SetVolume(0.0f);
                    to.Play();
                }

                while (timer < fadeDuration)
                {
                    to.FadeVolume(volume, timer / fadeDuration, false);
                    from.FadeVolume(0.0f, timer / fadeDuration, true);

                    timer += Time.deltaTime;
                    yield return null;
                }

                if (from.IsPlaying)
                {
                    if (pauseInsteadOfStop)
                    {
                        from.Pause();
                    }
                    else
                    {
                        from.Stop();
                    }
                }
            }
        }

        private void FadeMusic(MusicAudioSource musicAudioSource, bool pauseInsteadOfStop, float fadeDuration)
        {
            if (_fadeMusicCoroutine != null)
            {
                StopCoroutine(_fadeMusicCoroutine);
            }

            _fadeMusicCoroutine = StartCoroutine(FadeMusicCoroutine(musicAudioSource, pauseInsteadOfStop, fadeDuration));
        }

        private IEnumerator FadeMusicCoroutine(MusicAudioSource musicAudioSource, bool pauseInsteadOfStop, float fadeDuration)
        {
            float timer = 0.0f;

            bool isPlaying = musicAudioSource.IsPlaying;
            if (!isPlaying)
            {
                musicAudioSource.SetVolume(0.0f);
                musicAudioSource.Play();
            }

            while (timer < fadeDuration)
            {
                if (isPlaying)
                {
                    musicAudioSource.FadeVolume(0.0f, timer / fadeDuration, true);
                }
                else
                {
                    musicAudioSource.FadeVolume(musicAudioSource.IntendedVolume, timer / fadeDuration, false);
                }

                timer += Time.deltaTime;
                yield return null;
            }

            if (isPlaying)
            {
                if (pauseInsteadOfStop)
                {
                    musicAudioSource.Pause();
                }
                else
                {
                    musicAudioSource.Stop();
                }
            }
        }
    }
}