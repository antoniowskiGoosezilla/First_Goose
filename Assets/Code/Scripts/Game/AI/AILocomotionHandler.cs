using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AntoNamespace
{    
    public class AILocomotionHandler : MonoBehaviour
    {
        //PUBLIC

        public void UpdateMoveBool(bool newValue)
        {
            isMoving = newValue;
        }


        //PRIVATE

        private AIStatsHandler statistiche;
        private NavMeshAgent agent;
        private bool isMoving;

        private void Move()
        {
            if(isMoving)
            {
                Debug.Log("Moving");
                agent.isStopped = false;
                agent.SetDestination(GameObject.FindGameObjectWithTag("Player").transform.position);
                return;
            }

            agent.isStopped = true;
            Debug.Log("Not Moving");
            return;
        }


        //STANDARD
        void Awake()
        {
            statistiche = GetComponent<AIStatsHandler>();
            agent = GetComponent<NavMeshAgent>();
        }

        void Start()
        {
            
        }

        void Update()
        {
            Move();
        }
    }
}