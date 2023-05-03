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
    public int currentRoomGrade;
    public int[] roomGrades;
    public int missedShots;
    public int totalShots;
     

    //PRIVATE
    //SINGLETON
    private RunManager instance;

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
    }
}
