using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadCanvasHandler : MonoBehaviour
{
    //PUBLIC
    [SerializeField] GameObject quickTimeEventSlider;

    public void UpdateTargetPosition(float correctTime, float maxReloadTime, float deltaReloading)
    {
        Debug.Log(deltaReloading);
        //_start position = 0
        //_end position = maxReloadTime
        //correctTimer => _end : maxReloadTime = x_pos : correctTime =>
        quickTimeEventSlider.SetActive(true);
        target.maxValue = maxReloadTime;
        target.value = correctTime;

        //sliderWidth : maxReloadTime = targetWidth : deltaReloading
        float targetWidth = reloadSliderWidth * deltaReloading / maxReloadTime;
        target.transform.Find("Image").GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, targetWidth);
        Debug.Log(target.transform.Find("Image").GetComponent<RectTransform>().sizeDelta.x);
    }

    public void SetMaxReloadValue(float maxReloadTime)
    {
        input.maxValue = maxReloadTime;
    }

    //Immediato
    public void UpdateQuickTimeEventReload(float currentValue)
    {
        input.value = currentValue;
    }

    //Smooth
    public void UpdateQuickTimeEventReload(float currentValue, float oldValue, float interval)
    {
        input.value = Mathf.Lerp(oldValue, currentValue, interval);
    }

    public void DeactivateReloadSlider()
    {
        quickTimeEventSlider.SetActive(false);
    }








    //PRIVATE 
    private Camera _camera;
    private Slider target;
    private Slider input;
    
    private float reloadSliderWidth;

    private void Awake()
    {
        target = quickTimeEventSlider.transform.Find("TargetSlider").GetComponent<Slider>();
        input = quickTimeEventSlider.GetComponent<Slider>();
        target.value = 0;
        input.value = 0;
        reloadSliderWidth =  quickTimeEventSlider.transform.parent.GetComponent<RectTransform>().sizeDelta.x; //hard coded

        quickTimeEventSlider.SetActive(false);
    }

    private void Start()
    {
        _camera = Camera.main;
        
    }

    private void LateUpdate()
    {
        AdjustCanvasPosition();
    }

    private void AdjustCanvasPosition()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - _camera.transform.position);
    }
}
