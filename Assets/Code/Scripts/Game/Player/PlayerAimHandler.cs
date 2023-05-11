using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AntoNamespace
{    
    [RequireComponent(typeof(ComboHandler))]
    [RequireComponent(typeof(PlayerInventoryHandler))]
    [RequireComponent(typeof(PlayerStatsHandler))]
    //Classe che gestisce sia la mira che lo shooting del giocatore
    public class PlayerAimHandler : MonoBehaviour
    {
        //PUBLIC O EDITOR
    
        [SerializeField] LayerMask layerToCheck;

        //EVENTI
        public static event Action<float, float> OnUpdateWeaponAmmo;


        //PRIVATE 
        private PlayerInventoryHandler playerInventoryHandler;
        private PlayerStatsHandler playerStatsHandler;
        private ComboHandler comboHandler;
        
        private Vector3 mouseWorldPosition;
        private Plane aimPlane = new Plane(Vector3.down, 0);
        private Vector2 oldMousePosition;                       //Una variabile per far eseguire l'aggionamento della posizione del mouse solo quando avviene
                                                                //un effettivo cambiamento della posizione su schermo





        void Awake()
        {
            playerInventoryHandler = GetComponent<PlayerInventoryHandler>();
            playerStatsHandler = GetComponent<PlayerStatsHandler>();
            comboHandler = GetComponent<ComboHandler>();

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
            //transform.forward = new Vector3(mouseWorldPosition.x - transform.position.x, 0, mouseWorldPosition.z - transform.position.z);
        }


        //Funzioni per lo sparo
        //Considerare la possibilità di spostare queste funzioni nella classe delle armi
        void Shoot(InputAction.CallbackContext context)
        {
            Weapon usedWeapon = playerInventoryHandler.equippedWeapon.GetComponent<Weapon>();
            if(usedWeapon == null)
                return;
            
            if(playerStatsHandler.availableActionStacks <= 0)
                return;
            
            if(usedWeapon.mainShotCost > playerStatsHandler.availableActionStacks)
                return;
                
            if(usedWeapon.inCooldown)
                return;

            //Ruotiamo il modello nella direzione dello sparo se usiamo mouse + tastiera
            if(InputCustomSystem.controllerType == InputCustomSystem.ControllerType.KEYBOARD)
                transform.forward = new Vector3(mouseWorldPosition.x - transform.position.x, 0, mouseWorldPosition.z - transform.position.z);

            
            if(usedWeapon.magAmmo == 0)
            {
                //Aggiungere suono caricatore scarico
                Debug.Log("Caricatore scarico");
                return;
            }

            
            bool shotResult = usedWeapon.Shoot();
            if(shotResult)                  //Se colpiamo un avversario, aggiungiamo i punti;
                comboHandler.AddPoints(100);
            
            OnUpdateWeaponAmmo?.Invoke(usedWeapon.magAmmo, usedWeapon.totalAmmo);
            //playerStatsHandler.SetAvailableActionStacks(playerStatsHandler.availableActionStacks - usedWeapon.mainShotCost); DEPRECATED
            playerStatsHandler.RemoveActionStacks(usedWeapon.mainShotCost);
        }
    }
}