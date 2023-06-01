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

    public Weapon.WeaponCategory category;
    
    public float shotCooldown;              //Serve per definire il rateo di fuoco
    public int mainShotCost = 1;
    public int alternativeShotCost;

    [Header("Munizioni")]
    public float maxMagAmmo;
    public float maxTotalAmmo;

    public Weapon.WeaponType type;
    public int rarity;

    [Header("Suoni")]
    public AudioClip mainShotSound;
    public AudioClip alternativeShotSound;
    
    [Header("Prefab")]
    [SerializeField] GameObject prefab;

}
