using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        [SerializeField] AnimationCurve speedCurve;
        float movementTimer;
        Vector2 velocity;
        float verticalVelocity;

        //Roll
        [SerializeField] AnimationCurve rollSpeedCurve;
        

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
            InputSystem.OnRollAction += TriggerRoll;
        }

        #region MOVEMENT FUNCTIONS
        void MovePlayer(float delta)
        {
            if(InputSystem.inputMagnitude != 0)
            {
                movementTimer += delta;
                velocity = new Vector2(InputSystem.horizontal, InputSystem.vertical)*speedCurve.Evaluate(movementTimer);
                HandlePlayerOrientation(delta);
            }
            else
            {
                velocity = Vector2.MoveTowards(velocity, Vector2.zero, 10f);
            }

            Vector3 movement = new Vector3(velocity.x, 0, velocity.y);
            characterController.Move(movement*delta);
        }
        void HandlePlayerOrientation(float delta)
        {
            Quaternion toRotation = Quaternion.LookRotation(new Vector3(InputSystem.horizontal, 0, InputSystem.vertical), Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, 15f*delta);
        }
        #endregion

        #region ROLL
        void TriggerRoll()
        {
            StartCoroutine(Roll());
        }

        IEnumerator Roll(){
            //animatorHandler.PlayAnimationTarget("RollForward", true);
            float timer = 0.8f; //DURATA POCO PIU PICCOLA DELL'ANIMAZIONE ANIMAZIONE
            float rollTime = 0f;
            float rollSpeed = rollSpeedCurve.Evaluate(rollTime);
            Vector3 lookDirection = new Vector3(InputSystem.horizontal, 0, InputSystem.vertical);

            while(timer > 0){
                //Dodge direction
                Quaternion toRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, toRotation,1);

                timer -= Time.deltaTime;
                rollTime += Time.deltaTime;
                rollSpeed = rollSpeedCurve.Evaluate(rollTime);
                characterController.Move(new Vector3(InputSystem.horizontal, 0, InputSystem.vertical)*Time.deltaTime*rollSpeed);
                //animatorHandler.UpdateAnimatorMovementValues(0,0,InputSystem.runFlag);
                yield return null;
            }       

        }
        #endregion


    }

}