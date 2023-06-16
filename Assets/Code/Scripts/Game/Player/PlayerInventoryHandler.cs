using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using AntoNamespace;

[RequireComponent(typeof(ComboHandler))]
[RequireComponent(typeof(BulletHandler))]
public class PlayerInventoryHandler : MonoBehaviour
{

    //PUBLIC
    public GameObject equippedWeapon;
    public string equippedObject; //Creare tipo Oggetto
    
    [SerializeField] Inventory inventory;
    [Space]
    [Header("SUONI")]
    [SerializeField] AudioClip perfectReloadSound;
    [Header("Utility")]
    [SerializeField] GameObject rightHand;


    //EVENTI
    public static event Action<float, float, float> OnUpdateWeaponAmmo;
    //*************

    public void GetNextWeapon(InputAction.CallbackContext context)
    {
        if(equippedWeapon != null)
            UnequipWeapon(inventory.weaponIndex);
        
        inventory.weaponIndex += 1;
        if(inventory.weaponIndex > 2)
            inventory.weaponIndex = 0;

        if(GetWeapon(inventory.weaponIndex) == null)
        {
            GetNextWeapon(context);
            return;    
        }

        EquipWeapon(GetWeaponPrefab(inventory.weaponIndex));
    }
    public void GetPreviousWeapon(InputAction.CallbackContext context)
    {
        if(equippedWeapon != null)
            UnequipWeapon(inventory.weaponIndex);

        inventory.weaponIndex -= 1;
        if(inventory.weaponIndex < 0)
            inventory.weaponIndex = 2;
        
        if(GetWeapon(inventory.weaponIndex) == null)
        {
            GetPreviousWeapon(context);
            return;    
        }

        EquipWeapon(GetWeaponPrefab(inventory.weaponIndex));
    }





    //PRIVATE

    private ReloadCanvasHandler reloadCanvas;
    private ComboHandler comboHandler;
    private BulletHandler bulletHandler;

    private bool isReloading;
    private float deltaReloading;
    private float minPerfectReload;
    private float maxPerfectReload;
    private float reloadTimer = 0;
    private Coroutine reloadCoroutine;


    private void Awake()
    {
        reloadCanvas = GetComponentInChildren<ReloadCanvasHandler>();
        comboHandler = GetComponent<ComboHandler>();
        bulletHandler = GetComponent<BulletHandler>();

        InputCustomSystem.OnNextWeaponAction += GetNextWeapon;
        InputCustomSystem.OnPreviousWeaponAction += GetPreviousWeapon;
        InputCustomSystem.OnReloadAction += Reload;
    }
    private void Start()
    {
        //Da verificare prima se è disponibile un inventario nel Run Manager
        if(inventory == null)
            inventory = new Inventory();
        
        UnequipWeapon();
        EquipWeapon(GetWeaponPrefab(inventory.weaponIndex));
    }
    private void Update()
    {
        
    }

    //Funzione per equipaggiare l'arma instanziando il modello 3D nella posizione desiderata

    private void EquipWeapon(GameObject weapon)
    {   
        equippedWeapon = Instantiate(weapon, rightHand.transform.position, Quaternion.LookRotation(-transform.forward));
        //equippedObject = inventory.equippedObject;
        equippedWeapon.transform.parent = rightHand.transform;
        equippedWeapon.transform.position = new Vector3(equippedWeapon.transform.position.x, equippedWeapon.transform.position.y, equippedWeapon.transform.position.z);

        Weapon weaponInfo = equippedWeapon.GetComponent<Weapon>();

        if(GetWeaponAmmo(inventory.weaponIndex).isUsed)
            weaponInfo.RestoreAmmo(GetWeaponAmmo(inventory.weaponIndex));

        weaponInfo.SetBulletHandler(bulletHandler);                 //Impostiamo l'handler che si occupa dei proiettili dell'arma

        //Aggiorno il valore del delta per la ricarica perfetta
        deltaReloading = weaponInfo.reloadTime * 0.15f;

        OnUpdateWeaponAmmo?.Invoke(weaponInfo.magAmmo, weaponInfo.totalAmmo, weaponInfo.maxTotalAmmo);
    }
    private void UnequipWeapon()
    {
        if(equippedWeapon != null)
            Destroy(equippedWeapon);
    }
    private void UnequipWeapon(float index)                            //Funzione per rimuovere i modelli 3D quando si cambia o butta un'arma
    {       
        switch(index)
        {
            case 0:
                Weapon weaponData = equippedWeapon.GetComponent<Weapon>();
                inventory.firstWeapon = weaponData.GetWeaponTemplate();
                inventory.firstWeaponAmmo.isUsed = true;
                inventory.firstWeaponAmmo.currentMagAmmo = weaponData.magAmmo;
                inventory.firstWeaponAmmo.totalAmmo = weaponData.totalAmmo;
                break;
            case 1:
                Weapon weaponData2 = equippedWeapon.GetComponent<Weapon>();
                inventory.secondWeapon = weaponData2.GetWeaponTemplate();
                inventory.secondWeaponAmmo.isUsed = true;
                inventory.secondWeaponAmmo.currentMagAmmo = weaponData2.magAmmo;
                inventory.secondWeaponAmmo.totalAmmo = weaponData2.totalAmmo;
                 break;
            case 2:
                //Weapon weaponData3 = equippedWeapon.GetComponent<Weapon>();
                //inventory.firstWeapon = weaponData3.GetWeaponTemplate();
                break;
            default:
                 break;
        }

        Destroy(equippedWeapon);
    }

