using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
    // todo: add default volume 
    public static SoundManager Instance { get; private set; }

    public event EventHandler soundEffectEvent;
    
    [SerializeField] private AudioClipRefsSO audioClipRefsSO;
    private float volume;

    private const string PLAYER_PREFS_SOUNDEFFECT_VOLOUME = "PLAYER_PREFS_SOUNDEFFECT_VOLOUME";

    private void Awake()
    {
        Instance = this;

        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUNDEFFECT_VOLOUME, 1f);

    }

    private void Start()
    {
        DeliveryManager.Instance.deliverySuccessEvent += OnDeliverySuccessEvent;
        DeliveryManager.Instance.deliveryFaildEvent += OnDeliveryFailedEvent;
        PlayerController.anyPlayerSoundEvent += OnChopSoundEvent;
        ClearCounter.dropDownSoundEvent += OnDropDownSoundEvent;
        CuttingCounter.cuttingsSoundEvent += OnCuttingsSoundEvent;
        GarbageCounter.garbageSoundEvent += OnGarbageSoundEvent;
        StoveFizzleSound.Instance.stoveFizzleSoundEvent += OnStoveFizzleSoundEvent;
        PlayerStepSound.playerStepSoundEvent += OnPlayerStepSoundEvent;
        
        soundEffectEvent?.Invoke(this,EventArgs.Empty);

    }

    private void OnPlayerStepSoundEvent(object sender, EventArgs e)
    {
        PlayerStepSound playerStepSound = sender as PlayerStepSound;;
        PlaySound(audioClipRefsSO.footStep, playerStepSound.transform.position);
    }

    private void OnStoveFizzleSoundEvent(object sender, EventArgs e)
    {
        float volume = .1f;
        PlaySound(audioClipRefsSO.stoveSizzle, StoveFizzleSound.Instance.transform.position, volume);
    }

    private void OnGarbageSoundEvent(object sender, EventArgs e)
    {
        GarbageCounter garbageCounter = sender as GarbageCounter;
        PlaySound(audioClipRefsSO.trash,garbageCounter.transform.position);
    }

    private void OnCuttingsSoundEvent(object sender, EventArgs e)
    {
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaySound(audioClipRefsSO.chop,cuttingCounter.transform.position);
    }

    private void OnDropDownSoundEvent(object sender, EventArgs e)
    {
        ClearCounter clearCounter = sender as ClearCounter;
        PlaySound(audioClipRefsSO.objectDrop, clearCounter.transform.position);
    }

    private void OnChopSoundEvent(object sender, EventArgs e)
    {
        PlayerController playerController = sender as PlayerController;
        PlaySound(audioClipRefsSO.objectPicked, playerController.transform.position);
    }

    private void OnDeliveryFailedEvent(object sender, EventArgs e)
    {
        PlaySound(audioClipRefsSO.deliveryFail,DeliveryManager.Instance.transform.position);
    }

    private void OnDeliverySuccessEvent(object sender, EventArgs e)
    {
        PlaySound(audioClipRefsSO.deliverySuccess,DeliveryManager.Instance.transform.position);
    }

    private void PlaySound(List<AudioClip> audioClipList,Vector3 position,float volumeMultiple = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClipList[Random.Range(0,audioClipList.Count)],position,volume);
    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiple = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip,position,volume);
    }

    public void PressSoundEffect()
    {
        volume += .1f;
        if (volume > 1.01f)
        {
            volume = 0f;
        }

        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUNDEFFECT_VOLOUME, volume);
        PlayerPrefs.Save();
        soundEffectEvent?.Invoke(this,EventArgs.Empty);
    }

    public float GetVolume()
    {
        return volume;
    }
}

