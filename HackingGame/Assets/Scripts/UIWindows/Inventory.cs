using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    private void OnEnable()
    {
        SetNumberOfNukes(PlayerPrefs.GetInt("nukes"));
        PlayerPrefsController.onNukesCountChange.AddListener(SetNumberOfNukes);
        SetNumberOfTraps(PlayerPrefs.GetInt("traps"));
        PlayerPrefsController.onTrapsCountChange.AddListener(SetNumberOfTraps);
        SetXp(PlayerPrefs.GetInt("xp"));
        PlayerPrefsController.onXpChange.AddListener(SetXp);
    }

    private void OnDisable()
    {
        PlayerPrefsController.onNukesCountChange.RemoveListener(SetNumberOfNukes);
        PlayerPrefsController.onTrapsCountChange.RemoveListener(SetNumberOfTraps);
        PlayerPrefsController.onXpChange.RemoveListener(SetXp);
    }

    public Text nuberOfNukes;
    public void SetNumberOfNukes(int nukes)
    {
        nuberOfNukes.text = nukes.ToString();
    }

    public Text nuberOfTraps;
    public void SetNumberOfTraps(int traps)
    {
        nuberOfTraps.text = traps.ToString();
    }

    public Text xp;
    public void SetXp(int xp)
    {
        this.xp.text = xp.ToString();
    }
}
