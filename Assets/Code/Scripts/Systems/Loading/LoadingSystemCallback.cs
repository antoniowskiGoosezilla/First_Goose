using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Si tratta di uno script da assegnare solo alla scena di caricamento.
//Si occupa di iniziare il caricamento asincrono a partire dal primo frame
//dell'update
public class LoadingSystemCallback : MonoBehaviour
{
    private bool firstUpdate = true;

    
    void Update()
    {
        //Per far azionare la callback solo una volta dopo il primo frama di Update
        if(firstUpdate)
        {
            LoadingSystem.LoadingCallback();
            firstUpdate = false;
        }
    }
}
