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
        private Vector3 mouseWorldPosition;
        private Plane aimPlane = new Plane(Vector3.down, 0);

        //Una variabile per far eseguire l'aggionamento della posizione del mouse solo quando avviene
        //un effettivo cambiamento della posizione su schermo
        private Vector2 oldMousePosition;


        //Sostituire con quello specifico delle armi
        private float shootingCooldown = 0.5f;
        private bool inCooldown;


        void Awake()
        {
            InputSystem.OnShootAction += Shoot;
        }

        void Update()
        {
            UpdateMouseWorldPosition();
        }

        void UpdateMouseWorldPosition()
        {
            if(oldMousePosition == AntoNamespace.InputSystem.mouseScreenPosition)
                return;

            oldMousePosition = AntoNamespace.InputSystem.mouseScreenPosition;
            
            Ray ray = Camera.main.ScreenPointToRay(AntoNamespace.InputSystem.mouseScreenPosition);
            if(aimPlane.Raycast(ray, out float distance))
            {
                mouseWorldPosition = ray.GetPoint(distance);
            }

            //Fatti i calcoli, giriamo il giocatore verso il punto desiderato
        }

        void Shoot(InputAction.CallbackContext context)
        {
            if(inCooldown)
                return;

            transform.forward = new Vector3(mouseWorldPosition.x - transform.position.x, 0, mouseWorldPosition.z - transform.position.z);
            Debug.DrawRay(transform.position, transform.forward, Color.red, 2);
            inCooldown = true;
            StartCoroutine(StartShootingCooldown());
        }

        private IEnumerator StartShootingCooldown()
        {
            yield return new WaitForSeconds(shootingCooldown);
            inCooldown = false;
        }
    }
}