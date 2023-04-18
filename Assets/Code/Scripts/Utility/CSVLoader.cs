using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text.RegularExpressions;

[SerializeField]
public class CSVLoader
{
    //File di riferimento
    private TextAsset csvFile;
    private char lineSeparator = '\n';
    private char surround = '"';
    private string[] fieldSeparator = {"\",\""};

    public void LoadCSV()
    {
        csvFile = Resources.Load<TextAsset>("localization");
    }

    //Il languageId rappresente l'id a due catatteri usato per identificare
    //la lingua. Ad esempio: "it", che identifica l'italiano
    public Dictionary<string, string> GetDictonary(string languageId)
    {
        //Dizionario di appoggio
        Dictionary<string, string> dictonary = new Dictionary<string, string>();

        //Separiamo le righe. Ogni riga del csv rappresenta una frase o, in generale,
        //un elemento da tradurre.
        string[] lines = csvFile.text.Split(lineSeparator);

        int languageColumnIndex = -1;
        
        //Per header si intende la prima riga del csv con i "titoli" delle colonne.
        //Cerchiamo dunque la lingua che ci interessa.
        string[] header = lines[0].Split(fieldSeparator, System.StringSplitOptions.None);
        for(int i=0; i<header.Length;i++)
        {
            //Se troviamo la colonna relativa alla lingua interessata, segnamo
            //l'index della colonna relativa ed usciamo dal loop
            if(header[i].Contains(languageId))
            {
                languageColumnIndex = i;
                break;
            }
        }

        //Check di sicurezza. Se non abbiamo trovato la lingua esce dalla funzione
        if(languageColumnIndex == -1)
        {
            Debug.Log("Lingua non trovata o non supportata");
            return null;
        }

        //??????
        Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
        
        //Cicliamo tutte le righe, escluso l'header e inseriamo le key non presenti in un dizionario.
        //Prima di fare questo eliminiamo le " (virgolette) dalla frase.
        for(int i=1; i<lines.Length;i++)
        {
            string line = lines[i];
            
            //Otteniamo le singole colonne o campi
            string[] fields = CSVParser.Split(line);
            for(int x=0;x<fields.Length;x++)
            {
                fields[x] = fields[x].TrimStart(' ', surround);
                fields[x] = fields[x].TrimEnd(surround);
            }

            //Check per vedere se la traduzione della lingua si trova nel file
            if(fields.Length > languageColumnIndex)
            {
                string key = fields[0];

                //Check per vedere se il dizionario contiene già quella key.
                //Se è già presente nel dizionario, skippa.
                if(dictonary.ContainsKey(key))
                    continue;
                
                string value = fields[languageColumnIndex];
                dictonary.Add(key, value);
            }
        }

        return dictonary;
    }
}
