using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelFinishedDialog : WindowAbstract
{
    public static LevelFinishedDialog instance;

    private void Awake()
    {
        instance = this;
    }

    public Text title;
    public GameObject restartBtn;

    public override void Open()
    {
        base.Open();
        restartBtn.SetActive(!LevelController.levelCompletedSuccessfully);
        title.text = LevelController.levelCompletedSuccessfully ? "LEVEL COMPLETED" : "GAME OVER";
    }

    public void RestartLevelClicked()
    {
        LevelController.instance.Restart();
        UiManager.instance.OpenWindow(GameplayScreen.instance);
    }
    public void NewGameClicked()
    {
        LevelController.instance.SpownNewLevel();
        UiManager.instance.OpenWindow(GameplayScreen.instance);
    }
    public void BackButtonClicked()
    {
        LevelController.instance.DestroyCurrentLevel();
        UiManager.instance.OpenWindow(StartScreen.instance);
    }
}
