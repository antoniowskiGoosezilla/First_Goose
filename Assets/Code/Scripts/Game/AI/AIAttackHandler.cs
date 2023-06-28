using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttackHandler : MonoBehaviour
{
    //PUBLIC
    public bool readyToShot;

    public void StartCooldown()
    {
        readyToShot = false;
        StartCoroutine(CooldownTimer());
    }

    //PRIVATE
    private float shotCooldown = 3f;






    private IEnumerator CooldownTimer()
    {
        yield return new WaitForSeconds(shotCooldown);
        readyToShot = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
