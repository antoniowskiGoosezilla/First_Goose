using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AntoNamespace{    
    public class PlayerStatsHandler : CharacterStats
    {
        //PUBLIC O ACCESSIBILI DALL'EDITOR
        public int availableActionStacks; //{get; private set;}    //Variabile accessibile dagli altri moduli
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

        public void SetAvailableActionStacks(int newValue)          //Funzione utile per settare sia
        {                                                           //nuovi valori che per eseguire operazioni
            availableActionStacks = newValue;                       //come sottrazioni o addizioni di stacks

            if(availableActionStacks > maxActionStacks)
                SetAvailableActionStacksToMaxValue();
            
            if(availableActionStacks < 0)
                availableActionStacks = 0;

            //Iniziamo il cooldown delle stack
            if(availableActionStacks == 0)
            {
                StopAllStackCooldowns();
                StartCoroutine("StartMaxActionStackCooldown");
            }
            else
            {   
                Coroutine coroutine = StartCoroutine("StartActionStacksCooldown");
                stackCoroutine.Add(coroutine);
            }
        }
        




        //PRIVATE
        private List<Coroutine> stackCoroutine;
        private int maxActionStacks = 5;
        private float actionStacksCooldown = 3;
        private bool inCooldown;

        // Start is called before the first frame update
        void Start()
        {
            availableActionStacks = maxActionStacks;
            stackCoroutine = new List<Coroutine>();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private IEnumerator StartActionStacksCooldown()
        {
            if(inCooldown)
                yield break;

            inCooldown = true;
            yield return new WaitForSeconds(actionStacksCooldown);
            SetAvailableActionStacks(availableActionStacks + 1);
            inCooldown = false;

            if(availableActionStacks != maxActionStacks)
            {
                Coroutine coroutine = StartCoroutine(StartActionStacksCooldown());
                stackCoroutine.Add(coroutine);
            }
                
        }

        private IEnumerator StartMaxActionStackCooldown()
        {
            yield return new WaitForSeconds(actionStacksCooldown + 1);
            SetAvailableActionStacksToMaxValue();
        }

        private void StopAllStackCooldowns()
        {
            inCooldown = false;
            
            foreach (Coroutine coroutine in stackCoroutine)
            {
                if(coroutine != null)
                    StopCoroutine(coroutine);
            }

            stackCoroutine.Clear();
        }

    }
}