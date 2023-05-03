using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour, IEquippable
{
    public enum WeaponType
    {
        STANDARD,
        ENERGY,
        MAGIC_FIRE,
        MAGIC_SHOCK,
        MAGIC_ICE
    }

    public string weaponName;
    public string weaponBrand;

    //STATS
    public float damage;
    public float range;
    [Range(0,1)]
    public float precision;
    public float reloadTime;
    
    public float shotCooldown;              //Serve per definire il rateo di fuoco
    public bool inCooldown;
    public int mainShotCost = 1;
    public int alternativeShotCost;

    public float magAmmo;
    public float maxMagAmmo;
    public float totalAmmo;
    public float maxTotalAmmo;

    public WeaponType type;
    public int rarity;

    [Header("Suoni")]
    [SerializeField] protected AudioClip mainShotSound;
    [SerializeField] protected AudioClip alternativeShotSound;
    
    protected static LayerMask layerMaskToCheck = 0x6;

    public abstract void Shoot();
    public abstract void AlternativeShoot();
    public abstract void Equip();
    public IEnumerator Reload()
    {
        if(magAmmo == maxMagAmmo)                       //Non deve caricare se il caricatore è pieno
            yield break;

        if(totalAmmo <= 0)                              //Non hai altre munizioni a disposizione
            yield break;
        
        yield return new WaitForSeconds(reloadTime);

        if(totalAmmo >= maxMagAmmo)
            magAmmo = maxMagAmmo;
        else
            magAmmo = totalAmmo;
        //Aggiungere suono
    }

    public void TickFireDamage()
    {

    }

    public void TickIceDamage()
    {

    }

    public void TickShockDamage()
    {
        
    }

    public void PlaySound(AudioClip clip)
    {
        float pitch = Random.Range(0.5f, 2f);
        AudioSource.PlayClipAtPoint(clip, transform.position, 1f);
    }
}
