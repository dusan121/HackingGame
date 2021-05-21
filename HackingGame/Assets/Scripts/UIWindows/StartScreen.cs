using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartScreen : WindowAbstract
{
    public static StartScreen instance;
    private void Awake()
    {
        instance = this;
    }

    public GameObject playBtn;
    public Button facebookLoginBtn;

    private void Start()
    {
        FacebookController.facebookLogedIn.AddListener(LogedInOnFB);
    }

    public void LogedInOnFB(string userId)
    {
        facebookLoginBtn.interactable = false;
        facebookLoginBtn.GetComponentInChildren<Text>().text = "Welcome " + userId;
    }

    public void PlayButtonClicked()
    {
        UiManager.instance.OpenWindow(GameplayScreen.instance);
        LevelController.instance.SpownNewLevel();
    }
    public void SettingsButtonClicked()
    {
        UiManager.instance.OpenWindow(SettingsDialog.instance);
    }
    public void FacebookLoginButtonClicked()
    {
        FacebookController.instance.FBLogin();
    }

}
