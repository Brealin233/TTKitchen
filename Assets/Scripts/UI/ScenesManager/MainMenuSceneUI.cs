using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuSceneUI : MonoBehaviour
{
    [SerializeField] private Button playGameButton;
    [SerializeField] private Button exitGameButton;

    private void Awake()
    {
        playGameButton.onClick.AddListener(() =>
        {
            Loader.LoadScene(Loader.Scene.LoadingScene);
        });
        
        // todo: consider add main music
        
        exitGameButton.onClick.AddListener(Application.Quit);

        Time.timeScale = 1f;
    }
}
