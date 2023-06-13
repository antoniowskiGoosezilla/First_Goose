using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Pistol : Weapon
{
    public override void Shoot()
    {
        if(mainShotSound != null)
            PlaySound(mainShotSound);
        
        if(shotEffect != null)
        {
            Debug.Log("Not Null");
            ParticleSystem effect = Instantiate(shotEffect, muzzle.position, Quaternion.identity);
            effect.transform.forward = muzzle.forward;
            effect.Emit(1);
        }
        
        magAmmo -= 1;                                                         //Togliamo un colpo al caricatore

        //In questo metodo creaiamo un proiettile per il bullethandler di riferimento.
        //L'handler si occupa della gestione di tutti i singoli colpi sparati dall'entita'
        Vector3 velocity = -muzzle.forward* bulletSpeed;
        BulletHandler.Bullet bullet = bulletHandler.CreateBullet(muzzle.position, velocity,bulletDrop, maxBulletLifeTime, trailShotEffect, hitEffect, layerMaskToCheck);
        
        //Camera Shake
        CinemachineCore.Instance.GetActiveBrain(0).ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CameraShake>().Shake(1f, .2f);
        


        StartCoroutine(StartShootingCooldown());   //Necessaria per il rateo di fuoco.
    }

    public override void AlternativeShoot()
    {

    }

    public override void Equip()
    {
        throw new System.NotImplementedException();
    }
    
}
