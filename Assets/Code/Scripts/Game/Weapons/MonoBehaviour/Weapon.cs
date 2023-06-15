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
    public int magAmmo;
    public int maxMagAmmo;
    public int totalAmmo;
    public int maxTotalAmmo;

    public WeaponCategory category;
    public WeaponType type;
    public int rarity;

    public bool firstInit = true;

    [Header("Suoni")]
    [SerializeField] protected AudioSource audioSource;
    protected AudioClip mainShotSound;
    protected AudioClip alternativeShotSound;
    protected AudioClip hitSound;
    public GameObject shotEffect;
    protected GameObject trailShotEffect;
    protected GameObject hitEffect;
    
    [Header("Utility")]
    [SerializeField] protected Transform muzzle;

    public abstract void Shoot();
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
        category = weaponTemplate.category;
        type = weaponTemplate.type;
        rarity = weaponTemplate.rarity;

        mainShotSound = weaponTemplate.mainShotSound;
        alternativeShotSound = weaponTemplate.alternativeShotSound;
        hitSound = weaponTemplate.hitSound;

        shotEffect = weaponTemplate.shotEffect;
        trailShotEffect = weaponTemplate.trailShotEffect;
        hitEffect = weaponTemplate.hitEffect;
    }

    public void WeaponInit(Weapon weaponDataToCopy)
    {
        weaponName = weaponDataToCopy.weaponName;
        weaponBrand = weaponDataToCopy.weaponBrand;
        damage = weaponDataToCopy.damage;
        range = weaponDataToCopy.range;
        precision = weaponDataToCopy.precision;
        reloadTime = weaponDataToCopy.reloadTime;
        bulletSpeed = weaponDataToCopy.bulletSpeed;
        bulletDrop = weaponDataToCopy.bulletDrop;
        maxBulletLifeTime = weaponDataToCopy.maxBulletLifeTime;
        shotCooldown = weaponDataToCopy.shotCooldown;
        inCooldown = false;
        mainShotCost = weaponDataToCopy.mainShotCost;
        alternativeShotCost = weaponDataToCopy.alternativeShotCost;
        magAmmo = weaponDataToCopy.maxMagAmmo;
        maxMagAmmo = weaponDataToCopy.maxMagAmmo;
        totalAmmo = weaponDataToCopy.maxTotalAmmo;
        maxTotalAmmo = weaponDataToCopy.maxTotalAmmo;
        type = weaponDataToCopy.type;
        rarity = weaponDataToCopy.rarity;

        mainShotSound = weaponDataToCopy.mainShotSound;
        alternativeShotSound = weaponDataToCopy.alternativeShotSound;
        hitSound = weaponDataToCopy.hitSound;

        shotEffect = weaponDataToCopy.shotEffect;
        trailShotEffect = weaponDataToCopy.trailShotEffect;
        hitEffect = weaponDataToCopy.hitEffect;
    }

    public void RestoreAmmo(Inventory.MagHolder ammos)
    {
        magAmmo = ammos.currentMagAmmo;
        totalAmmo = ammos.totalAmmo;
    }
    
    public void SetBulletHandler(BulletHandler bulletHandler)
    {
        this.bulletHandler = bulletHandler;
    }

    public WeaponTemplate GetWeaponTemplate()
    {
        return this.weaponTemplate;
    }
    
    
    
    
    
    //PRIVATE
    protected static LayerMask layerMaskToCheck = 0x64;
   
    protected BulletHandler bulletHandler;


    protected IEnumerator StartShootingCooldown()
    {
        inCooldown = true;
        yield return new WaitForSeconds(shotCooldown);
        inCooldown = false;
    }

    protected void Awake()
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
