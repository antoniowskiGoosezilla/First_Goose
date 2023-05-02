using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventoryHandler : MonoBehaviour
{
    [SerializeField] Inventory inventory;
    public GameObject equippedWeapon;
    public string equippedObject; //Creare tipo Oggetto

    void Awake()
    {
        AntoNamespace.InputCustomSystem.OnNextWeaponAction += GetNextWeapon;
        AntoNamespace.InputCustomSystem.OnPreviousWeaponAction += GetPreviousWeapon;
    }
    void Start()
    {
        //Da verificare prima se è disponibile un invantario nel Run Manager
        if(inventory == null)
            inventory = new Inventory();
        
        UnequipWeapon();
        EquipWeapon(GetWeapon(inventory.weaponIndex));
    }

    void Update()
    {
        
    }

    public GameObject GetWeapon(int index)
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
    public void GetNextWeapon(InputAction.CallbackContext context)
    {
        inventory.weaponIndex += 1;
        if(inventory.weaponIndex > 2)
            inventory.weaponIndex = 0;
        UnequipWeapon();
        EquipWeapon(GetWeapon(inventory.weaponIndex));

    }
    public void GetPreviousWeapon(InputAction.CallbackContext context)
    {
        inventory.weaponIndex -= 1;
        if(inventory.weaponIndex < 0)
            inventory.weaponIndex = 2;
        
        UnequipWeapon();
        EquipWeapon(GetWeapon(inventory.weaponIndex));
    }
    private void EquipWeapon(GameObject weapon)
    {
        equippedWeapon = Instantiate(weapon, transform.Find("RightHand").position, Quaternion.identity);
        equippedObject = inventory.equippedObject;
        equippedWeapon.transform.parent = transform.Find("RightHand");
        equippedWeapon.transform.position = new Vector3(equippedWeapon.transform.position.x, equippedWeapon.transform.position.y, equippedWeapon.transform.position.z+.2f);
    }
    private void UnequipWeapon()
    {
        Destroy(equippedWeapon);
    }
}
