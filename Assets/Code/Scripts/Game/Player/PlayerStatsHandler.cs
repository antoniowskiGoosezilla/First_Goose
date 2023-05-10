using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace AntoNamespace{    
    public class PlayerStatsHandler : CharacterStats
    {
        //PUBLIC O ACCESSIBILI DALL'EDITOR
        public int availableActionStacks; /*{get; private set;}*/    //Variabile accessibile dagli altri moduli
                                                                //per verificare la presenza di AS.
                                                                //Considerare un possibile "semaforo" per l'accesso
        
        [Space]
        [Header("EVENTI")]
        public UnityEvent OnEndStacks;                          //UNITY EVENTS per comunicare con moduli dello stesso oggetto (VEDI ISPECTOR)
        
        public static event Action<int, bool> OnUpdateStacks;   //Eventi normali per comunicare con moduli globali o di altri oggetti
        public static event Action<int, float> OnUpdateCooldown;


        public void SetMaxActionStacks(int newValue)
        {
            maxActionStacks = newValue;
        }

        public void SetAvailableActionStacksToMaxValue()
        {
            availableActionStacks = maxActionStacks;
        }

        public void SetAvailableActionStacks(int newValue)          //(DEPRECATED)Funzione utile per settare sia
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
                OnEndStacks.Invoke();
                StartCoroutine("StartMaxActionStackCooldown");
            }
            else
            {   
                Coroutine coroutine = StartCoroutine("StartActionStacksCooldown");
                stackCoroutine.Add(coroutine);
            }
        }

        public void RemoveActionStacks(int valueToRemove)
        {
            OnUpdateStacks?.Invoke(availableActionStacks, false);
            availableActionStacks -= valueToRemove;

            if(availableActionStacks < 0)
                availableActionStacks = 0;

            //Iniziamo il cooldown delle stack
            if(availableActionStacks == 0)
            {
                StopAllStackCooldowns();
                OnEndStacks.Invoke();
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
            float timer = 0;
            //yield return new WaitForSeconds(actionStacksCooldown);
            while(timer < actionStacksCooldown)
            {
                timer += Time.deltaTime;
                OnUpdateCooldown?.Invoke(availableActionStacks+1,timer/actionStacksCooldown);
                yield return null;
            }
            SetAvailableActionStacks(availableActionStacks + 1);
            OnUpdateStacks?.Invoke(availableActionStacks, true);
            inCooldown = false;

            if(availableActionStacks != maxActionStacks)
            {
                Coroutine coroutine = StartCoroutine(StartActionStacksCooldown());
                stackCoroutine.Add(coroutine);
            }
                
        }

        private IEnumerator StartMaxActionStackCooldown()
        {
            OnUpdateStacks?.Invoke(-1, false);
            yield return new WaitForSeconds(actionStacksCooldown + 1);
            SetAvailableActionStacksToMaxValue();
            OnUpdateStacks?.Invoke(-1, true);
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