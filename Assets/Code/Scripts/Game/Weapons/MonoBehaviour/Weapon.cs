using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Weapon : MonoBehaviour, IEquippable
{
    public enum WeaponType
    {
        STANDARD,
        ENERGY,
        MAGIC_FIRE,
        MAGIC_SHOCK,
        MAGIC_ICE
    }

    public enum WeaponCategory
    {
        PISTOL,
        RIFLE,
        SHOTGUN,
        LASER,
        LAUNCHER,
        CHARGER
    }

    public class Bullet
    {
        public float time;
        public Vector3 initialPosition;
        public Vector3 initialVelocity;
        public TrailRenderer tracer;
    }

    
    [SerializeField] protected WeaponTemplate weaponTemplate;

    public string weaponName;
    public string weaponBrand;

    //STATS
    [Header("Stats")]
    public float damage;
    public float range;
    [Range(0,1)]
    public float precision;
    public float reloadTime;
    public float bulletSpeed;
    public float bulletDrop;
    public float maxBulletLifeTime;

    public float shotCooldown;              //Serve per definire il rateo di fuoco
    public bool inCooldown;
    public int mainShotCost = 1;
    public int alternativeShotCost;

    [Header("Ammo")]
    public float magAmmo;
    public float maxMagAmmo;
    public float totalAmmo;
    public float maxTotalAmmo;

    public WeaponType type;
    public int rarity;

    [Header("Suoni")]
    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected AudioClip mainShotSound;
    [SerializeField] protected AudioClip alternativeShotSound;
    [SerializeField] protected AudioClip hitSound;
    [Header("VFX")]
    [SerializeField] protected ParticleSystem shotEffect;
    [SerializeField] protected TrailRenderer trailShotEffect;
    [SerializeField] protected ParticleSystem hitEffect;
    [Space]
    [Header("Utility")]
    [SerializeField] protected Transform muzzle;

    
    protected static LayerMask layerMaskToCheck = 0x64;
    protected List<Bullet> firedBullets = new List<Bullet>();

    public abstract bool Shoot();
    public abstract void AlternativeShoot();
    public abstract void Equip();
    
    public IEnumerator Reload()
    {   
        yield return new WaitForSeconds(reloadTime);

        if(totalAmmo >= maxMagAmmo)
        {
            magAmmo = maxMagAmmo;
            totalAmmo -= maxMagAmmo;
        }

        else
        {
            magAmmo = totalAmmo;
            totalAmmo = 0;
        }

        //Aggiungere suono
        Debug.Log("Caricato");
    }

    public void StandardReload()
    {

        if(totalAmmo >= maxMagAmmo)
        {
            totalAmmo -= maxMagAmmo - magAmmo;
            magAmmo = maxMagAmmo;
        }

        else
        {
            magAmmo = totalAmmo;
            totalAmmo = 0;
        }
        Debug.Log("Caricato");
    }

    public void TickFireDamage()
    {

    }

    public void TickIceDamage()
    {

    }

    public void TickShockDamage()
    {
        
    }

    public void PlaySound(AudioClip clip)
    {
        float pitch = UnityEngine.Random.Range(0.9f, 1.3f);
        try
        {
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(mainShotSound);
        }
        catch
        {
            Debug.LogWarning("AudioSource mancante. Suono riprodotto con PlayClipAtPoint.");
            AudioSource.PlayClipAtPoint(clip, transform.position, 1f);
        }

        
    }

    public void WeaponFirstInit()
    {
        weaponName = weaponTemplate.weaponName;
        weaponBrand = weaponTemplate.weaponBrand;
        damage = weaponTemplate.damage;
        range = weaponTemplate.range;
        precision = weaponTemplate.precision;
        reloadTime = weaponTemplate.reloadTime;
        bulletSpeed = weaponTemplate.bulletSpeed;
        bulletDrop = weaponTemplate.bulletDrop;
        maxBulletLifeTime = weaponTemplate.maxBulletLifeTime;
        shotCooldown = weaponTemplate.shotCooldown;
        inCooldown = false;
        mainShotCost = weaponTemplate.mainShotCost;
        alternativeShotCost = weaponTemplate.alternativeShotCost;
        magAmmo = weaponTemplate.maxMagAmmo;
        maxMagAmmo = weaponTemplate.maxMagAmmo;
        totalAmmo = weaponTemplate.maxTotalAmmo;
        maxTotalAmmo = weaponTemplate.maxTotalAmmo;
        type = weaponTemplate.type;
        rarity = weaponTemplate.rarity;

        mainShotSound = weaponTemplate.mainShotSound;
        alternativeShotSound = weaponTemplate.alternativeShotSound;
        hitSound = weaponTemplate.hitSound;

        trailShotEffect = weaponTemplate.trailShotEffect;
        hitEffect = weaponTemplate.hitEffect;
    }


    //PRIVATE

    #region BULLET

    //Serve ad aggiornare la posizione dei proiettili ad ogni frame
    public void UpdateBullet(float delta)
    {
        SimulateBullets(delta);
        DestroyBullet();
    }

    protected Vector3 GetBulletPosition(Bullet bullet) 
    {
        //posizioneIniziale + velocitaIniziale*tempo + .5f*g*tempo*tempo
        Vector3 gravity = Vector3.down * bulletDrop;
        return bullet.initialPosition + bullet.initialVelocity*bullet.time + .5f*gravity*Mathf.Pow(bullet.time,2);
    }

    //Crea un proiettile e lo inizializza
    protected Bullet CreateBullet(Vector3 position, Vector3 velocity)
    {
        Bullet bullet = new Bullet();
        bullet.initialPosition = position;
        bullet.initialVelocity = velocity;
        bullet.time = 0;
        bullet.tracer = Instantiate(trailShotEffect, position, Quaternion.identity);
        bullet.tracer.AddPosition(muzzle.position);
        return bullet;
    }


    protected void SimulateBullets(float delta)
    {
        firedBullets.ForEach(bullet =>{
            Vector3 position0 = GetBulletPosition(bullet);
            bullet.time += delta;
            Vector3 position1 = GetBulletPosition(bullet);
            RaycastSegment(position0, position1, bullet);
        });
    }

    //Questa funzione serve a fare un ray cast per ogni segmenti che si viene a creare durante i
    //frame tra la posizione vecchia del proiettile e quella nuova.
    //il check per vedere se qualcosa è stato colpito avviene qui.
    protected void RaycastSegment(Vector3 start, Vector3 end, Bullet bullet)
    {
       //Generiamo un raggio per vedere se il proiettile colpisce il nemico
        if(muzzle == null) muzzle = transform;
        Ray shot = new Ray(start, end - start); 
        RaycastHit hit;

        bool isHit = Physics.Raycast(shot, out hit, (end-start).magnitude, layerMaskToCheck); //Sostituire lo 0 con il layermask su cui fare il controllo

        //Se colpisce
        if(isHit)
        {
            try
            {
                ParticleSystem effect = Instantiate(hitEffect, hit.point, Quaternion.identity);
                effect.transform.forward = hit.normal;
                effect.Emit(1);
                bullet.tracer.transform.position = hit.point;
                bullet.time = maxBulletLifeTime;        //Distrugger il proiettile
            }
            catch
            {
                Debug.LogError("Effetto Hit o Trail mancante");
            }

            Debug.Log("Colpito");
        }
        else
        {
            bullet.tracer.transform.position = end; //Orribile, ma per ora fa il suo lavoro
        }
    }

    protected void DestroyBullet()
    {
        firedBullets.RemoveAll(bullet => bullet.time >= maxBulletLifeTime);
    }
    #endregion


}
