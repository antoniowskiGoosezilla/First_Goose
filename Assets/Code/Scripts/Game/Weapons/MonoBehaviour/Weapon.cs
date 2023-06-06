using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

    public enum WeaponCategory
    {
        PISTOL,
        RIFLE,
        SHOTGUN,
        LASER,
        LAUNCHER,
        CHARGER
    }
    
    [SerializeField] protected WeaponTemplate weaponTemplate;

    public string weaponName;
    public string weaponBrand;

    //STATS
    [Header("Stats")]
    public float damage;
    public float range;
    [Range(0,1)]
    public float precision;
    public float reloadTime;
    public float bulletSpeed;
    public float bulletDrop;
    public float maxBulletLifeTime;

    public float shotCooldown;              //Serve per definire il rateo di fuoco
    public bool inCooldown;
    public int mainShotCost = 1;
    public int alternativeShotCost;

    [Header("Ammo")]
    public float magAmmo;
    public float maxMagAmmo;
    public float totalAmmo;
    public float maxTotalAmmo;

    public WeaponType type;
    public int rarity;

    [Header("Suoni")]
    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected AudioClip mainShotSound;
    [SerializeField] protected AudioClip alternativeShotSound;
    [SerializeField] protected AudioClip hitSound;
    [Header("VFX")]
    [SerializeField] protected ParticleSystem shotEffect;
    [SerializeField] protected TrailRenderer trailShotEffect;
    [SerializeField] protected ParticleSystem hitEffect;
    [Space]
    [Header("Utility")]
    [SerializeField] protected Transform muzzle;

    public abstract bool Shoot();
    public abstract void AlternativeShoot();
    public abstract void Equip();
    
    public IEnumerator Reload()
    {   
        yield return new WaitForSeconds(reloadTime);

        if(totalAmmo >= maxMagAmmo)
        {
            magAmmo = maxMagAmmo;
            totalAmmo -= maxMagAmmo;
        }

        else
        {
            magAmmo = totalAmmo;
            totalAmmo = 0;
        }

        //Aggiungere suono
        Debug.Log("Caricato");
    }

    public void StandardReload()
    {

        if(totalAmmo >= maxMagAmmo)
        {
            totalAmmo -= maxMagAmmo - magAmmo;
            magAmmo = maxMagAmmo;
        }

        else
        {
            magAmmo = totalAmmo;
            totalAmmo = 0;
        }
        Debug.Log("Caricato");
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
        float pitch = UnityEngine.Random.Range(0.9f, 1.3f);
        try
        {
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(clip);
        }
        catch
        {
            Debug.LogWarning("AudioSource mancante. Suono riprodotto con PlayClipAtPoint.");
            AudioSource.PlayClipAtPoint(clip, transform.position, 1f);
        }

        
    }

    public void WeaponFirstInit()
    {
        weaponName = weaponTemplate.weaponName;
        weaponBrand = weaponTemplate.weaponBrand;
        damage = weaponTemplate.damage;
        range = weaponTemplate.range;
        precision = weaponTemplate.precision;
        reloadTime = weaponTemplate.reloadTime;
        bulletSpeed = weaponTemplate.bulletSpeed;
        bulletDrop = weaponTemplate.bulletDrop;
        maxBulletLifeTime = weaponTemplate.maxBulletLifeTime;
        shotCooldown = weaponTemplate.shotCooldown;
        inCooldown = false;
        mainShotCost = weaponTemplate.mainShotCost;
        alternativeShotCost = weaponTemplate.alternativeShotCost;
        magAmmo = weaponTemplate.maxMagAmmo;
        maxMagAmmo = weaponTemplate.maxMagAmmo;
        totalAmmo = weaponTemplate.maxTotalAmmo;
        maxTotalAmmo = weaponTemplate.maxTotalAmmo;
        type = weaponTemplate.type;
        rarity = weaponTemplate.rarity;

        mainShotSound = weaponTemplate.mainShotSound;
        alternativeShotSound = weaponTemplate.alternativeShotSound;
        hitSound = weaponTemplate.hitSound;

        trailShotEffect = weaponTemplate.trailShotEffect;
        hitEffect = weaponTemplate.hitEffect;
    }

    public void SetBulletHandler(BulletHandler bulletHandler)
    {
        this.bulletHandler = bulletHandler;
    }

    //PRIVATE
    protected static LayerMask layerMaskToCheck = 0x64;
   
    protected BulletHandler bulletHandler;
    


}
