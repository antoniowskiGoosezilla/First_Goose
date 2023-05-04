using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunManager : MonoBehaviour
{
    //PUBLIC
    public float timer;
    public int shards;
    public GameObject character;
    //public RoomManager currentRoom;
    public uint roomIndex;
    public int currentRoomGrade;
    public int[] roomGrades;
    public int missedShots;
    public int totalShots;
     
    public bool shopToken;


    //PRIVATE
    //SINGLETON
    private RunManager instance;

    private int floor;
    private int difficulty;

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    private void Init()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        GameFrameworkManager.OnCharacterChange += ChangeCharacter;
    }

    private void ChangeCharacter(GameObject newCharacter)
    {
        character = newCharacter;
    }

    private void GenerateRoom(int difficulty, int floor)
    {
        //TODO:
        //  Aggiungere la funzione che selezionerà la stanza 
        //  al caricamento del nuovo "livello"  
    }
}
