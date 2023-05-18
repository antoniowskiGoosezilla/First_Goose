using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Classe che gestisce la scena del menu principale
public class MainMenuManager : MonoBehaviour
{
    [Header("Lights")]
    [SerializeField] GameObject _lights;

    
    
    
    //PRIVATE
    private string dateTime = DateTime.Now.ToString();
    private int dateTimeHour = DateTime.Now.Hour;
    private float xLightRotation;
    private float yLightRotation = -40;

    private void Start()
    {
        SetSceneTime();
    }

    
    private void Update()
    {
        
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
                    break;
                case var expression when(dateTimeHour >= 11 && dateTimeHour < 17):
                    xLightRotation = 79;
                    break;
                case var expression when(dateTimeHour >= 17 && dateTimeHour < 20):
                    print("enter");
                    xLightRotation = 0;
                    break;
                case var expression when(dateTimeHour >= 20 || dateTimeHour < 6):
                    xLightRotation = -50;
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
                        break;
                    case var expression when(dateTimeHour >= 11):
                        xLightRotation = 79;
                        break;
                    case var expression when(dateTimeHour >= 0 && dateTimeHour < 6):
                        xLightRotation = -50;
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
                        break;
                    case var expression when(dateTimeHour >= 5 && dateTimeHour < 8):
                        xLightRotation = 0;
                        break;
                    case var expression when(dateTimeHour >= 8):
                        xLightRotation = -50;
                        break;
                    default:
                        break;
                }
            }
        }
        mainLight.transform.rotation = Quaternion.Euler(xLightRotation, yLightRotation, 0);
    }
}
