using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    public override bool Shoot()
    {
        if(mainShotSound != null)
            PlaySound(mainShotSound);
        
        if(shotEffect != null)
            shotEffect.Play();
        
        magAmmo -= 1;                                                         //Togliamo un colpo al caricatore

        
        Vector3 velocity = -transform.forward * bulletSpeed;
        Bullet bullet = CreateBullet(muzzle.position, velocity);
        firedBullets.Add(bullet);
        //TODO: Rimuovere la gestione dei bullete dall'arma. Metterla sui singoli personaggi o in un'entita' globale
        //TODO: RIMUOVERE VECCHIA FUNZIONE DI SHOOT
        
        //Generiamo un raggio per vedere se il proiettile colpisce il nemico
        /*if(muzzle == null) muzzle = transform;
        Ray shot = new Ray(muzzle.position, -transform.forward); 
        RaycastHit hit;
        
        bool isHit = Physics.Raycast(shot, out hit, range, layerMaskToCheck); //Sostituire lo 0 con il layermask su cui fare il controllo
        TrailRenderer trail = Instantiate(trailShotEffect, transform.position, Quaternion.identity);
        trail.AddPosition(shot.origin);

        //Se colpisce
        if(isHit)
        {
            try
            {
                ParticleSystem effect = Instantiate(hitEffect, hit.point, Quaternion.identity);
                effect.transform.forward = hit.normal;
                effect.Emit(1);
                trail.transform.position = hit.point;
            }
            catch
            {
                Debug.LogError("Effetto Hit o Trail mancante");
            }

            Debug.Log("Colpito");
        }
        else
        {
            trail.transform.position = muzzle.position - muzzle.forward * range; //Orribile, ma per ora fa il suo lavoro
        }
        return isHit;
        */
        StartCoroutine(StartShootingCooldown());
        return false;
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


    private void Awake()
    {
        //TODO: cambiare in caso di arma già presente
        //nell'invetario
        if(weaponTemplate != null) WeaponFirstInit();
    }
    
}
