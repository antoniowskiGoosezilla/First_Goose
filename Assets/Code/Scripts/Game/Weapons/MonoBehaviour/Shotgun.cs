using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AntoNamespace;
using Cinemachine;

public class Shotgun : Weapon
{
    public override void Shoot()
    {
        //CLASSICI CHECK PER LE ARMI
        if(mainShotSound != null)
            PlaySound(mainShotSound);
        
        if(shotEffect != null)
        {
            GameObject effect = Instantiate(shotEffect, muzzle.position, Quaternion.identity);
            effect.transform.forward = -muzzle.forward;
            //effect.Emit(1);
        }
        
        magAmmo -= 1;
        bulletHandler.CreateShotgunBullet(muzzle, range, layerMaskToCheck);

        //Camera Shake
        CinemachineCore.Instance.GetActiveBrain(0).ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CameraShake>().Shake(2f, .2f);

        StartCoroutine(StartShootingCooldown());   //Necessaria per il rateo di fuoco.
    }

    public override void AlternativeShoot()
    {
        throw new System.NotImplementedException();
    }

    public override void Equip()
    {
        throw new System.NotImplementedException();
    }
}
