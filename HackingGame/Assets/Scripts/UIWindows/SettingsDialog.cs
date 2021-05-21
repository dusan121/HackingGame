using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsDialog : WindowAbstract
{
    public static SettingsDialog instance;

    public Toggle sounds;
    [Space]
    public Text nodeCoutText;
    public Slider nodeCountSlider;
    [Space]
    public Text treasuresText;
    public Slider treasuresSlider;
    [Space]
    public Text firewallsText;
    public Slider firewallsSlider;
    [Space]
    public Text spamsText;
    public Slider spamsSlider;
    [Space]
    public Text spamDecreaseText;
    public Slider spamDecreaseSlider;
    [Space]
    public Text trapTimeText;
    public Slider trapTimeSlider;

    //public static bool soundsOn;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        sounds.isOn = PlayerPrefs.GetInt("sounds") == 1;

        nodeCountSlider.value = LevelController.instance.nodeCount;
        nodeCoutText.text = "Node Count - " + LevelController.instance.nodeCount;

        treasuresSlider.value = LevelController.instance.treasureNodeCount;
        treasuresText.text = "Treasures - " + LevelController.instance.treasureNodeCount;

        firewallsSlider.value = LevelController.instance.firewallNodeCount;
        firewallsText.text = "Firewalls - " + LevelController.instance.firewallNodeCount;

        spamsSlider.value = LevelController.instance.spamNodeCount;
        spamsText.text = "Spams - " + LevelController.instance.spamNodeCount;

        spamDecreaseSlider.value = LevelController.instance.spamNodeCount;
        spamDecreaseText.text = "Spam Decrease - " + LevelController.instance.spamNodeCount;

        trapTimeSlider.value = LevelController.instance.trapDelayTime;
        trapTimeText.text = "Trap time - " + LevelController.instance.trapDelayTime;
    }

    public void BackButtonClicked()
    {
        UiManager.instance.OpenWindow(StartScreen.instance);
    }

    public void SoundToggleClicked(bool enabled)
    {
        SoundManager.instance.MuteMaster(!enabled);
    }

    public void NodeCountChanged(float value)
    {
        nodeCoutText.text = "Node Count - " + (int)value;
        LevelController.instance.nodeCount = (int)value;
    }
    public void TreasuresChanged(float value)
    {
        treasuresText.text = "Treasures - " + (int)value;
        LevelController.instance.treasureNodeCount = (int)value;
    }
    public void FirewallsChanged(float value)
    {
        firewallsText.text = "Firewalls - " + (int)value;
        LevelController.instance.firewallNodeCount = (int)value;
    }
    public void SpamsChanged(float value)
    {
        spamsText.text = "Spams - " + (int)value;
        LevelController.instance.spamNodeCount = (int)value;
    }
    public void SpamDecreaseChanged(float value)
    {
        spamDecreaseText.text = "Spam Decrease - " + (int)value;
        LevelController.instance.spamNodeDecrease = value;
    }
    public void TrapTimeChanged(float value)
    {
        trapTimeText.text = "Trap time - " + value;
        LevelController.instance.trapDelayTime = value;
    }
}
