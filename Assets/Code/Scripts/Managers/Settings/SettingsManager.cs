using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

namespace AntoNamespace
{
    public class SettingsManager : MonoBehaviour
    {

        //Instanza del singleton
        public static SettingsManager instance;

        //EVENTI
        public static event Action OnSettingsChange;

        public static event Action<float> OnMasterVolumeSettingsChange;
        public static event Action<float> OnSFXVolumeSettingsChange;
        public static event Action<float> OnMusicVolumeSettingsChange;

        public static event Action<LocalizationSystem.Language> OnLanguageChange;
        
        //Path di salvataggio file
        static string path
        {
            get
            {
                return Application.persistentDataPath + "/settings.mamt";
            }
        }

        //Valori di impostazione che vengono usate al momento e quelli di default per il ripristino
        SettingsData currentSettings;
        SettingsData defaultSettings;


        //Tutti le impostazioni modificabili nel menù
        //Da mettere PRIVATE quando verrà inserito una UI

        [Header("AUDIO SETTINGS")]
        [Space]
        
        [Range(0,1)]
        public float masterVolume;
        
        [Range(0,1)]
        public float musicVolume;
        
        [Range(0,1)]
        public float sfxVolume;

        public bool mute;

        [Header("VIDEO SETTINGS")]
        public Resolution[] resolution;


        public LocalizationSystem.Language language = LocalizationSystem.Language.English;




        void Awake()
        {
            if(instance != null)
            {
                Destroy(gameObject);
            }

            instance = this;
            DontDestroyOnLoad(gameObject);

            //Controlliamo se bisogna caricare un file di impostazioni
            //Il file viene aggiornato ogni volta che qualcosa viene cambiato
            //nelle impostazioni regolate dal setting manager
            if(currentSettings == null)
            {
                if(!File.Exists(path))
                {
                    //Se non esiste ancora il file, allora lo creaiamo
                    //e lo aggiungiamo al path scelto
                    CreateSettingsConfiguration();
                }

                //Legge il file delle impostazioni e le applica 
                GetSettingsConfiguration();

                //Inizializziamo il sistema di localizzazione.
                //Vengono creati i Dizionari con le stringhe tradotte ed impostata la lingua
                LocalizationSystem.Init();

                //Aggiungo la funzione di update all'evento.
                //Ogni volta che l'evento viene triggerato, anche la funzione di update
                //verrà chiamata
                OnSettingsChange += UpdateSettingConfiguration;
            }

        }


        #region SETTINGS SAVE FUNCTIONS

        void CreateSettingsConfiguration()
        {
            //Nuovi settings
            SettingsData newSettings = new SettingsData();

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Create);

            //lo rendiamo binario
            formatter.Serialize(stream, newSettings);

            stream.Close();
        }

        void UpdateSettingConfiguration()
        {

        }

        void GetSettingsConfiguration()
        {
            
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            //lo rendiamo non binario
            currentSettings = formatter.Deserialize(stream) as SettingsData;

            stream.Close();

            //Applichiamo i nostri settings
            ApplySettingsConfiguration();
        }

        void ApplySettingsConfiguration()
        {
            this.masterVolume = currentSettings.masterVolume;
            this.sfxVolume = currentSettings.sfxVolume;
            this.musicVolume = currentSettings.musicVolume;
            this.mute = currentSettings.mute;
            this.language = LocalizationSystem.Language.English; //System.Enum.Parse<LocalizationSystem.Language>(currentSettings.language); //Da testare con una build
        }
    
        #endregion

        #region SET FUNCTIONS
        public void SetMasterVolume(float newVolume)
        {
            if(newVolume > 1)
                newVolume = 1;

            if(newVolume < 0)
                newVolume = 0;

            masterVolume = newVolume;

            //Invochiamo l'evento in moodo che tutti imoduli interessati da cambiamenti nelle
            //impostazioni modifichino di conseguenza se stessi
            OnMasterVolumeSettingsChange?.Invoke(newVolume);
        }
        public void SetMusicVolume(float newVolume)
        {
            if(newVolume > 1)
                newVolume = 1;

            if(newVolume < 0)
                newVolume = 0;

            musicVolume = newVolume;

            //Invochiamo l'evento in moodo che tutti imoduli interessati da cambiamenti nelle
            //impostazioni modifichino di conseguenza se stessi
            OnMusicVolumeSettingsChange?.Invoke(newVolume);
        }
        public void SetSFXVolume(float newVolume)
        {
            if(newVolume > 1)
                newVolume = 1;

            if(newVolume < 0)
                newVolume = 0;

            sfxVolume = newVolume;

            //Invochiamo l'evento in moodo che tutti imoduli interessati da cambiamenti nelle
            //impostazioni modifichino di conseguenza se stessi
            OnSFXVolumeSettingsChange?.Invoke(newVolume);
        }
        public void SetLanguage(LocalizationSystem.Language newLanguage)
        {
            language = newLanguage;

            OnLanguageChange?.Invoke(newLanguage);
        }

        #endregion
    }


}