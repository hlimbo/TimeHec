using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controller moves towards the direction of where the user swipes his/her
//finger on the phone..
//This attempts to resolve the issue of not being able to see the player vessel
//due to finger being on top of player vessel.
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

    //debug stuffz
    public float motionSpeed;
    public Vector3 moveVelocity;

    void Start()
    {
        motionSpeed = 0.0f;
        moveVelocity = Vector3.zero;

        oldMousePos = Vector3.zero;
        newMousePos = Vector3.zero;
        movePercent = Input.touchSupported ? 0.01f : 0.5f;
    }

	void Update ()
    {
        if(Input.touchSupported)
        {
            if(Input.touchCount == 1)
            {
                switch(Input.GetTouch(0).phase)
                {
                    case TouchPhase.Moved:
                        deltaMousePos = Input.GetTouch(0).deltaPosition;
                        motionSpeed = deltaMousePos.magnitude / Input.GetTouch(0).deltaTime;
                        moveVelocity = motionSpeed * deltaMousePos.normalized * Time.deltaTime * movePercent;
                        transform.position += moveVelocity;
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
