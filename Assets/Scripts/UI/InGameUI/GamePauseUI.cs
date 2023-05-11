using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
   [SerializeField] private Button resumeButton;
   [SerializeField] private Button mainMenuButton;
   [SerializeField] private Button optionUIButton;

   private void Awake()
   {
      resumeButton.onClick.AddListener(() =>
      {
         TTKitchenGameManager.Instance.SetGamePauseState();
      });
      
      mainMenuButton.onClick.AddListener(() =>
      {
         Loader.LoadScene(Loader.Scene.MainMenuScene);
      });
      
      optionUIButton.onClick.AddListener(() =>
      {
         OptionUI.Instance.Show();
         Hide();
      });
   }

   private void Start()
   {
      TTKitchenGameManager.Instance.gamePauseEvent += OnGamePauseEvent;
      TTKitchenGameManager.Instance.gameUnPauseEvent += OnGameUnPauseEvent;
      
      Hide();
   }

   private void OnGameUnPauseEvent(object sender, EventArgs e)
   {
      Hide();
   }

   private void OnGamePauseEvent(object sender, EventArgs e)
   {
      Show();
   }

   private void Show()
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