    //Funzione di ricarica legata al player.
    //La ricarica chiama l'effettiva funzione dell'arma per ricaricarla
    //ma è utile a gestire tutta una serie di controlli che altrimenti sarebbero
    //legati alle singole armi

    private void Reload(InputAction.CallbackContext context)
    {
        Weapon usedWeapon = equippedWeapon.GetComponent<Weapon>();
        if(usedWeapon == null)                                     //Check per evitare errori
            return;
        
        if(usedWeapon.magAmmo == usedWeapon.maxMagAmmo)            //Non deve caricare se il caricatore è pieno
            return;

        if(usedWeapon.totalAmmo <= 0)                              //Non hai altre munizioni a disposizione
            return;

        if(!isReloading)
        {
            isReloading = true;
            minPerfectReload = UnityEngine.Random.Range(0.45f, usedWeapon.reloadTime - deltaReloading);
            maxPerfectReload = minPerfectReload + deltaReloading;
            float media = (maxPerfectReload + minPerfectReload)*0.5f;       //Necessario per trovare il punto medio e aggiornare il canvas di ricarica
            reloadCanvas.UpdateTargetPosition(media, usedWeapon.reloadTime, deltaReloading);

            reloadCoroutine = StartCoroutine(ReloadQuickTimeEvent(usedWeapon));
            return;
        }
        
        StopCoroutine(reloadCoroutine);
        if(reloadTimer >= minPerfectReload && reloadTimer <= maxPerfectReload)
        {
            comboHandler.AddPoints(150);
            //StartCoroutine(usedWeapon.Reload());
            AudioSource.PlayClipAtPoint(perfectReloadSound, transform.position); //DEBUG
            usedWeapon.StandardReload();
            ResetReload();
        }
        else{
            //AGGIUNGERE MALUS
            comboHandler.AddPoints(-250);
            usedWeapon.StandardReload();
            ResetReload();
        }
        OnUpdateWeaponAmmo?.Invoke(usedWeapon.magAmmo, usedWeapon.totalAmmo, usedWeapon.maxTotalAmmo);
        //StartCoroutine(usedWeapon.Reload());
    }

    private IEnumerator ReloadQuickTimeEvent(Weapon weapon)
    {   
        reloadTimer = 0;
        reloadCanvas.SetMaxReloadValue(weapon.reloadTime);
        while (reloadTimer < weapon.reloadTime)
        {
            float oldValue = reloadTimer;
            reloadTimer += Time.deltaTime;
            reloadCanvas.UpdateQuickTimeEventReload(reloadTimer, oldValue, reloadTimer/weapon.reloadTime);
            yield return null;
        }

        //Aggiungere Malus dei punti
        //StartCoroutine(weapon.Reload());              //Ricarica con cooldown
        weapon.StandardReload();                        //Ricarica Immediata;
        ResetReload();

        OnUpdateWeaponAmmo?.Invoke(weapon.magAmmo, weapon.totalAmmo, weapon.maxTotalAmmo);
    }

    private void ResetReload()
    {
            reloadTimer = 0;
            minPerfectReload = 0;
            maxPerfectReload = 0;
            isReloading = false;
            reloadCoroutine = null;
            reloadCanvas.DeactivateReloadSlider();
    }
    
    private WeaponTemplate GetWeapon(int index)
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
    private GameObject GetWeaponPrefab(int index)
    {
        switch(index)
        {
            case 0:
                return inventory.firstWeapon.prefab;
            case 1:
                return inventory.secondWeapon.prefab;
            case 2:
                return inventory.meleeWeapon.prefab;
            default:
                return null;
        }
    }
    private Inventory.MagHolder GetWeaponAmmo(int index)
    {
        switch(index)
        {
            case 0:
                return inventory.firstWeaponAmmo;
            case 1:
                return inventory.secondWeaponAmmo;
            default:
                return new Inventory.MagHolder();
        }
    }
}
