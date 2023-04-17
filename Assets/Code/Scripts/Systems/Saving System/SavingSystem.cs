using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SavingSystem
{

    //Path in cui verranno salvati e caricati i file di salvataggio
    public static string path
    {
        get
        {
            return Application.persistentDataPath + "/save.mamt";
        }
    }

    public static void Save()
    {
        //Creaimao il formatter per la transformazione dei dati in binario
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        //trasformiamo il salvataggio in binario
        SaveData data = new SaveData();
        formatter.Serialize(stream, data);
        
        //chiudiamo lo stream di file
        stream.Close();
    }

    public static SaveData Load()
    {


        if(File.Exists(path))
        {
            //formatter e stream per i file
            BinaryFormatter formatter = new BinaryFormatter();

            //apriamo il file necessario
            FileStream stream = new FileStream(path, FileMode.Open);
            
            //lo decodifichiamo da binario alla nostra strtuttura dati necessaria
            SaveData data = formatter.Deserialize(stream) as SaveData;

            //Chiudiamo lo stream (SEMPRE)
            stream.Close();

            return data;
        }
        else
        {
            Debug.Log("File di salvataggio non trovato");
            return null;
        }
    }
}
