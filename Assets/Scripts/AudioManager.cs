using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioSource backgroundMusic;
    public AudioSource menuButtonSFX;
    public Slider musicSlider;
    public Slider sfxSlider;

    List<AudioSource> musicList = new List<AudioSource>();
    List<AudioSource> sfxList = new List<AudioSource>();

    // Start is called before the first frame update
    void Start()
    {
        musicList.Add(backgroundMusic);
        
        sfxList.Add(menuButtonSFX);

        //Fist time play check set volumes just in case
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
        }
        if (!PlayerPrefs.HasKey("sfxVolume"))
        {
            PlayerPrefs.SetFloat("sfxVolume", 1);
        }

        updateASVolumes();
        loadVolume();

        backgroundMusic.Play();
    }

    //Play UI button SFX
    public void buttonSFX()
    {
        menuButtonSFX.Play();
    }

    //Whenever music volume slider changes, update PlayerPrefs
    public void changeMusicVolume()
    {
        saveMusicVolume();
        updateASVolumes();
    }

    //Whenever sfx volume slider changes, update PlayerPrefs
    public void changeSFXVolume()
    {
        saveSFXVolume();
        updateASVolumes();
    }

    //Set all audios sources volumes according to PlayerPref
    public void updateASVolumes()
    {
        foreach(AudioSource a in musicList)
        {
            a.volume = PlayerPrefs.GetFloat("musicVolume");
        }
        foreach (AudioSource a in sfxList)
        {
            a.volume = PlayerPrefs.GetFloat("sfxVolume");
        }
    }

    //Set music volume value in PlayerPref
    void saveMusicVolume()
    {
        PlayerPrefs.SetFloat("musicVolume", musicSlider.value);
    }

    //Set SFX volume value in PlayerPref
    void saveSFXVolume()
    {
        PlayerPrefs.SetFloat("sfxVolume", sfxSlider.value);
    }

    //Adjust sliders to saved PlayerPref values
    void loadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");
    }
}
