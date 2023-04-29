using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour
{
    [Header("ELEMENTI UI")]
    [Space]
    [SerializeField] GameObject blackScreen;
    [SerializeField] AudioSource audioSource;

    [Header("SETTINGS ANIMATION")]
    [SerializeField] float delayStartAnimation_seconds;
    [SerializeField] float delaySound_seconds;
    [SerializeField] float delaySplashScreenEnd_seconds;
    [SerializeField] AnimationCurve splashAnimation;
    private float animationTimer;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(BeginAnimation());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator BeginAnimation()
    {
        float alpha = 1;
        yield return new WaitForSecondsRealtime(delayStartAnimation_seconds);

        while(alpha > 0)
        {
            animationTimer += Time.deltaTime;
            alpha = splashAnimation.Evaluate(animationTimer);
            blackScreen.GetComponent<Image>().color = new Color(0,0,0, alpha);
            yield return null;
        }

        yield return new WaitForSecondsRealtime(delaySound_seconds);

        audioSource.Play();

        yield return new WaitForSecondsRealtime(delaySplashScreenEnd_seconds);

        while(alpha < 1)
        {
            animationTimer += Time.deltaTime;
            alpha = splashAnimation.Evaluate(animationTimer);
            blackScreen.GetComponent<Image>().color = new Color(0,0,0, alpha);
            yield return null;
        }
    }

    
}
