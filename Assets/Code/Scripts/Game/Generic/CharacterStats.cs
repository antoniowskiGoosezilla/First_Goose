using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterStats : MonoBehaviour, IDamageble
{
    public float health;
    private float maxHealth;
    private float movementSpeed;
    private float attack;



    #region GENERIC FUNCTIONS

    public void SetMaxHealth(float newValue)
    {
        maxHealth = newValue;
    }

    public void SetCurrentHealth(float newValue)
    {
        health = newValue;
    }

    public void SetMovementSpeed(float newValue)
    {
        movementSpeed = newValue;
    }

    public void SetAttack(float newValue)
    {
        attack = newValue;
    }

    #endregion


    public void GetDamage(float damage)
    {
        SetCurrentHealth(health-damage);
    }

    public void GetDamage(float damage, Weapon.WeaponType type)
    {
        
    }
}
