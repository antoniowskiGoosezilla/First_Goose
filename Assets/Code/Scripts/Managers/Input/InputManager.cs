using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    //Singleton
    public static InputManager instance;

    //Action Controller
    private PlayerInputSystem inputSystem;

    //Booleana per sapere quando abilitare i controlli menu e quando quelli giocatore
    public bool inMenu;


    void OnEnable()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        //Da cambiare con lo specifico schema di comandi necessario all'avvio
        inputSystem.Enable();
        SetUpInputBehaviour();
    }


    void OnDisable()
    {
        inputSystem.Disable();
    }

    void Awake()
    {
      
        if(inputSystem == null)
            inputSystem = new PlayerInputSystem();

    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void SetUpInputBehaviour()
    {
        //Pause
        inputSystem.Pause.Button.performed += OnPauseToggle;
    }

    private void OnPauseToggle(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if(inMenu)
            inMenu = false;
        else
            inMenu = true;
    }
}
