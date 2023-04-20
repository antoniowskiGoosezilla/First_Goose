using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AntoNamespace
{
    public static class LocalizationSystem
    {
        public enum Language
            {
                Italian,
                English,
                French
            }

            //la ligue viene sincronizzata dal SettingsManager
            public static Language language;

            private static Dictionary<string,string> englishLocalization;
            private static Dictionary<string,string> italianLocalization;
            private static Dictionary<string,string> frenchLocalization;


            public static bool isInit;

            public static void Init()
            {
                CSVLoader csvLoader = new CSVLoader();
                csvLoader.LoadCSV();

                englishLocalization = csvLoader.GetDictonary("en");
                italianLocalization = csvLoader.GetDictonary("it");
                frenchLocalization = csvLoader.GetDictonary("fr");

                //Aggiungiamo il cambio di lingua all'evento.
                //La lingua viene specifcata quando l'evento viene chiamato
                SettingsManager.OnLanguageChange += SetNewLanguage;

            }

            public static string GetLocalizedValue(string key)
            {
                if(!isInit)
                    Init();

                string value = key;

                switch(language)
                {
                    case Language.English:
                        englishLocalization.TryGetValue(key, out value);
                        break;
                    case Language.Italian:
                        italianLocalization.TryGetValue(key, out value);
                        break;
                    case Language.French:
                        frenchLocalization.TryGetValue(key, out value);
                        break;
                }

                return value;
            }

            public static void SetNewLanguage(Language newLanguage)
            {
                language = newLanguage;
            }
    }
}
