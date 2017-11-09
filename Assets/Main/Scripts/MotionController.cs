using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionController : MonoBehaviour {

    [SerializeField]
    private float movePercent;
    //2^-7
    private float tolerance = 0.0078125f;

    [SerializeField]
    private Vector3 oldMousePos;
    [SerializeField]
    private Vector3 newMousePos;
    [SerializeField]
    private Vector3 deltaMousePos;

    void Start()
    {
        oldMousePos = Vector3.zero;
        newMousePos = Vector3.zero;
        movePercent = Input.touchSupported ? 1.0f : 0.5f;
    }

	void Update ()
    {
        if(Input.touchSupported)
        {
            if(Input.touchCount == 1)
            {
                switch(Input.GetTouch(0).phase)
                {
                    case TouchPhase.Began:
                        newMousePos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                        break;
                    case TouchPhase.Moved:
                        //Vector3 deltaPos = Camera.main.ScreenToWorldPoint((Vector3)Input.GetTouch(0).deltaPosition);
                        oldMousePos.Set(newMousePos.x, newMousePos.y, newMousePos.z);
                        newMousePos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                        deltaMousePos = newMousePos - oldMousePos;
                        transform.position = Vector3.Lerp(transform.position, transform.position + deltaMousePos, movePercent);
                        break;
                    case TouchPhase.Stationary:
                        newMousePos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                        oldMousePos.Set(newMousePos.x, newMousePos.y, newMousePos.z);
                        break;
                }
            }
        }
        else
        {
            newMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //left click hold
            if (Input.GetMouseButton(0))
            {
                //apply vessel movement
                deltaMousePos = (newMousePos - oldMousePos);
                if (deltaMousePos.sqrMagnitude > tolerance)
                {
                    transform.position = Vector3.Lerp(transform.position, transform.position + deltaMousePos, movePercent);
                }
            }
            oldMousePos.Set(newMousePos.x, newMousePos.y, newMousePos.z);
        }
    }

}
