using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class IdleState : State
{
    [SerializeField] State moveState;

    public override State RunCurrentState()
    {
        return moveState;
    }



    //PRIVATE
}
