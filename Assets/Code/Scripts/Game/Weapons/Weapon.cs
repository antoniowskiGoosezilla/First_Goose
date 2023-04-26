using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public enum WeaponType
    {
        STANDARD,
        ENERGY,
        MAGIC_FIRE,
        MAGIC_SHOCK,
        MAGIC_ICE
    }
    //STATS
    public float damage;
    public float range;
    public float precision;
    public float shotCooldown;              //Serve per definire il rateo di fuoco
    public float magAmmo;
    public float totalAmmo;
    public WeaponType type;
    public int rarity;

    public abstract void Shoot();
    public abstract void AlternativeShoot();

    public void TickFireDamage()
    {

    }

    public void TickIceDamage()
    {

    }

    public void TickShockDamage()
    {
        
    }
}
