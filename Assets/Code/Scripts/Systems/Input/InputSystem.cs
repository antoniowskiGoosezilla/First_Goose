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
        private static bool isInit = false;

        private static PlayerInputSystem inputAction;       //Action Controller
        public static bool inMenu;                          //Booleana per sapere quando abilitare i controlli menu e quando quelli giocatore


        //VARIABILI PER IL MOVIMENTO
        //Sono statiche poiché il giocatore è unico e quindi non c'è bisogno
        //di avere diversi valori per gli input
        public static float horizontal;
        public static float vertical;
        public static Vector2 rawInputVector;           //Si tratta di un vettore che prende semplicemente gli input dati dal giocatore.
                                                        //Non è ancora adattato al mondo tridimensionale né ad eventuali angoli di camera
        public static Vector3 inputVector;              //Un vettore "lavorato", adattato al 3D e ad una camera angolata
        public static float inputMagnitude;


        //VARIABILI MOUSE
        public static Vector2 mouseScreenPosition;



        //Azioni che vengono triggerate in caso di pressione di tasti
        //Altri moduli possono accedervi senza avere un riferimento alla classe
        public static event Action<InputAction.CallbackContext> OnRollAction;
        public static event Action<InputAction.CallbackContext> OnShootAction;
        public static event Action<InputAction.CallbackContext> OnNextWeaponAction;
        public static event Action<InputAction.CallbackContext> OnPreviousWeaponAction;
        public static event Action<InputAction.CallbackContext> OnSpecificWeaponAction;

        
        
        
        
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

            if(isInit)
                return;
            
            isInit = true;

            if(inputAction == null)
                inputAction = new PlayerInputSystem();

            SetUpMovementInput();
            SetUpMenuNavigation();
            SetUpRollInput();
            SetUpAimInput();
            SetUpShootInput();
            SetUpChangeWeaponInput();

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
        static void SetUpMovementInput(){
            inputAction.Game.Movement.started += OnMoveInput;
            inputAction.Game.Movement.performed += OnMoveInput;
            inputAction.Game.Movement.canceled += OnMoveInput; 
        }
        static void SetUpRollInput()
        {
            inputAction.Game.Roll.performed += OnRollAction;
        }
        static void SetUpMenuNavigation()
        {
            inputAction.Menu.Navigate.started += OnMoveInMenu;
            inputAction.Menu.Navigate.performed += OnMoveInMenu;
            inputAction.Menu.Navigate.canceled += OnMoveInMenu;
        }
        static void SetUpAimInput()
        {
            inputAction.Game.Aim.performed += OnMouseMovement;
        }
        static void SetUpShootInput()
        {
            inputAction.Game.Shoot.performed += OnShootAction;
        }
        static void SetUpChangeWeaponInput()
        {
            inputAction.Game.NextWeapon.performed += OnNextWeaponAction;
            inputAction.Game.PreviousWeapon.performed += OnPreviousWeaponAction;
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
        static void OnMouseMovement(InputAction.CallbackContext context)
        {
            mouseScreenPosition = context.ReadValue<Vector2>();
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
