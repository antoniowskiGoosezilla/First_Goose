using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryHandler : MonoBehaviour
{

    private Inventory inventory;

    public Weapon equippedWeapon;
    public string equippedObject;

    // Start is called before the first frame update
    void Start()
    {
        //Da verificare prima se è disponibile un invantario nel Run Manager
        if(inventory == null)
            inventory = new Inventory();

        equippedWeapon = GetWeapon(inventory.weaponIndex);
        equippedObject = inventory.equippedObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Weapon GetWeapon(int index)
    {
        switch(index)
        {
            case 0:
                return inventory.firstWeapon;
            case 1:
                return inventory.secondWeapon;
            case 2:
                return inventory.meleeWeapon;
            default:
                return null;
        }
    }
    public Weapon GetNextWeapon()
    {
        inventory.weaponIndex += 1;
        if(inventory.weaponIndex > 2)
            inventory.weaponIndex = 0;
        
        return GetWeapon(inventory.weaponIndex);

    }
    public Weapon GetPreviousWeapon()
    {
        inventory.weaponIndex -= 1;
        if(inventory.weaponIndex < 0)
            inventory.weaponIndex = 2;
        
        return GetWeapon(inventory.weaponIndex);
    }
}
