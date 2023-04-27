using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    public override void Shoot()
    {
        Debug.Log("Pistol Shoot");
    }

    public override void AlternativeShoot()
    {

    }

    public override void Equip()
    {
        throw new System.NotImplementedException();
    }
}
