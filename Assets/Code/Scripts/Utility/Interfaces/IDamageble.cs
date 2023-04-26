using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageble
{
    void GetDamage(float damage);
    void GetDamage(float damage, Weapon.WeaponType type);
}
