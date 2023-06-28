using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AntoNamespace{

    public class AIStatsHandler : CharacterStats
    {

        //PUBLIC
        public float hitPoint = 50;
        public float killPoint = 200;

        //PRIVATE
        private int level;

        //Si tratta di una curva da usare per gestire un aumento non lineare delle statistiche
        //del nemico
        private AnimationCurve levelCurve;
        

        private void Init()
        {
            this.health = this.maxHealth*levelCurve.Evaluate(level);
            this.movementSpeed = this.movementSpeed*levelCurve.Evaluate(level);
            this.attack = this.attack*levelCurve.Evaluate(level);
        }


        //STANDARD FUNC
        void Awake()
        {
            //Init();
        }
    
        void Start()
        {
            
        }

        
        void Update()
        {
            
        }
    }
}
