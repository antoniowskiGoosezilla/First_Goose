using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadCanvasHandler : MonoBehaviour
{
    //PUBLIC
    [SerializeField] GameObject target;
    [SerializeField] GameObject selector;
    [Space]
    [Header("REFERENCES")]
    [SerializeField] GameObject _start;
    [SerializeField] GameObject _end;


    public void UpdateTargetPosition(float correctTime, float maxReloadTime)
    {
        //_start position = 0
        //_end position = maxReloadTime
        //correctTimer => _end : maxReloadTime = x_pos : correctTime =>
        Debug.Log("Media: "+correctTime); 
        float x_pos = correctTime*_end.transform.position.x / maxReloadTime;
        target.SetActive(true);
        target.transform.position = new Vector3(x_pos, target.transform.position.y, target.transform.position.z);
    }

    //PRIVATE 
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
        target.SetActive(false);
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
