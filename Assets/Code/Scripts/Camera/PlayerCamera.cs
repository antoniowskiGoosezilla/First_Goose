using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AntoNamespace
{

    //Questo scripe serve ad implementare il movimento della camera di gioco
    //in modo che segui sempre il giocatore. 
    //L'angolazione può essere impostata tramite le due variabili per gli angoli
    public class PlayerCamera : MonoBehaviour
    {

        [SerializeField]
        GameObject player; //E' il giocatore da seguire


        //Angoli standard per una visuale isometrica a 45 gradi
        [SerializeField]
        float fi = 45;
        [SerializeField]
        float theta = 225;
        [SerializeField]
        float distance = 10;

        void Awake()
        {
            if(player == null)
                player = GameObject.FindGameObjectWithTag("Player");
            
            //OPZIONALE
            //Component.FindObjectOfType<Camera>().orthographicSize = distance;

            //Impostiamo la camera usando le coordinate sferiche
            SetAndHoldCameraPosition();
            //Ruotiamo di 45 gradi
            transform.rotation = Quaternion.Euler(new Vector3(fi, 45, 0));
        }

        void Start()
        {
            
        }

        void Update()
        {
            //Aggiorniamo costantemente la posizione della camera
            SetAndHoldCameraPosition();
        }

        //La funzione permette di calolare ed impostare la posizione della camera
        //in funzione della distanza scelta e degli angoli selezionati.
        void SetAndHoldCameraPosition(){
            float xValue = player.transform.position.x + distance*Mathf.Cos(fi*Mathf.PI/180)*Mathf.Cos(theta*Mathf.PI/180);
            float zValue = player.transform.position.z + distance*Mathf.Sin(theta*Mathf.PI/180)*Mathf.Cos(fi*Mathf.PI/180);
            float yValue = player.transform.position.y + distance*Mathf.Sin(fi*Mathf.PI/180);
            transform.position = new Vector3(xValue, yValue, zValue);
        }
    }

}
