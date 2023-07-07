using UnityEngine;
using AudioManagement.SFX;
using AudioManagement.Music;

public class AudioTest : MonoBehaviour
{
    [SerializeField] private float _fadeDuration;
    [SerializeField] private float _silenceDuration;
    [SerializeField] private bool _fadeSimultaneously;
    [SerializeField] private bool _pauseInsteadOfStop;

    public void PlayOverworldMusic()
    {
        MusicManager.Instance.PlayMusic("Overworld Theme", _fadeSimultaneously, _pauseInsteadOfStop, _fadeDuration, _silenceDuration);
    }

    public void PlayBattleMusic()
    {
        MusicManager.Instance.PlayMusic("Battle Theme", _fadeSimultaneously, _pauseInsteadOfStop, _fadeDuration, _silenceDuration);
    }

    public void TryDummyString()
    {
        MusicManager.Instance.PlayMusic("dummy");
    }
}
