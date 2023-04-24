using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


//Questa classe serve ad avere un unico oggetto che può essere
//salvato nel Run Manager in modo da tenere l'inventario sempre
//salvato tra le scene
public class Inventory
{
    public Weapon firstWeapon;
    public Weapon secondWeapon;
    public Weapon meleeWeapon;

    public int weaponIndex;

    //Sostituire con la classe oggetto
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
    public Inventory(Weapon first, Weapon melee)
    {
        firstWeapon = first;
        secondWeapon = null;
        meleeWeapon = melee;
        weaponIndex = 0;
        equippedObject = null;
    }
}
