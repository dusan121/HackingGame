using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public static UiManager instance;

    WindowAbstract currentLocation;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        currentLocation = StartScreen.instance;
    }

    public void OpenWindow(WindowAbstract window)
    {
        currentLocation.Close();
        currentLocation = window;
        currentLocation.Open();
    }
}
