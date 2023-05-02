using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//Questa classe è responsabile del movimento del giocatore.
//Ottenendo i valori degli input, li elabora permettendo il movimento
//dei personaggi

namespace AntoNamespace
{
    public class PlayerLocomotionHandler : MonoBehaviour
    {

        //Components
        CharacterController characterController;
        
        //Movement
        [SerializeField] AnimationCurve speedCurve;             //Al posto di classiche variabili, per il movimento sono state utilizzate
                                                                //delle AnimationCurve. La velocità viene definita in funzione del tempo 
                                                                //di movimento. In questo modo possiamo definire in modo semplice un tipo
                                                                //di velocità direttamente dall'Inspector di Unity
        float movementTimer;
        Vector2 velocity;
        float verticalVelocity;
        
        //Roll
        [SerializeField] AnimationCurve rollSpeedCurve;         //Il roll ha subito lo stesso trattamento della velocità di movimento
        

        void Awake()
        {
            Init();
        }

        void Start()
        {
            movementTimer = 0;            
        }

        void Update()
        {
            float delta = Time.deltaTime;
            MovePlayer(delta);
        }

        void Init()
        {
            characterController = GetComponent<CharacterController>();
            InputCustomSystem.OnRollAction += TriggerRoll;
        }

        #region MOVEMENT FUNCTIONS
        void MovePlayer(float delta)
        {
            if(InputCustomSystem.isInteracting)
                return;

            if(InputCustomSystem.inputMagnitude != 0)
            {
                movementTimer += delta;
                velocity = new Vector2(InputCustomSystem.horizontal, InputCustomSystem.vertical)*speedCurve.Evaluate(movementTimer);
                HandlePlayerOrientation(delta);
            }
            else
            {
                movementTimer = 0;
                velocity = Vector2.MoveTowards(velocity, Vector2.zero, 10f);
            }

            Vector3 movement = new Vector3(velocity.x, 0, velocity.y);
            characterController.Move(movement*delta);
        }
        void HandlePlayerOrientation(float delta)
        {
            Quaternion toRotation = Quaternion.LookRotation(new Vector3(InputCustomSystem.horizontal, 0, InputCustomSystem.vertical), Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, 15f*delta);
        }
        #endregion

        #region ROLL
        void TriggerRoll(InputAction.CallbackContext context)
        {
            if(InputCustomSystem.isInteracting)
                return;
            StartCoroutine(Roll());
        }

        IEnumerator Roll(){
            //animatorHandler.PlayAnimationTarget("RollForward", true);
            InputCustomSystem.isInteracting = true;
            float timer = 0.8f;                 //DURATA POCO PIU PICCOLA DELL'ANIMAZIONE ANIMAZIONE
            float rollTime = 0f;
            float rollSpeed = rollSpeedCurve.Evaluate(rollTime);
            Vector3 lookDirection = new Vector3(InputCustomSystem.horizontal, 0, InputCustomSystem.vertical);

            while(timer > 0){
                //Dodge direction
                Quaternion toRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, toRotation,1);

                timer -= Time.deltaTime;
                rollTime += Time.deltaTime;
                rollSpeed = rollSpeedCurve.Evaluate(rollTime);
                characterController.Move(new Vector3(lookDirection.x, 0, lookDirection.z)*Time.deltaTime*rollSpeed);
                //animatorHandler.UpdateAnimatorMovementValues(0,0,InputCustomSystem.runFlag);
                yield return null;
            }       
            InputCustomSystem.isInteracting = false;
        }
        #endregion


    }

}