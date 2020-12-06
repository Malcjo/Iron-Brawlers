using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public const string PUNCHHIT = "Punch Hit";
    public const string PUNCHMISS = "Punch Miss";


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
    }

    
    [System.Serializable]
    public struct LibraryLink
    {
        public string name;
        public AudioLibrary library;
    }
}
