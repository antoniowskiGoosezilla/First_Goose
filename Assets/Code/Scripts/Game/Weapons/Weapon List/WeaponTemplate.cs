using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName ="Weapons")]
public class WeaponTemplate : ScriptableObject
{

    public string weaponName;
    public string weaponBrand;

    //STATS
    [Header("Statistiche")]
    public float damage;
    public float range;
    [Range(0,1)]
    public float precision;
    public float reloadTime;
    public float bulletSpeed;
    public float bulletDrop;
    public float maxBulletLifeTime;

    
    public float shotCooldown;              //Serve per definire il rateo di fuoco - firingRate
    public int mainShotCost = 1;
    public int alternativeShotCost;

    [Header("Munizioni")]
    public int maxMagAmmo;
    public int maxTotalAmmo;

    public Weapon.WeaponCategory category;
    public Weapon.WeaponType type;
    public int rarity;

    [Header("Suoni")]
    public AudioClip mainShotSound;
    public AudioClip alternativeShotSound;
    public AudioClip hitSound;
    [Header("VFX")]
    public TrailRenderer trailShotEffect;
    public ParticleSystem hitEffect;

    
    [Header("Prefab")]
    public GameObject prefab;

}
