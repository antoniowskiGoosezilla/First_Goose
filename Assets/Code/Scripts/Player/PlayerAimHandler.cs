using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimHandler : MonoBehaviour
{
    private Vector3 mouseWorldPosition;
    private Plane aimPlane = new Plane(Vector3.down, 0);

    //Una variabile per far eseguire l'aggionamento della posizione del mouse solo quando avviene
    //un effettivo cambiamento della posizione su schermo
    private Vector2 oldMousePosition;

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
        //(DA SOSTITUIRE CON IL CLICK DEL MOUSE)
        transform.forward = new Vector3(mouseWorldPosition.x - transform.position.x, 0, mouseWorldPosition.z - transform.position.z);
    }
}
