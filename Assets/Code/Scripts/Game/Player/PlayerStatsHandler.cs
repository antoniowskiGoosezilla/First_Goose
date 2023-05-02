using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AntoNamespace{    
    public class PlayerStatsHandler : CharacterStats
    {
        //PUBLIC O ACCESSIBILI DALL'EDITOR
        public int availableActionStacks {get; private set;}   //Variabile accessibile dagli altri moduli
                                                                //per verificare la presenza di AS.
                                                                //Considerare un possibile "semaforo" per l'accesso
        
        public void SetMaxActionStacks(int newValue)
        {
            maxActionStacks = newValue;
        }

        public void SetAvailableActionStacksToMaxValue()
        {
            availableActionStacks = maxActionStacks;
        }

        public void SetAvailableActionStacks(int newValue)
        {
            availableActionStacks = newValue;

            if(availableActionStacks > maxActionStacks)
                SetAvailableActionStacksToMaxValue();
            
            if(availableActionStacks < 0)
                availableActionStacks = 0;

            //Iniziamo il cooldown delle stack
            if(availableActionStacks == 0)
            {
                StartCoroutine("StartActionStacksCooldown");
            }
            else
            {
                StartCoroutine("StartMaxActionStackCooldown");
            }
        }
        




        //PRIVATE
        private int maxActionStacks = 5;
        private float actionStacksCooldown = 2;
        private bool inCooldown;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private IEnumerator StartActionStacksCooldown()
        {
            yield return new WaitForSeconds(actionStacksCooldown);
            SetAvailableActionStacks(availableActionStacks + 1);
        }

        private IEnumerator StartMaxActionStackCooldown()
        {
            yield return new WaitForSeconds(actionStacksCooldown + 1);
            SetAvailableActionStacksToMaxValue();
        }


    }
}