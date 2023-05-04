using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameFrameworkManager : MonoBehaviour
{
    //PUBLIC
    public int week {get; private set;}         //Un valore che mostra il numero di run fatte, come se si trattasse
                                                //di numero di partecipazioni allo show
    public int saloonCoin {get; private set;}
    public GameObject character;

    public static event Action<GameObject> OnCharacterChange;

    public void AddSaloonCoins(int value)
    {
        saloonCoin += value;
    }
    public void RemoveSaloonCoins(int value)
    {
        saloonCoin -= value;

        if(saloonCoin < 0)                      //Non DEVE accadere
            saloonCoin = 0;
    }

    //PRIVATE
    private GameFrameworkManager instance;

    private SaveData save;

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

        RestoreGameData();
    }

    private void RestoreGameData()
    {
        save = SavingSystem.Load();

        week = save.week;
        saloonCoin = save.saloonCoin;
    }

    private void ChangeCharacter(GameObject newCharacter)
    {
        //Funzione per il cambio dei personaggi


        OnCharacterChange?.Invoke(newCharacter);
    }

    private void GoNextWeek()
    {
        week += 1;
    }
}
