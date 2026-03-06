using System;
using UnityEngine;

public class SongManager : MonoBehaviour
{
    public Partition partition;
    
    [SerializeField] private AudioSource _musicSource;
    public float MusicTime => _musicSource.time;
    public float MusicLength => _musicSource.clip.length;

    private void OnEnable()
    {
        InputManager.OnFastForwardEvent += FastForward;
        InputManager.OnFastBackwardEvent += FastBack;
        InputManager.OnPlayEvent += PlayPauseMusic;
        InputManager.OnRestartEvent += Restart;
    }

    private void OnDisable()
    {
        InputManager.OnFastForwardEvent -= FastForward;
        InputManager.OnFastBackwardEvent -= FastBack;
        InputManager.OnPlayEvent -= PlayPauseMusic;
        InputManager.OnRestartEvent -= Restart;
    }

    private void Start()
    {
        _musicSource.Play();
        _musicSource.Pause();
        _musicSource.time = 0;
    }


    public void Restart()
    {
        _musicSource.time = 0;
        _musicSource.Play();
    }

    public void PlayPauseMusic()
    {
        if (_musicSource.isPlaying)
        {
            _musicSource.Pause();
            return;
        }
        _musicSource.UnPause();
    }

    public void FastForward()
    {
        float newTime = _musicSource.time + 5;
        if  (newTime > _musicSource.clip.length) newTime = _musicSource.clip.length;
        _musicSource.time = newTime;
    }
    
    public void FastBack()
    {
        float newTime = _musicSource.time - 5;
        if  (newTime < 0) newTime = 0;
        _musicSource.time = newTime;
    }
    
    public void StopMusic()
    {
        _musicSource.Stop();
    }
}
