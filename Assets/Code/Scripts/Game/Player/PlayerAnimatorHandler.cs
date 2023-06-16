using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class PlayerAnimatorHandler : MonoBehaviour
{

    //PUBLIC
    public Animator characterAnimator;

    //V = 0 IDLE
    //V = 0.5 CAMMINATA
    //V = 1 CORSA
    public void UpdateAnimatorMovementValues(float verticalMovement, float horizontalMovement)
    {
        #region Vertical
        float v;

        if(verticalMovement > 0){
            v = 1;
        }else if(verticalMovement < 0){
            v = -1;
        }else{
            v = 0;
        }
        #endregion


        #region Horizontal
        float h;
        if(horizontalMovement > 0){
            h = 1f;
        }else if (horizontalMovement < 0){
            h = -1f;
        }else{
            h = 0;
        }
        #endregion

        characterAnimator.SetFloat(vertical, v, 0.1f, Time.deltaTime);
        characterAnimator.SetFloat(horizontal, h, 0.1f, Time.deltaTime);

    }

    public void PlayAnimationTarget(string targetAnim, bool isInteracting)
    {
        characterAnimator.applyRootMotion = isInteracting;
        characterAnimator.SetBool("isInteracting", isInteracting);
        characterAnimator.CrossFade(targetAnim, 0.2f);
        
    }

    public void PlayAnimationTarget(Animation targetAnim, bool isInteracting)
    {
        characterAnimator.applyRootMotion = isInteracting;
        characterAnimator.SetBool("isInteracting", isInteracting);
        characterAnimator.CrossFade(targetAnim.name, 0.2f);
    }










    //PRIVATE

    private int vertical;
    private int horizontal;


    public void Init(){
        characterAnimator = GetComponent<Animator>();
        vertical = Animator.StringToHash("Vertical");
        horizontal = Animator.StringToHash("Horizontal");
    }
    

    private void Awake()
    {
        Init();
    }
    // Start is called before the first frame update
    public void Start()
    {
        
    }

    // Update is called once per frame
    public void Update()
    {
        
    }
}
