using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AntoNamespace{    
    public class PlayerStatsHandler : CharacterStats
    {

        public int actionStacks;
        private int maxActionStacks;
        public int actionStacksCoolDown;
        public bool inCooldown;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }


        public void SetMaxActionStacks(int newValue)
        {
            maxActionStacks = newValue;
        }

        public void SetActionStacks(int newValue)
        {
            actionStacks = newValue;

            if(actionStacks > maxActionStacks)
                actionStacks = maxActionStacks;   
        }

        public void AddActionStacks(int numberToAdd)
        {
            actionStacks += numberToAdd;

            if(actionStacks > maxActionStacks)
                actionStacks = maxActionStacks;
        }

        public IEnumerator StartActionStacksCooldown()
        {
            yield return null;
        }
    }
}