using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName ="Weapons")]
public class WeaponTemplate : ScriptableObject
{

    public string weaponName;
    public string weaponBrand;

    //STATS
    [Header("Stats")]
    public float damage;
    public float range;
    [Range(0,1)]
    public float precision;
    public float reloadTime;
    
    public float shotCooldown;              //Serve per definire il rateo di fuoco
    public int mainShotCost = 1;
    public int alternativeShotCost;

    [Header("Ammo")]
    public float magAmmo;
    public float maxMagAmmo;
    public float totalAmmo;
    public float maxTotalAmmo;

    public Weapon.WeaponType type;
    public int rarity;

    [Header("Suoni")]
    [SerializeField] protected AudioClip mainShotSound;
    [SerializeField] protected AudioClip alternativeShotSound;
    
    [Header("Prefab")]
    [SerializeField] GameObject prefab;
    void Start()
    {
        
    }

}
