using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AntoNamespace;
using UnityEngine.UI;   
using UnityEngine.InputSystem;


public class DialogueManager : MonoBehaviour
{
    //PUBLIC
    [SerializeField] GameObject dialogueCanvas;
    [SerializeField] AnimationCurve fadeAnimationCurve;
    [SerializeField] float backgroundOpacity;


    //PRIVATE 
    private GameObject background;
    private GameObject textBox;
    private Vector3 characterPosition;
    private Vector3 textBoxPosition;

    private bool isActive = false;                 //Dialogo attivo

    private Coroutine fadeAnimation;
    private float fadeTimer;

    private void Awake()
    {
        if(dialogueCanvas == null)
            dialogueCanvas = this.gameObject;
        //Un po' forzato ma chissene
        background = dialogueCanvas.transform.Find("Black").gameObject;
        textBox = dialogueCanvas.transform.Find("Textbox").gameObject;
        textBoxPosition = dialogueCanvas.transform.Find("textBoxPosition").position;
        characterPosition = dialogueCanvas.transform.Find("CharacterSprite").position;

        textBox.SetActive(false);
        //Eventi
        InputCustomSystem.OnTest += OnTestDialogue;
    }

    private void Start()
    {
        
    }

    // Update is called once per frame
    private void LateUpdate()
    {
    }

    private void OnTestDialogue(InputAction.CallbackContext context)
    {
        Debug.Log("Start");
        if(!isActive)
        {
            isActive = true;
            if(fadeAnimation != null)
                StopCoroutine(fadeAnimation);

            fadeAnimation = StartCoroutine("FadeIn");
            return;
        }


        isActive = false;
        if(fadeAnimation != null)
            StopCoroutine(fadeAnimation);

        fadeAnimation = StartCoroutine("FadeOut");
    }


    //ANIMAZIONI
    private IEnumerator FadeIn()
    {
        Color color = background.GetComponent<Image>().color;
        fadeTimer = 0;
        while(background.GetComponent<Image>().color.a < backgroundOpacity)
        {
            fadeTimer += Time.deltaTime;
            float alpha = fadeAnimationCurve.Evaluate(fadeTimer);
            background.GetComponent<Image>().color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }
        StartCoroutine(MoveInTextbox());
        fadeAnimation = null;
    }

    private IEnumerator FadeOut()
    {
        Color color = background.GetComponent<Image>().color;
        fadeTimer = fadeAnimationCurve.keys[fadeAnimationCurve.keys.Length - 1].time;
        while(background.GetComponent<Image>().color.a > 0)
        {
            fadeTimer -= Time.deltaTime;
            float alpha = fadeAnimationCurve.Evaluate(fadeTimer);
            background.GetComponent<Image>().color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }
        fadeAnimation = null;
    }

    private IEnumerator MoveInTextbox()
    {
        Debug.Log("Muovo");
        textBox.SetActive(true);
        while(textBox.transform.position.y != textBoxPosition.y)
        {
            textBox.transform.position = new Vector3(textBoxPosition.x,Mathf.Lerp(textBox.transform.position.y, textBoxPosition.y, Time.deltaTime*2f), textBoxPosition.z);
            yield return null;
        }
    }
}
