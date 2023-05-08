using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AntoNamespace
{
    //Questa classe serve da gestore della stanza che il giocatore sta
    //giocando. Deve tenere conto di quanti nemici ci devono essere,
    //quando la stanza è completata e se devono accadere eventi legati alla
    //stanza.
    //N.B.: Ogno stanza è una scena, dunque il manager sarà presente nella scena
    //a prescindere

    public class RoomManager : MonoBehaviour
    {
        public enum Rooms
        {

        }

        [System.Serializable]
        public struct RoomStage                     //Serve per indicare i tipi di nemici da far spawnare
        {
            public int jumpers;
            public Transform[] jumpersSpawnPoints;
            public int bandits;
            public Transform[] banditsSpawnPoints;
            public int vice;
            public Transform[] viceSpawnPoints;
        }

        //PUBLIC

        [SerializeField] RoomStage[] stages;
        [SerializeField] float stageDelay;
        [Space]
        [Header("BOSS SECTION")]
        [SerializeField] bool bossRoom;



        public void SpawnEnemy()
        {
            currentEnemiesCounter = stages[stageIndex].jumpers + stages[stageIndex].bandits + stages[stageIndex].vice;
            //Aggiungere funzione di spawn

            stageIndex++;
        }

        //PRIVATE
        private int stageIndex;                        //Serve a sapere in che stage ci troviamo
        private int currentEnemiesCounter;             //Quanti nemici nello stage attuale

        private void Awake()
        {

        }

        private void Start()
        {
            
        }

        private void Update()
        {
            
        }

        private void ReduceCurrentEnemyCounter()
        {
            currentEnemiesCounter -= 1;
        }

        private void CheckEnemyCounter()
        {
            if(currentEnemiesCounter > 0)
                return;

            if(stageIndex == stages.Length)
            {
                //Termina stanza e spawna l'oggetto
                //oppure spawna l'elite
                return;
            }

            StartCoroutine(PrepareNextStage());
        }

        private IEnumerator PrepareNextStage()
        {
            yield return new WaitForSeconds(stageDelay);

            SpawnEnemy();
        }
    }

}