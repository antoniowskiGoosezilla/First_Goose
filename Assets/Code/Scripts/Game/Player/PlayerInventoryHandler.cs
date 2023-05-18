using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using AntoNamespace;

public class PlayerInventoryHandler : MonoBehaviour
{

    //PUBLIC
    public GameObject equippedWeapon;
    public string equippedObject; //Creare tipo Oggetto
    
    [SerializeField] Inventory inventory;
    [Space]
    [Header("SUONI")]
    [SerializeField] AudioClip perfectReloadSound;


    //EVENTI
    public static event Action<float, float> OnUpdateWeaponAmmo;
    //*************

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

    private ReloadCanvasHandler reloadCanvas;
    private ComboHandler comboHandler;

    private bool isReloading;
    private float deltaReloading = 0.3f;
    private float minPerfectReload;
    private float maxPerfectReload;
    private float reloadTimer = 0;
    private Coroutine reloadCoroutine;


    private void Awake()
    {
        reloadCanvas = GetComponentInChildren<ReloadCanvasHandler>();
        comboHandler = GetComponent<ComboHandler>();

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

    //Funzione per equipaggiare l'arma instanziando il modello 3D nella posizione desiderata

    private void EquipWeapon(GameObject weapon)
    {
        equippedWeapon = Instantiate(weapon, transform.Find("RightHand").position, Quaternion.identity);
        //equippedObject = inventory.equippedObject;
        equippedWeapon.transform.parent = transform.Find("RightHand");
        equippedWeapon.transform.position = new Vector3(equippedWeapon.transform.position.x, equippedWeapon.transform.position.y, equippedWeapon.transform.position.z+.2f);

        OnUpdateWeaponAmmo?.Invoke(equippedWeapon.GetComponent<Weapon>().magAmmo, equippedWeapon.GetComponent<Weapon>().totalAmmo);
    }

    private void UnequipWeapon()                            //Funzione per rimuovere i modelli 3D quando si cambia o butta un
    {                                                       //un'arma
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
            float media = (maxPerfectReload + minPerfectReload)*0.5f;
            reloadCanvas.UpdateTargetPosition(media, usedWeapon.reloadTime);

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
        OnUpdateWeaponAmmo?.Invoke(usedWeapon.magAmmo, usedWeapon.totalAmmo);
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

        OnUpdateWeaponAmmo?.Invoke(weapon.magAmmo, weapon.totalAmmo);
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
    
}
