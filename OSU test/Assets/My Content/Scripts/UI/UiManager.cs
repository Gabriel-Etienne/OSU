using System;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] private SongManager _songManager;
    public Slider musicProgressBar;


    private void Awake()
    {
        musicProgressBar.maxValue = _songManager.MusicLength;
    }
    private void Update()
    {
        musicProgressBar.value = _songManager.MusicTime;
    }
}
