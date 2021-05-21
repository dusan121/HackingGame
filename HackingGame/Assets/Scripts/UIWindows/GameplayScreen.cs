using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayScreen : WindowAbstract
{
    public static GameplayScreen instance;

    private void Awake()
    {
        instance = this;
    }

    public void BackButtonClicked()
    {
        LevelController.instance.DestroyCurrentLevel();
        UiManager.instance.OpenWindow(StartScreen.instance);
    }
}
