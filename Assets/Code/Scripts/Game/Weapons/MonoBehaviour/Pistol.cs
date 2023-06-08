using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    public override void Shoot()
    {
        if(mainShotSound != null)
            PlaySound(mainShotSound);
        
        if(shotEffect != null)
            shotEffect.Play();
        
        magAmmo -= 1;                                                         //Togliamo un colpo al caricatore

        //In questo metodo creaiamo un proiettile per il bullethandler di riferimento.
        //L'handler si occupa della gestione di tutti i singoli colpi sparati dall'entita'
        Vector3 velocity = -muzzle.forward* bulletSpeed;
        BulletHandler.Bullet bullet = bulletHandler.CreateBullet(muzzle.position, velocity,bulletDrop, maxBulletLifeTime, trailShotEffect, hitEffect, layerMaskToCheck);

        StartCoroutine(StartShootingCooldown());   //Necessaria per il rateo di fuoco.
    }

    public override void AlternativeShoot()
    {

    }

    public override void Equip()
    {
        throw new System.NotImplementedException();
    }



    //Standard 
    private void Awake()
    {
        //TODO: cambiare in caso di arma già presente
        //nell'invetario
        if(weaponTemplate != null && firstInit) 
        {
            firstInit = false;
            WeaponFirstInit();
        };
    }
    
}
