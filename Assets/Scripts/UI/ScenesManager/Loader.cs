using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader 
{
    public enum Scene
    {
        MainMenuScene,
        GameScene,
        LoadingScene
    }

    public static void LoadScene(Scene sceneName)
    {
        SceneManager.LoadScene(sceneName.ToString());
    }
}
