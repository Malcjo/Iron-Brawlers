﻿using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public const string PUNCHHIT = "Jab Hit";
    public const string PUNCHMISS = "Jab Miss";
    public const string ARMOURBREAK = "Armour Break";
    public const string JUMP = "Jump Sounds";

    public Sound[] sounds;

    public LibraryLink[] links;

    public static AudioManager instance;

    public Dictionary<string, AudioLibrary> libraries;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = s.audioMixerGroup;
        }

        libraries = new Dictionary<string, AudioLibrary>();
        foreach (LibraryLink l in links) {
            libraries.Add(l.name, l.library);
        }
    }

    void Start()
    {
        Play("AlphaMusic");
    }

    public void Play (string name)
    {
        if (libraries.ContainsKey(name))
        {
            libraries[name].PlaySound();
        }
        
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "not found!");
            return;
        }
        s.source.Play();
        //Note: to play a sound from another script, use: FindObjectOfType<AudioManager>().Play(NAMEOFCONSTGOESHERE);
    }


    [System.Serializable]
    public struct LibraryLink
    {
        public string name;
        public AudioLibrary library;
    }
}
