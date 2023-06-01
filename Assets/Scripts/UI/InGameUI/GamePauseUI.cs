using System;
using Unity.Netcode;
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
         NetworkManager.Singleton.Shutdown();
         Loader.LoadScene(Loader.Scene.MainMenuScene);
      });
      
      optionUIButton.onClick.AddListener(() =>
      {
         OptionUI.Instance.Show(Show);
         Hide();
      });
   }

   private void Start()
   {
      TTKitchenGameManager.Instance.gameLocalPauseEvent += OnGamePauseEvent;
      TTKitchenGameManager.Instance.gameLocalUnPauseEvent += OnGameLocalStartEvent;
      
      Hide();
   }

   private void OnGameLocalStartEvent(object sender, EventArgs e)
   {
      Hide();
   }

   private void OnGamePauseEvent(object sender, EventArgs e)
   {
      Show();
   }

   private void Show()
   {
      resumeButton.Select();
      
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
