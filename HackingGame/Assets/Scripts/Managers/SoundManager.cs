using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    private void Awake()
    {
        instance = this;
    }

    public AudioMixer master;
    Dictionary<string, AudioSource> sounds;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("sounds"))
        {
            MuteMaster(false);
        }
        else
        {
            MuteMaster(PlayerPrefs.GetInt("sounds") ==0);
        }
        sounds = new Dictionary<string, AudioSource>();
        AudioSource[] audios = GetComponentsInChildren<AudioSource>();
        foreach (AudioSource item in audios)
        {
            sounds.Add(item.clip.name, item);
        }
    }

    public void PlaySound(string soundName,bool loop)
    {
        if (sounds.ContainsKey(soundName))
        {
            sounds[soundName].loop = loop;
            sounds[soundName].Play();
        }
    }

    public void StopSound(string soundName)
    {
        if (sounds.ContainsKey(soundName))
        {
            sounds[soundName].Stop();
        }
    }

    public void StopLevelSounds()
    {
        StopSound("hacking");
        StopSound("alarm");
    }

    public void MuteMaster(bool mute)
    {
        float volume = 0;
        int soundForPrefs = 1;
        if (mute)
        {
            volume = -80;
            soundForPrefs = 0;
        }
        master.SetFloat("MasterVolume", volume);
        PlayerPrefs.SetInt("sounds", soundForPrefs);
    }
}
