using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
   public static OptionUI Instance { get; private set; }
   
   [SerializeField] private Button resumeButton;
   [SerializeField] private Button soundEffectButton;
   [SerializeField] private Button musicButton;
   [SerializeField] private TextMeshProUGUI soundEffectButtonText;
   [SerializeField] private TextMeshProUGUI musicButtonText;
   
   private void Awake()
   {
      Instance = this;

      soundEffectButton.onClick.AddListener(() =>
      {
         SoundManager.Instance.PressSoundEffect();
      });
      
      musicButton.onClick.AddListener(() =>
      {
         AudioManager.Instance.PressMusic();
      });
      
      resumeButton.onClick.AddListener(() =>
      {
         Hide();
         
         Time.timeScale = 1f;
      });
   }

   private void Start()
   {
      Hide();
      
      SoundManager.Instance.soundEffectEvent += OnSoundEffectEvent;
      AudioManager.Instance.musicVolumeEvent += OnMusicVolumeEvent;
   }

   private void OnSoundEffectEvent(object sender, EventArgs e)
   {
      soundEffectButtonText.text =
         "Sound Effect : " + SoundManager.Instance.GetVolume().ToString("F1");
   }

   private void OnMusicVolumeEvent(object sender, EventArgs e)
   {
      musicButtonText.text = "Music : " + AudioManager.Instance.GetVolume().ToString("F1");
   }
   

   public void Show()
   {
      foreach (Transform child in transform)
      {
         child.gameObject.SetActive(true);
      }
   }

   private void Hide()
   {
      foreach (Transform child in transform)
      {
         child.gameObject.SetActive(false);
      }
   }
}
