using System;
using DefaultNamespace;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public bool UseTouch;
    
    private float _touchDuration;

    [field: SerializeField]
    public Camera MainCam { get; private set; }
    
    private void Start()
    {
        Input.multiTouchEnabled = false;
    }

    private void Update()
    {
        if (UseTouch)
        {
            TouchUpdate();
        }
        else
        {
            MouseUpdate();
        }
        
    }

    private void MouseUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TouchBegan(Input.mousePosition);
            return;
        }
        
        if (Input.GetMouseButton(0))
        {
            TouchMoved(Input.mousePosition);
            return;
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            TouchEnd(Input.mousePosition);
            return;
        }
    }

    private void TouchUpdate()
    {
        if (Input.touchCount == 0) return;
        var touch = Input.GetTouch(0);

        switch (touch.phase)
        {
            case TouchPhase.Began:
                TouchBegan(touch.position);
                break;
            case TouchPhase.Moved:
                TouchMoved(touch.position);
                break;
            case TouchPhase.Stationary:
                TouchMoved(touch.position);
                break;
            case TouchPhase.Ended:
                TouchEnd(touch.position);
                break;
            case TouchPhase.Canceled:
                break;
            default:
                break;
        }
    }
    
    private void TouchBegan(Vector3 touchPos)
    {
        _touchDuration = 0;
        SendRay(touchPos);
    }

    private void SendRay(Vector3 touchPos)
    {
        RaycastHit hit;
        var ray = MainCam.ScreenPointToRay(touchPos);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.TryGetComponent<Draggable>(out var draggable))
            {
                ObjectDragger.SetDraggedObject(draggable);
            }
        }
    }

    private void TouchMoved(Vector3 touchPos)
    {
        _touchDuration += Time.deltaTime;
        var camPos = MainCam.transform.position;
        touchPos[2] = 7.5f - camPos.z;
        var worldPos = MainCam.ScreenToWorldPoint(touchPos);

        if (ObjectDragger.CurrentDraggedObject == null)
        {
            //SendRay(touchPos);
            Debug.DrawLine(camPos,worldPos,Color.red);
        }
        else
        {
            Debug.DrawLine(camPos,worldPos,Color.green);
        }
        
        
        ObjectDragger.AddForceTowards(worldPos);
    }
    
    private void TouchEnd(Vector3 touchPos)
    {
        _touchDuration += Time.deltaTime;
        ObjectDragger.Free();
    }
}