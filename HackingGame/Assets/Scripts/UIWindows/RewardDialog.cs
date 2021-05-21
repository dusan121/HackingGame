using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RewardDialog : WindowAbstract
{

    public static RewardDialog instance;
    private void Awake()
    {
        instance = this;
    }

    public Text nukesText, trapsText, xpText;

    public void SetNukeNumber(int numberOfNukes)
    {
        nukesText.text = numberOfNukes + " Nukes";
        nukesText.gameObject.SetActive(numberOfNukes != 0);
    }
    public void SetTrapsNumber(int numberOfTraps)
    {
        trapsText.text = numberOfTraps + " Traps";
        trapsText.gameObject.SetActive(numberOfTraps != 0);
    }
    public void SetXpNumber(int numberOfXp)
    {
        xpText.text = numberOfXp + " XP";
        xpText.gameObject.SetActive(numberOfXp != 0);
    }
    public void BackButtonClicked()
    {
        UiManager.instance.OpenWindow(StartScreen.instance);
    }
}
