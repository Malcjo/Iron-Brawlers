using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public void SetMasterVolume (float masterVolume)
    {
        audioMixer.SetFloat("MasterVolume", masterVolume);
       
    }
    public void SetMusicVolume(float musicVolume)
    {
        audioMixer.SetFloat("MusicVolume", musicVolume);
        
    }
    public void SetSFXVolume(float sfxVolume)
    {
        audioMixer.SetFloat("SFXVolume", sfxVolume);
      
    }
}
