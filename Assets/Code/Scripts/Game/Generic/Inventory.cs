using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


//Questa classe serve ad avere un unico oggetto che può essere
//salvato nel Run Manager in modo da tenere l'inventario sempre
//salvato tra le scene
[System.Serializable]
public class Inventory
{
    [System.Serializable]
    public struct MagHolder
    {
        public bool isUsed;
        public int currentMagAmmo;
        public int totalAmmo;
    }


    public WeaponTemplate firstWeapon;
    public WeaponTemplate secondWeapon;
    public WeaponTemplate meleeWeapon;

    public MagHolder firstWeaponAmmo;
    public MagHolder secondWeaponAmmo;

    //Indice per ciclare tra le armi
    public int weaponIndex;

    //TODO: Sostituire con la classe oggetto
    public string equippedObject;

    //Tutti i poweup collezionati
    public string[] cassette;

    //COSTRUTTORI
    public Inventory()
    {
        firstWeapon = null;
        secondWeapon = null;
        meleeWeapon = null;
        weaponIndex = 0;
        equippedObject = null;
    }
    public Inventory(WeaponTemplate first, WeaponTemplate melee)
    {
        firstWeapon = first;
        secondWeapon = null;
        meleeWeapon = melee;
        weaponIndex = 0;
        equippedObject = null;
    }
}
