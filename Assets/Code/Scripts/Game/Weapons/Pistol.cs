using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    public override void Shoot()
    {
        Debug.Log("Pistol Shoot");
        
        //Generiamo un raggio per vedere se il proiettile colpisce il nemico
        Ray shot = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        bool isHit = Physics.Raycast(shot, out hit, range, layerMaskToCheck); //Sostituire lo 0 con il layermask su cui fare il controllo
        
        //Se colpisce
        if(isHit)
        {
            Debug.Log("Colpito");
        }
        StartCoroutine(StartShootingCooldown());
    }

    public override void AlternativeShoot()
    {

    }

    private IEnumerator StartShootingCooldown()
    {
        inCooldown = true;
        yield return new WaitForSeconds(shotCooldown);
        inCooldown = false;
    }

    public override void Equip()
    {
        throw new System.NotImplementedException();
    }
}