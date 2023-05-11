using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadCanvasHandler : MonoBehaviour
{
    //PUBLIC
    [SerializeField] GameObject quickTimeEventSlider;

    public void UpdateTargetPosition(float correctTime, float maxReloadTime)
    {
        //_start position = 0
        //_end position = maxReloadTime
        //correctTimer => _end : maxReloadTime = x_pos : correctTime =>
        input.enabled = true;
        target.enabled = true;
        target.maxValue = maxReloadTime;
        target.value = correctTime;
    }

    public void SetMaxReloadValue(float maxReloadTime)
    {
        input.maxValue = maxReloadTime;
    }
    public void UpdateQuickTimeEventReload(float currentValue)
    {
        input.value = currentValue;
    }








    //PRIVATE 
    private Camera _camera;
    private Slider target;
    private Slider input;

    private void Awake()
    {
        target = quickTimeEventSlider.transform.Find("TargetSlider").GetComponent<Slider>();
        input = quickTimeEventSlider.GetComponent<Slider>();

    }

    private void Start()
    {
        _camera = Camera.main;
        target.enabled = false;
        input.enabled = false;
        
    }

    private void Update()
    {
        AdjustCanvasPosition();
    }

    private void AdjustCanvasPosition()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - _camera.transform.position);
    }
}
