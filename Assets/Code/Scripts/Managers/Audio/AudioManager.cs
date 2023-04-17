using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{

    public enum SoundType
    {
        Music,
        SFX
    }

    //ISTANZA DEL SINGLETON
    public static AudioManager instance;

    //Si tratta di un oggetto di appoggio su cui inserire tutte
    //le sorgenti audio dei vari suoni
    [SerializeField]
    GameObject audioPlayer;

    //Sono le pool di suoni che l'audio manager può usare
    //I due array saranno riempiti con tutti i suoni che
    //potranno essere usati nel gioco
    [SerializeField]
    Sound[] sfxPool;
    
    [SerializeField]
    Sound[] musicPool;


    //Decidere se lasciare i valori nella classe dell'
    //audio manager o recuperarli dal Setting Manager
    [Range(0,1)]
    public float muscicVolume;
    
    [Range(0,1)]
    public float sfxVolume;


    void Awake()
    {
        //Check per vedere se esiste già un'istanza
        if(instance != null)
        {
            Destroy(this);
            return;
        }
        
        instance = this;
        DontDestroyOnLoad(gameObject);
        SetUpSoundPool();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    //Funzione necessaria per creare le pool di suoni.
    //Per ogni suono viene creata una sorgente audio applicata all'Audio Player
    void SetUpSoundPool()
    {
        //Creazione di una sorgente per ogni sfx
        foreach (Sound sound in sfxPool)
        {
            sound.source = audioPlayer.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume * sfxVolume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }

        //Creazione di una sorgente per ogni musica
        foreach (Sound sound in musicPool)
        {
            sound.source = audioPlayer.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume * muscicVolume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }

    //Semplice funzione di riproduzione
    //ricerca il suono negli array preparati in precedenza
    //e riproduce il suono ricercato
    public void Play(string clipName, SoundType soundType)
    {
        if(soundType == SoundType.Music)
        {
            Sound music = Array.Find(musicPool, sound => sound.name == clipName);
            music.source.Play();
            return;
        }

        Sound sfx = Array.Find(sfxPool, sound => sound.name == clipName);
        sfx.source.Play();
    }

    #region FUNZIONI REGOLAZIONE VOLUME
    
    public void SetUpMasterVolume(float newMasterVolume)
    {
        //In questo caso il nuovo volume viene moltiplicato per i volumi singoli.
        //In questo modo dovrebbero essere ridotti della stessa quantità entrambi i volumi.
        muscicVolume = muscicVolume * newMasterVolume;
        sfxVolume = sfxVolume *newMasterVolume;
    }

    public void SetUpMusicVolume(float newVolume)
    {
        muscicVolume = newVolume;
    }

    public void SetUpSFXVolume(float newVolume)
    {
        sfxVolume = newVolume;
    }
    
    #endregion

}
