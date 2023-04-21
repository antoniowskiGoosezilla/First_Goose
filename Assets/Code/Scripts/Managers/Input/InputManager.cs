using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    //Singleton
    private static InputManager instance;

    //Action Controller
    private PlayerInputSystem inputSystem;

    //Booleana per sapere quando abilitare i controlli menu e quando quelli giocatore
    public bool inMenu;


    //Variabili per il movimento
    public float horizontal;
    public float vertical;
    public Vector2 rawInputVector;             //Si tratta di un vettore che prende semplicemente gli input dati dal giocatore.
                                                //Non è ancora adattato al mondo tridimensionale né ad eventuali angoli di camera
    public Vector3 inputVector;                 //Un vettore "lavorato", adattato al 3D e ad una camera angolata
    public float inputMagnitude;



    //VARAIBILE PER IL CHECK DELLE INTERAZIONI
    //durante le interazioni il giocatore non deve potersi muovere
    public bool isInteracting;


    void OnEnable()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        if(inputSystem == null)
            inputSystem = new PlayerInputSystem();

        SetupMovementInput();
        SetUpMenuNavigation();

        inputSystem.Pause.Button.performed += OnPauseToggle;
        inputSystem.Pause.Enable();
        //Da cambiare con lo specifico schema di comandi necessario all'avvio
        inputSystem.Game.Enable();
    }


    void OnDisable()
    {
        inputSystem.Disable();
    }

    void Awake(){}

    void Start(){}

    void Update(){}

    #region SET UP FUNCTIONS
    void SetupMovementInput(){
        inputSystem.Game.Movement.started += OnMoveInput;
        inputSystem.Game.Movement.performed += OnMoveInput;
        inputSystem.Game.Movement.canceled += OnMoveInput; 
    }

    

    void SetUpMenuNavigation()
    {
        inputSystem.Menu.Navigate.started += OnMoveInMenu;
        inputSystem.Menu.Navigate.performed += OnMoveInMenu;
        inputSystem.Menu.Navigate.canceled += OnMoveInMenu;
    }

    #endregion

    void OnMoveInput(InputAction.CallbackContext context)
    {
        rawInputVector = context.ReadValue<Vector2>();
        inputVector = AdaptInputDirection(cameraDegreesAngle: 45);

        horizontal = inputVector.x;
        vertical = inputVector.z;
        inputMagnitude = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
    }
    void OnMoveInMenu(InputAction.CallbackContext context)
    {
        rawInputVector = context.ReadValue<Vector2>();
    }
    private void OnPauseToggle(InputAction.CallbackContext context)
    {
        if(inMenu)
        {
            inMenu = false;
            inputSystem.Game.Enable();
            inputSystem.Menu.Disable();
        }
        else
        {
            inMenu = true;
            inputSystem.Game.Disable();
            inputSystem.Menu.Enable();
        }
            
   }

    //Ruota gli input del controller in modo che siano adattati alla visuale
    //di gioco
    private Vector3 AdaptInputDirection(float cameraDegreesAngle)
    {
        Vector3 newVector;
        newVector = new Vector3(rawInputVector.x, 0, rawInputVector.y);
        newVector = Quaternion.Euler(0,cameraDegreesAngle,0)*newVector;
        newVector = newVector.normalized;
        return newVector;
    }
}
