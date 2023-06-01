using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.EventSystems;

//Classe che gestisce la scena del menu principale
public class MainMenuManager : MonoBehaviour
{
    [Header("Lights")]
    [SerializeField] GameObject _lights;
    [Space]
    [Header("Title")]
    [SerializeField] GameObject title;
    [SerializeField] GameObject whiteTitle;
    [Space]
    [Header("Buttons And Selector")]
    [SerializeField] GameObject playBtn;
    [SerializeField] GameObject settingsBtn;
    [SerializeField] GameObject quitBtn;
    [Header("Selector")]
    [SerializeField] GameObject selector;
    [Header("Event System")]
    [SerializeField] EventSystem eventSystem;

    
    
    
    //PRIVATE
    private string dateTime = DateTime.Now.ToString();
    private int dateTimeHour = DateTime.Now.Hour;
    private float xLightRotation;
    private float yLightRotation = -40;

    private void Start()
    {
        //SetSceneTime();
        selector.transform.position = playBtn.transform.Find("SelectorPosition").position;
        eventSystem.SetSelectedGameObject(eventSystem.firstSelectedGameObject);
    }

    
    private void Update()
    {
        
    }

    private void LateUpdate()
    {
        //Una merda, MA per ora funziona
        if(selector.transform.position != eventSystem.currentSelectedGameObject.transform.Find("SelectorPosition").position)
        {
            selector.transform.position = Vector3.Lerp(selector.transform.position, eventSystem.currentSelectedGameObject.transform.Find("SelectorPosition").position, .1f);
        }
    }


    private void SetSceneTime()
    {
        //Intervalli plausibili
        //20:00 - 05:59 --- Notte
        //06:00 - 10:59 --- Mattina
        //11:00 - 16:59 --- Pieno Giorno
        //17:00 - 19:59 --- Pomeriggio/Quasi sera
        GameObject mainLight = _lights.transform.Find("Sun Light").gameObject;

        if(mainLight == null)
            return;
        
        if(!dateTime.Contains("A.M.") && !dateTime.Contains("P.M"))
        {
            //Sistema orario a 24H
            switch(dateTimeHour)
            {
                case var expression when(dateTimeHour >= 6 && dateTimeHour < 11):
                    xLightRotation = 180;
                    SetTextToBlack();
                    break;
                case var expression when(dateTimeHour >= 11 && dateTimeHour < 17):
                    xLightRotation = 79;
                    SetTextToBlack();
                    break;
                case var expression when(dateTimeHour >= 17 && dateTimeHour < 20):
                    print("enter");
                    xLightRotation = 0;
                    SetTextToBlack();
                    break;
                case var expression when(dateTimeHour >= 20 || dateTimeHour < 6):
                    xLightRotation = -50;
                    SetTextToWhite();
                    break;
                default:
                    break;
            }
        }
        else
        {
            //Orario a 12H
            if(dateTime.Contains("A.M."))
            {
                switch(dateTimeHour)
                {
                    case var expression when(dateTimeHour >= 6 && dateTimeHour < 11):
                        xLightRotation = 180;
                        SetTextToBlack();
                        break;
                    case var expression when(dateTimeHour >= 11):
                        xLightRotation = 79;
                        SetTextToBlack();
                        break;
                    case var expression when(dateTimeHour >= 0 && dateTimeHour < 6):
                        xLightRotation = -50;
                        SetTextToWhite();
                        break;
                    default:
                        break;
                }   
            }
            else
            {
                switch(dateTimeHour)
                {
                    case var expression when(dateTimeHour >= 1 && dateTimeHour < 5):
                        xLightRotation = 79;
                        SetTextToBlack();
                        break;
                    case var expression when(dateTimeHour >= 5 && dateTimeHour < 8):
                        xLightRotation = 0;
                        SetTextToBlack();
                        break;
                    case var expression when(dateTimeHour >= 8):
                        xLightRotation = -50;
                        SetTextToWhite();
                        break;
                    default:
                        break;
                }
            }
        }
        mainLight.transform.rotation = Quaternion.Euler(xLightRotation, yLightRotation, 0);
    }

    private void SetTextToWhite()
    {
        if(title.activeInHierarchy == false)
            return;

        title.SetActive(false);
        whiteTitle.SetActive(true);
        playBtn.GetComponentInChildren<TMP_Text>().color = Color.white;
        settingsBtn.GetComponentInChildren<TMP_Text>().color = Color.white;
        quitBtn.GetComponentInChildren<TMP_Text>().color = Color.white;
    }
    private void SetTextToBlack()
    {
        if(title.activeInHierarchy == true)
            return;

        title.SetActive(true);
        whiteTitle.SetActive(false);
        playBtn.GetComponent<TMP_Text>().color = new Color(75,75,75,1);
        settingsBtn.GetComponent<TMP_Text>().color = new Color(75,75,75,1);
        quitBtn.GetComponent<TMP_Text>().color = new Color(75,75,75,1);
    }
}
