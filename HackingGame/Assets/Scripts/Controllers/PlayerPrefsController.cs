using System;
using UnityEngine;
using UnityEngine.Events;

public class IntParametarEvent : UnityEvent<int> { }

public class PlayerPrefsController : MonoBehaviour
{
    public static PlayerPrefsController instance;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        FirebaseController.cloudDataLoaded.AddListener(FirebaseDataReady);
    }

    public void FirebaseDataReady(string data)
    {
        if (data.Equals("faild"))
        {
            Debug.LogError("FIREBASE INIT FAILD");
        }
        else if (data.Equals("new_user"))
        {
            SetDefaultPrefs();
        }
        else
        {
            SetCloudDataAsLocal(data);
        }
    }

    public void SetDefaultPrefs()
    {
        PlayerPrefs.SetInt("sounds", 1);
        AddNukes(10);
        AddTraps(10);
    }

    public static IntParametarEvent onNukesCountChange = new IntParametarEvent();
    public void AddNukes(int number)
    {
        int nukes = PlayerPrefs.GetInt("nukes");
        nukes += number;
        PlayerPrefs.SetInt("nukes", nukes);
        onNukesCountChange.Invoke(nukes);
    }

    public static IntParametarEvent onTrapsCountChange = new IntParametarEvent();
    public void AddTraps(int number)
    {
        int traps = PlayerPrefs.GetInt("traps");
        traps += number;
        PlayerPrefs.SetInt("traps", traps);
        onTrapsCountChange.Invoke(traps);
    }

    public static IntParametarEvent onXpChange = new IntParametarEvent();
    public void AddXp(int number)
    {
        int xp = PlayerPrefs.GetInt("xp");
        xp += number;
        PlayerPrefs.SetInt("xp", xp);
        onXpChange.Invoke(xp);
    }

    public string GetDataForCloudSave()
    {
        CustomData customData = new CustomData();
        customData.sounds = PlayerPrefs.GetInt("sounds");
        customData.nukes = PlayerPrefs.GetInt("nukes");
        customData.traps = PlayerPrefs.GetInt("traps");
        customData.xp = PlayerPrefs.GetInt("xp");
        customData.collected_rewards = PlayerPrefs.GetString("collected_rewards");
        return JsonUtility.ToJson(customData);
    }

    public void SetCloudDataAsLocal(string jsonString)
    {
        CustomData customData = JsonUtility.FromJson<CustomData>(jsonString);
        PlayerPrefs.SetInt("sounds", customData.sounds);
        PlayerPrefs.SetInt("nukes", customData.nukes);
        PlayerPrefs.SetInt("traps", customData.traps);
        PlayerPrefs.SetInt("xp", customData.xp);
        PlayerPrefs.SetString("collected_rewards", customData.collected_rewards);
    }

    private void OnApplicationQuit()
    {
        FirebaseController.instance.WriteData();
    }
}
[Serializable]
public class CustomData
{
    public int sounds;
    public int nukes;
    public int traps;
    public int xp;
    public string collected_rewards;
}
