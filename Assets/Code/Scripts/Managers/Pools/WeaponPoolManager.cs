using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPoolManager : MonoBehaviour
{

    private WeaponPoolManager instance;

    [SerializeField]
    private GameObject[] pool;

    void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
