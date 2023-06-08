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





        private void Awake()
        {
            playerInventoryHandler = GetComponent<PlayerInventoryHandler>();
            playerStatsHandler = GetComponent<PlayerStatsHandler>();
            comboHandler = GetComponent<ComboHandler>();

            InputCustomSystem.OnShootAction += Shoot;
            InputCustomSystem.OnHoldShootAction += HoldShoot;
        }

        private void Update()
        {
            UpdateMouseWorldPosition();      
        }

        private void UpdateMouseWorldPosition()
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
        private void Shoot(InputAction.CallbackContext context)
        {
            Weapon usedWeapon = playerInventoryHandler.equippedWeapon.GetComponent<Weapon>();

            if(CheckPreShot(usedWeapon) == false)
                return;
            //Queste armi usano un altro tipo di funzione di sparo
            if(usedWeapon.category == Weapon.WeaponCategory.RIFLE || usedWeapon.category == Weapon.WeaponCategory.CHARGER || usedWeapon.category == Weapon.WeaponCategory.LASER)
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

            //Usiamo l'arma per sparare
            usedWeapon.Shoot();   //La gestione dei punti è stata spostata nel bullet
            
            OnUpdateWeaponAmmo?.Invoke(usedWeapon.magAmmo, usedWeapon.totalAmmo);
            //playerStatsHandler.SetAvailableActionStacks(playerStatsHandler.availableActionStacks - usedWeapon.mainShotCost); DEPRECATED
            playerStatsHandler.RemoveActionStacks(usedWeapon.mainShotCost);
        }
        
        private void HoldShoot(InputAction.CallbackContext context)
        {
            Weapon usedWeapon = playerInventoryHandler.equippedWeapon.GetComponent<Weapon>();

            if(CheckPreShot(usedWeapon) == false)
                return;

            //Queste armi usano un altro tipo di funzione di sparo
            if(usedWeapon.category == Weapon.WeaponCategory.PISTOL || usedWeapon.category == Weapon.WeaponCategory.SHOTGUN || usedWeapon.category == Weapon.WeaponCategory.LAUNCHER)
                return;

            //Hard code ---- CERCARE MODO DI SPECIFICARE TRAMITE INTERFACCIA
            if(usedWeapon.category == Weapon.WeaponCategory.RIFLE)
            {
                Rifle rifle = playerInventoryHandler.equippedWeapon.GetComponent<Rifle>();
                rifle.OnHold();
                Debug.Log("Call");
            }
            StartCoroutine(RotateModel());
            playerStatsHandler.RemoveActionStacks(usedWeapon.mainShotCost);
            OnUpdateWeaponAmmo?.Invoke(usedWeapon.magAmmo, usedWeapon.totalAmmo);
                
        }

        private bool CheckPreShot(Weapon usedWeapon)
        {

            if(usedWeapon == null)
                return false;

            if(playerStatsHandler.availableActionStacks <= 0)
                return false;
            
            if(usedWeapon.mainShotCost > playerStatsHandler.availableActionStacks)
                return false;
                
            if(usedWeapon.inCooldown)
                return false;

            if(usedWeapon.magAmmo == 0)
            {
                //Aggiungere suono caricatore scarico
                Debug.Log("Caricatore scarico");
                return false;
            }

            return true;
        }
    
        private IEnumerator RotateModel()
        {
            while(InputCustomSystem.holdingShoot)
            {
                transform.forward = new Vector3(mouseWorldPosition.x - transform.position.x, 0, mouseWorldPosition.z - transform.position.z);
                yield return null;
            }
        }
    }   
}