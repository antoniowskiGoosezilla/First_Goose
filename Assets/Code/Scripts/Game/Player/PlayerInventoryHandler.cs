using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using AntoNamespace;

public class PlayerInventoryHandler : MonoBehaviour
{

    //PUBLIC
    [SerializeField] Inventory inventory;

    public GameObject equippedWeapon;
    public string equippedObject; //Creare tipo Oggetto
    [Space]
    [Header("SUONI")]
    [SerializeField] AudioClip perfectReloadSound;

    
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




    //PRIVATE

    private bool isReloading;
    private float deltaReloading = 0.3f;
    private float minPerfectReload;
    private float maxPerfectReload;
    private float reloadTimer = 0;



    private void Awake()
    {
        InputCustomSystem.OnNextWeaponAction += GetNextWeapon;
        InputCustomSystem.OnPreviousWeaponAction += GetPreviousWeapon;
        InputCustomSystem.OnReloadAction += Reload;
    }
    private void Start()
    {
        //Da verificare prima se è disponibile un invantario nel Run Manager
        if(inventory == null)
            inventory = new Inventory();
        
        UnequipWeapon();
        EquipWeapon(GetWeapon(inventory.weaponIndex));
    }

    private void Update()
    {
        
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

    private void Reload(InputAction.CallbackContext context)
    {
        Weapon usedWeapon = equippedWeapon.GetComponent<Weapon>();
        if(usedWeapon == null)
            return;
        
        if(usedWeapon.magAmmo == usedWeapon.maxMagAmmo)            //Non deve caricare se il caricatore è pieno
            return;

        if(usedWeapon.totalAmmo <= 0)                              //Non hai altre munizioni a disposizione
            return;

        if(!isReloading)
        {
            minPerfectReload = Random.Range(0.2f, usedWeapon.reloadTime);
            maxPerfectReload = minPerfectReload + deltaReloading;
            StartCoroutine(ReloadQuickTimeEvent(usedWeapon));
            return;
        }
        
        StopCoroutine("ReloadQuickTimeEvent");
        if(reloadTimer >= minPerfectReload && reloadTimer <= maxPerfectReload)
        {
            //AGGIUNGI I PUNTI BONUS
            //StartCoroutine(usedWeapon.Reload());
            AudioSource.PlayClipAtPoint(perfectReloadSound, transform.position); //DEBUG
            usedWeapon.StandardReload();
            ResetRelaod();
        }
        else{
            //AGGIUNGERE MALUS
            usedWeapon.StandardReload();
            ResetRelaod();
        }
        //StartCoroutine(usedWeapon.Reload());
    }

    private IEnumerator ReloadQuickTimeEvent(Weapon weapon)
    {
        reloadTimer = 0;
        while (reloadTimer < weapon.reloadTime)
        {
            reloadTimer += Time.deltaTime;
            yield return null;
        }

        //Aggiungere Malus dei punti
        //StartCoroutine(weapon.Reload());
        weapon.StandardReload();
        ResetRelaod();
    }

    private void ResetRelaod()
    {
            reloadTimer = 0;
            minPerfectReload = 0;
            maxPerfectReload = 0;
            isReloading = false;
    }
    
}
