using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateHandler : MonoBehaviour
{
    [SerializeField] State currentState;


    void Update()
    {
        RunStateMachine();
    }

    void RunStateMachine()
    {
        State nextState = currentState?.RunCurrentState();

        if(nextState != null)
        {
            SwitchToNextState(nextState);
        }

    }

    public void SwitchToNextState(State nextState)
    {
        currentState = nextState;
    }    
}
