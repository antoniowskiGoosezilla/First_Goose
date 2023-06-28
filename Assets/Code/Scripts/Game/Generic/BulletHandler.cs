using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using AntoNamespace;

public class BulletHandler : MonoBehaviour
{

    public class Bullet
    {
        public float time;
        public Vector3 initialPosition;
        public Vector3 initialVelocity;
        public float bulletDrop;
        public float maxBulletLifeTime;
        public Transform muzzle;
        public GameObject tracer;
        public GameObject hitEffect;
        public LayerMask layerMask;
    }

    /*** PUBLIC ***/
    //EVENT

    //UNITY EVENT
    public UnityEvent<float> OnHit;



    //Crea un proiettile e lo inizializza
    public Bullet CreateBullet(Vector3 position, Vector3 velocity, float bulletDrop, float maxBulletLifeTime, GameObject trailShotEffect, GameObject hitEffect, LayerMask layerMaskToCheck)
    {
        Bullet bullet = new Bullet();
        bullet.initialPosition = position;
        bullet.initialVelocity = velocity;
        bullet.bulletDrop = bulletDrop;
        bullet.maxBulletLifeTime = maxBulletLifeTime;
        bullet.time = 0;
        bullet.tracer = Instantiate(trailShotEffect, position, Quaternion.identity);
        bullet.tracer.transform.forward = velocity.normalized;
        //bullet.tracer.AddPosition(position);
        bullet.hitEffect = hitEffect;
        bullet.layerMask = layerMaskToCheck;

        AddBullet(bullet);

        return bullet;
    }

    //Funzione specifica per gli shotgun.
    //Non viene creato un vero e proprio proiettile, ma viene creata una checkbox
    //davanti al player per controllare se il colpo colpisce o meno qualcuno
    public void CreateShotgunBullet(Transform center, float range, LayerMask layerMasktoCheck)
    {
        float halfDimension = range*.5f;
        //Aggiustare posizione box
        bool isHit = Physics.CheckBox(new Vector3(center.position.x, center.position.y, center.position.z) - center.forward*halfDimension, new Vector3(halfDimension, halfDimension, halfDimension), Quaternion.identity, layerMasktoCheck);
        //ExtDebug.DrawBox(new Vector3(center.position.x, center.position.y, center.position.z) - center.forward*halfDimension, new Vector3(halfDimension, halfDimension, halfDimension), Quaternion.identity, Color.red);
        if(isHit)
        {
            //Aggiungiamo i punti in caso di hit    
            OnHit?.Invoke(100f); //TODO: Sostituire con i punti del personaggio
            Debug.Log("Colpito");
        }
    }





    /*** PRIVATE ***/

    //Lista dei proiettili sparati e da aggiornare frame dopo frame
    private List<Bullet> firedBullets;
    


    //Aggiunge un proiettile da gestire nell'array di proiettili
    private void AddBullet(Bullet bullet)
    {
        firedBullets.Add(bullet);
    }

    private Vector3 GetBulletPosition(Bullet bullet) 
    {
        //posizioneIniziale + velocitaIniziale*tempo + .5f*g*tempo*tempo
        Vector3 gravity = Vector3.down * bullet.bulletDrop;
        return bullet.initialPosition + bullet.initialVelocity*bullet.time + .5f*gravity*Mathf.Pow(bullet.time,2);
    }
    
    //Serve ad aggiornare la posizione dei proiettili ad ogni frame
    private void UpdateBullet(float delta)
    {
        SimulateBullets(delta);
        DestroyBullet();
    }

    private void SimulateBullets(float delta)
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
    private void RaycastSegment(Vector3 start, Vector3 end, Bullet bullet)
    {
        Ray shot = new Ray(start, end - start); 
        RaycastHit hit;

        bool isHit = Physics.Raycast(shot, out hit, (end-start).magnitude, bullet.layerMask); //Sostituire lo 0 con il layermask su cui fare il controllo

        //Se colpisce
        if(isHit)
        {
            try
            {
                GameObject effect = Instantiate(bullet.hitEffect, hit.point, Quaternion.identity);
                effect.transform.forward = hit.normal;
                //effect.Emit(1);
                bullet.tracer.transform.position = hit.point;
                Destroy(bullet.tracer);
                bullet.time = bullet.maxBulletLifeTime;        //Distrugger il proiettile
            }
            catch
            {
                Debug.LogError("Effetto Hit o Trail mancante");
            }
            //Aggiungiamo i punti in caso di hit
            AIStatsHandler enemyStatsHandler = hit.collider.gameObject.GetComponent<AIStatsHandler>();    
            if(enemyStatsHandler != null)
            {
                OnHit?.Invoke(enemyStatsHandler.hitPoint);
            }
            else
            {
                OnHit?.Invoke(100f); 
            }
            
        }
        else
        {
            bullet.tracer.transform.position = end; //Orribile, ma per ora fa il suo lavoro
        }
    }

    private void DestroyBullet()
    {
        //Troppo pesante secondo me
        List<Bullet> list = firedBullets.FindAll(bullet => bullet.time >= bullet.maxBulletLifeTime);
        list.ForEach(bullet => Destroy(bullet.tracer));
        firedBullets.RemoveAll(bullet => bullet.time >= bullet.maxBulletLifeTime);
    }
    





    
    //Standard

    private void Awake()
    {
        firedBullets = new List<Bullet>();
    }

    private void Update()
    {
        UpdateBullet(Time.deltaTime);
    }
}
