using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
   public static OptionUI Instance { get; private set; }
   
   [SerializeField] private Button closeButton;
   [SerializeField] private Button soundEffectButton;
   [SerializeField] private Button musicButton;
   [SerializeField] private TextMeshProUGUI soundEffectButtonText;
   [SerializeField] private TextMeshProUGUI musicButtonText;

   [SerializeField] private Button moveDownButton;
   [SerializeField] private TextMeshProUGUI moveDownText;
   [SerializeField] private Button moveUpButton;
   [SerializeField] private TextMeshProUGUI moveUpText;
   [SerializeField] private Button moveLeftButton;
   [SerializeField] private TextMeshProUGUI moveLeftText;
   [SerializeField] private Button moveRightButton;
   [SerializeField] private TextMeshProUGUI moveRightText;
   [SerializeField] private Button interactButton;
   [SerializeField] private TextMeshProUGUI interactText;
   [SerializeField] private Button interactAlternateButton;
   [SerializeField] private TextMeshProUGUI interactAlternateText;
   [SerializeField] private Button pauseButton;
   [SerializeField] private TextMeshProUGUI pauseText;

   [SerializeField] private Transform pressToRebindTransform;

   private Action onCloseButtonAction;
   
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
      
      closeButton.onClick.AddListener(() =>
      {
         onCloseButtonAction();
         Hide();
      });
      
      moveUpButton.onClick.AddListener(() =>
      {
         RebindKey(GameInputManager.Bindings.Move_UP);
      });
      
      moveDownButton.onClick.AddListener(() =>
      {
         RebindKey(GameInputManager.Bindings.Move_Down);
      });

      moveLeftButton.onClick.AddListener(() =>
      {
         RebindKey(GameInputManager.Bindings.Move_Left);
      });
      
      moveRightButton.onClick.AddListener(() =>
      {
         RebindKey(GameInputManager.Bindings.Move_Right);
      });
      
      interactButton.onClick.AddListener(() =>
      {
         RebindKey(GameInputManager.Bindings.Interact);
      });
      
      interactAlternateButton.onClick.AddListener(() =>
      {
         RebindKey(GameInputManager.Bindings.InteractAlternate);
      });
      
      pauseButton.onClick.AddListener(() =>
      {
         RebindKey(GameInputManager.Bindings.pause);
      });
   }

   private void Start()
   {
      Hide();
      HidePressToRebind();
      
      SoundManager.Instance.soundEffectEvent += OnSoundEffectEvent;
      AudioManager.Instance.musicVolumeEvent += OnMusicVolumeEvent;
      
      UpdateBinding();
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

   private void UpdateBinding()
   {
      moveUpText.text = GameInputManager.Instance.GetBindingsText(GameInputManager.Bindings.Move_UP);
      moveDownText.text = GameInputManager.Instance.GetBindingsText(GameInputManager.Bindings.Move_Down);
      moveLeftText.text = GameInputManager.Instance.GetBindingsText(GameInputManager.Bindings.Move_Left);
      moveRightText.text = GameInputManager.Instance.GetBindingsText(GameInputManager.Bindings.Move_Right);
      interactText.text = GameInputManager.Instance.GetBindingsText(GameInputManager.Bindings.Interact);
      interactAlternateText.text = GameInputManager.Instance.GetBindingsText(GameInputManager.Bindings.InteractAlternate);
      pauseText.text = GameInputManager.Instance.GetBindingsText(GameInputManager.Bindings.pause);
   }
   

   public void Show(Action onCloseButtonAction)
   {
      this.onCloseButtonAction = onCloseButtonAction;

      soundEffectButton.Select();
      
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

   private void ShowPressToRebind()
   {
      pressToRebindTransform.gameObject.SetActive(true);
   }

   private void HidePressToRebind()
   {
      pressToRebindTransform.gameObject.SetActive(false);
   }

   private void RebindKey(GameInputManager.Bindings bindings)
   {
      ShowPressToRebind();
      GameInputManager.Instance.RebindBindings(bindings, () =>
      {
         HidePressToRebind();
         UpdateBinding();
      });
   }
}
