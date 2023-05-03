using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AntoNamespace
{    
    //Classe che gestisce sia la mira che lo shooting del giocatore
    public class PlayerAimHandler : MonoBehaviour
    {
        //PUBLIC O EDITOR
    
        [SerializeField] LayerMask layerToCheck;


        //PRIVATE 
        private PlayerInventoryHandler playerInventoryHandler;
        private PlayerStatsHandler playerStatsHandler;
        
        private Vector3 mouseWorldPosition;
        private Plane aimPlane = new Plane(Vector3.down, 0);
        private Vector2 oldMousePosition;                       //Una variabile per far eseguire l'aggionamento della posizione del mouse solo quando avviene
                                                                //un effettivo cambiamento della posizione su schermo





        void Awake()
        {
            playerInventoryHandler = GetComponent<PlayerInventoryHandler>();
            playerStatsHandler = GetComponent<PlayerStatsHandler>();

            InputCustomSystem.OnShootAction += Shoot;
        }

        void Update()
        {
            UpdateMouseWorldPosition();
        }

        void UpdateMouseWorldPosition()
        {
            if(oldMousePosition == AntoNamespace.InputCustomSystem.mouseScreenPosition)
                return;

            oldMousePosition = AntoNamespace.InputCustomSystem.mouseScreenPosition;
            
            Ray ray = Camera.main.ScreenPointToRay(AntoNamespace.InputCustomSystem.mouseScreenPosition);
            if(aimPlane.Raycast(ray, out float distance))
            {
                mouseWorldPosition = ray.GetPoint(distance);
            }

            //Fatti i calcoli, dobbiamo solo girare il giocatore verso il punto desiderato
        }


        //Funzioni per lo sparo
        //Considerare la possibilità di spostare queste funzioni nella classe delle armi
        void Shoot(InputAction.CallbackContext context)
        {

            if(playerStatsHandler.availableActionStacks <= 0)
                return;

            Weapon usedWeapon = playerInventoryHandler.equippedWeapon.GetComponent<Weapon>();
            if(usedWeapon.inCooldown)
                return;

            //Ruotiamo il modello nella direzione dello sparo
            transform.forward = new Vector3(mouseWorldPosition.x - transform.position.x, 0, mouseWorldPosition.z - transform.position.z);
            
            if(usedWeapon.magAmmo == 0)
            {
                //Aggiungere suono caricatore scarico
                Debug.Log("Caricatore scarico");
                return;
            }

            playerStatsHandler.SetAvailableActionStacks(playerStatsHandler.availableActionStacks - usedWeapon.mainShotCost);
            usedWeapon.Shoot();
        }
    }
}