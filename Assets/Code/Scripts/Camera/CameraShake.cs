using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    //PUBLIC OR EDITOR VISIBLE
    public void SimpleShake()
    {   

        time = 1f;
        if(isShaking)
            return;
        shake = new AnimationCurve();
        shake.AddKey(0,0);
        shake.AddKey(1, 10);
        StartCoroutine(ShakeCoroutine());
    }

    public void Shake(float magnitude, float time)
    {   
        this.time = time;
        if(isShaking)
            return;
        shake = new AnimationCurve();
        shake.AddKey(0,0);
        shake.AddKey(time, magnitude);
        StartCoroutine(ShakeCoroutine());
    }



    //PRIVATE
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin _perlin;

    private float time = 1000f;     //Valore molto alto per non triggerare la routine
    private bool isShaking = false;

    private AnimationCurve shake;


    private IEnumerator ShakeCoroutine()
    {
        isShaking = true;
        while(time > 0)
        {
            time -= Time.deltaTime;
            _perlin.m_AmplitudeGain = shake.Evaluate(time);
            yield return null;
        }

        time = 0;
        _perlin.m_AmplitudeGain = 0f;
        isShaking = false;

    }

    //STANDARD
    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        _perlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

}
