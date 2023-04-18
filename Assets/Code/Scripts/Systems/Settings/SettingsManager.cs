using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace AntoNamespace
{
    public class SettingsManager : MonoBehaviour
    {

        //Instanza del singleton
        static SettingsManager instance;

        //TODO: Aggiungere dei delegate per la gestione di eventi
        
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


        #region SETTINGS
        [Header("AUDIO SETTINGS")]
        
        [Range(0,1)]
        public float masterVolume;
        
        [Range(0,1)]
        public float musicVolume;
        
        [Range(0,1)]
        public float sfxVolume;

        public bool mute;

        [Header("VIDEO SETTINGS")]
        Resolution[] resolution;
        
        #endregion




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
        }
    
        #endregion
    }


}