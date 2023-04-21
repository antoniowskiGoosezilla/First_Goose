using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AntoNamespace
{
    public static class InputSystem
    {
        //Singleton
        //private static InputManager instance;

        //Action Controller
        private static PlayerInputSystem inputAction;

        //Booleana per sapere quando abilitare i controlli menu e quando quelli giocatore
        public static bool inMenu;


        //Variabili per il movimento
        //Sono statiche poiché il giocatore è unico e quindi non c'è bisogno
        //di avere diversi valori per gli input
        public static float horizontal;
        public static float vertical;
        public static Vector2 rawInputVector;           //Si tratta di un vettore che prende semplicemente gli input dati dal giocatore.
                                                        //Non è ancora adattato al mondo tridimensionale né ad eventuali angoli di camera
        public static Vector3 inputVector;              //Un vettore "lavorato", adattato al 3D e ad una camera angolata
        public static float inputMagnitude;

        //Azioni che vengono triggerate in caso di pressione di tasti
        //Altri moduli possono accedervi senza avere un riferimento alla classe
        public static event Action OnRollAction;


        //VARAIBILE PER IL CHECK DELLE INTERAZIONI
        //durante le interazioni il giocatore non deve potersi muovere
        public static bool isInteracting;


        public static void Init()
        {
           /*if(instance != null)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
            DontDestroyOnLoad(gameObject);*/

            if(inputAction == null)
                inputAction = new PlayerInputSystem();

            SetupMovementInput();
            SetUpMenuNavigation();
            SetupRollInput();

            inputAction.Pause.Button.performed += OnPauseToggle;
            inputAction.Pause.Enable();
            //Da cambiare con lo specifico schema di comandi necessario all'avvio
            inputAction.Game.Enable();
        }


        /*void OnDisable()
        {
            inputAction.Disable();
        }

        void Awake(){}

        void Start(){}

        void Update(){}*/

        #region SET UP FUNCTIONS
        static void SetupMovementInput(){
            inputAction.Game.Movement.started += OnMoveInput;
            inputAction.Game.Movement.performed += OnMoveInput;
            inputAction.Game.Movement.canceled += OnMoveInput; 
        }
        static void SetupRollInput()
        {
            inputAction.Game.Roll.performed += OnRollInput;
        }
        

        static void SetUpMenuNavigation()
        {
            inputAction.Menu.Navigate.started += OnMoveInMenu;
            inputAction.Menu.Navigate.performed += OnMoveInMenu;
            inputAction.Menu.Navigate.canceled += OnMoveInMenu;
        }

        #endregion

        static void OnMoveInput(InputAction.CallbackContext context)
        {
            rawInputVector = context.ReadValue<Vector2>();
            inputVector = AdaptInputDirection(cameraDegreesAngle: 45);

            horizontal = inputVector.x;
            vertical = inputVector.z;
            inputMagnitude = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        }
        static void OnMoveInMenu(InputAction.CallbackContext context)
        {
            rawInputVector = context.ReadValue<Vector2>();
        }
        static void OnRollInput(InputAction.CallbackContext context)
        {
            OnRollAction.Invoke();
        }
        static void OnPauseToggle(InputAction.CallbackContext context)
        {
            if(inMenu)
            {
                inMenu = false;
                inputAction.Game.Enable();
                inputAction.Menu.Disable();
            }
            else
            {
                inMenu = true;
                inputAction.Game.Disable();
                inputAction.Menu.Enable();
            }
                
    }

        //Ruota gli input del controller in modo che siano adattati alla visuale
        //di gioco
        static Vector3 AdaptInputDirection(float cameraDegreesAngle)
        {
            Vector3 newVector;
            newVector = new Vector3(rawInputVector.x, 0, rawInputVector.y);
            newVector = Quaternion.Euler(0,cameraDegreesAngle,0)*newVector;
            newVector = newVector.normalized;
            return newVector;
        }
    }

}
