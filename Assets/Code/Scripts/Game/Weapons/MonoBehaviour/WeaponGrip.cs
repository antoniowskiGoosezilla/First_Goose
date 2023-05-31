using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGrip : MonoBehaviour
{
    //Si tratta delle due posizioni dove il modello del personaggio
    //poggerà le mani
    [SerializeField] Transform mainGrip;
    [SerializeField] Transform secondGrip;

    public Transform GetMainGrip()
    {
        return mainGrip;
    }

    public Transform GetSecondGrip()
    {
        return secondGrip;
    }
}
