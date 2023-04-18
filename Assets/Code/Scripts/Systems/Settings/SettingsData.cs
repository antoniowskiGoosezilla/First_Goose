using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AntoNamespace
{

    //Valutare la possibilità di rendere il settings manager statico
    [System.Serializable]
    public class SettingsData
    {
        //AUDIO SETTINGS
        public float masterVolume;
        public float sfxVolume;
        public float musicVolume;
        public bool mute;
        //Aggiungere tipo audio? cuffie, Casse, ecc...


        //VIDEO SETTINGS
        //Valutare l'idea di sostituire la stringa con un id
        public string resolution;
        //La qualità da bassa ad alta è gestita con un valore numerico da 0 a 3
        public int quality;

        //INPUT SETTINGS
        //Aggiungere gestione comandi
    }
}

