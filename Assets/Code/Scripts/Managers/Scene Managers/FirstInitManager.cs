using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Si tratta di un manager utilizzato per effettuare l'inizializzazione dei vari sistemi
//(Settings, Saving, Localizzazione, ecc...).
//Va inserito nella prima scena utile

namespace AntoNamespace
{
    public class FirstInitManager : MonoBehaviour
    {
        void Awake()
        {
            SettingsSystem.Init();
            LocalizationSystem.Init();
            InputCustomSystem.Init();
        }
    }
}