using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AntoNamespace
{    
    public class AttackState : State
    {   
        [SerializeField] State moveState;

        public UnityEvent OnShot;

        public override State RunCurrentState()
        {
            print("BANG");
            OnShot.Invoke();
            return moveState;
        }
    }
}
