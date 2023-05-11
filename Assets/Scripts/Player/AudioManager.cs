using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public event EventHandler musicVolumeEvent;
    
    private AudioSource audioSource;
    private float volume;

    private const string PLAYER_PREFS_MUSIC_VOLUME = "PLAYER_PREFS_MUSIC_VOLUME";

    private void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();

        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC_VOLUME, 1f);
    }

    private void Update()
    {
        audioSource.volume = volume;
        
        musicVolumeEvent?.Invoke(this,EventArgs.Empty);

    }

    public void PressMusic()
    {
        volume += .1f;
        if (volume > 1.01f)
        {
            volume = 0f;
        }
        
        PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME,volume);
        PlayerPrefs.Save();
        musicVolumeEvent?.Invoke(this,EventArgs.Empty);
    }

    public float GetVolume()
    {
        return volume;
    }
}
