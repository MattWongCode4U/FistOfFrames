using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAudioManager : MonoBehaviour
{
    public AudioSource backgroundMusic;
    
    public AudioSource menuButtonSFX;
    public AudioSource moveSFX;
    public AudioSource hitSFX;
    public AudioSource blockSFX;
    public AudioSource stunSFX;

    List<AudioSource> musicList = new List<AudioSource>();
    List<AudioSource> sfxList = new List<AudioSource>();

    // Start is called before the first frame update
    void Start()
    {
        musicList.Add(backgroundMusic);
        
        sfxList.Add(menuButtonSFX);
        sfxList.Add(moveSFX);
        sfxList.Add(hitSFX);
        sfxList.Add(blockSFX);
        sfxList.Add(stunSFX);

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

        backgroundMusic.Play();
    }

    //Play UI button sfx
    public void buttonSFX()
    {
        menuButtonSFX.Play();
    }

    //Set all audio sources volumes according to PlayerPref
    public void updateASVolumes()
    {
        foreach (AudioSource a in musicList)
        {
            a.volume = PlayerPrefs.GetFloat("musicVolume");
        }
        foreach (AudioSource a in sfxList)
        {
            a.volume = PlayerPrefs.GetFloat("sfxVolume");
        }
    }

    //Play player move sfx
    public void playMoveSFX()
    {
        moveSFX.Play();
    }

    //Play player getting hit sfx
    public void playHitSFX()
    {
        hitSFX.Play();
    }

    //Play successful block sfx
    public void playBlockSFX()
    {
        blockSFX.Play();
    }

    //Play stun sfx
    public void playStunSFX()
    {
        stunSFX.Play();
    }
}
