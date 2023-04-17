using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound 
{
    public string name;
    public AudioClip clip;

    [Range(0,1)]
    public float volume;

    [Range(0,3)]
    public float pitch;

    public bool loop;

    [HideInInspector]
    public AudioSource source;

    public Sound()
    {
        this.name = "";
        this.clip = null;
        this.volume = 0.5f;
        this.pitch = 1;
    }

    public Sound(string name, float volume, float pitch, bool loop)
    {
        this.name = name;
        this.volume = volume;
        this.pitch = pitch;
        this.loop = false;
    }

    public Sound(AudioClip clip ,string name, float volume, float pitch, bool loop)
    {
        this.clip = clip;
        this.name = name;
        this.volume = volume;
        this.pitch = pitch;
        this.loop = loop;
    }
}
