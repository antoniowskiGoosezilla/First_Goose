using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using AntoNamespace;

public class MoveState : State
{
    [SerializeField] State idleState;
    [SerializeField] State attackState;

    public UnityEvent OnFoundPlayer;
    public UnityEvent<bool> OnUpdateMovingCondition;
    
    public override State RunCurrentState()
    {
        if(attackHandler.readyToShot)
        {
            //Se supera tutti i check va in stato di attacco per attaccare
            if(CheckDistance())
            {
                if(CheckVision())
                {
                    isMoving = false;
                    OnUpdateMovingCondition.Invoke(isMoving);
                    return attackState;
                }
                    
            }

            if(!isMoving)
            {
                isMoving = true;
                OnUpdateMovingCondition.Invoke(isMoving);
            }
        }

        //In caso contrario si continua a muovere

        return this;
    }


    //PRIVATE
    private AIAttackHandler attackHandler;
    private GameObject player;
    private float maxAttackDistance = 1000;
    private bool isMoving;
    private bool goOn;

    //Fa un check sulla distanza tra sè e il giocatore per vedere se il player
    //si trova nel range di attacco.
    private bool CheckDistance()
    {
        if(Vector3.Distance(transform.position, player.transform.position) <= maxAttackDistance)
            return true;
        
        return false;
    }

    //Check per vedere se il player si trova nella sua linea visivo
    private bool CheckVision()
    {
        Ray raggio = new Ray(transform.position, player.transform.position - transform.position);
        Debug.DrawRay(raggio.origin, raggio.direction, Color.red, 10);
        if(Physics.Raycast(raggio, maxAttackDistance + 10, 0x64))
            return true;
        
        return false;
    }

    private IEnumerator RandomWait()
    {
        yield return new WaitForSeconds(Random.Range(.5f, 1.5f));
        goOn = true;
    }
    


    //STANDARD
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        attackHandler = GetComponentInParent<AIAttackHandler>();
    }
}
