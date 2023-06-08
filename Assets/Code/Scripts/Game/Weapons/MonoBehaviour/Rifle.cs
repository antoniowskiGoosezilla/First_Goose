using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AntoNamespace;

public class Rifle : Weapon, IHoldable
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
        Vector3 velocity = -muzzle.forward * bulletSpeed;
        BulletHandler.Bullet bullet = bulletHandler.CreateBullet(muzzle.position, velocity, bulletDrop, maxBulletLifeTime, trailShotEffect, hitEffect, layerMaskToCheck);

        StartCoroutine(StartShootingCooldown());   //Necessaria per il rateo di fuoco.
    }

    public override void AlternativeShoot()
    {

    }

    public override void Equip()
    {
        throw new System.NotImplementedException();
    }

    public void OnHold()
    {
        StartCoroutine(PreparingHoldShot());
    }


    public void OnReleaseHold()
    {

    }

    private IEnumerator PreparingHoldShot()
    {
        float shotPrecision = 0;
        while(InputCustomSystem.holdingShoot)
        {
            shotPrecision += 0.1f;
                
            //Ruotiamo il modello nella direzione dello sparo se usiamo mouse + tastiera
            if(InputCustomSystem.controllerType == InputCustomSystem.ControllerType.KEYBOARD)
                //transform.forward = new Vector3(mouseWorldPosition.x - transform.position.x, 0, mouseWorldPosition.z - transform.position.z);
            
            yield return null;
        }

        Shoot();
        //OnUpdateWeaponAmmo?.Invoke(magAmmo, totalAmmo);
        
    }
}
