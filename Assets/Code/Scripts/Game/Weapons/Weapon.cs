using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public float damage;
    public float range;
    public float precision;
    public float shotCooldown;              //Serve per definire il rateo di fuoco

    public abstract void Shoot();
    public abstract void AlternativeShoot();
}
