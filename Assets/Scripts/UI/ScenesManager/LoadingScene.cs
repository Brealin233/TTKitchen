using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScene : MonoBehaviour
{
    private bool goToGameScene = true;

    private void Update()
    {
        if (goToGameScene)
        {
            goToGameScene = false;

            Loader.LoadScene(Loader.Scene.GameScene);
        }
    }
}
