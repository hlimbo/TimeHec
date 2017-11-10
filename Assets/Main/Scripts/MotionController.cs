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
        // movePercent = Input.touchSupported ? 0.01f : 0.5f;
        movePercent = 0.5f;
    }

	void Update ()
    {
        Movement3();
    }

    //move based on the direction of mouse swipe
    void MouseMovement1()
    {
        newMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //left click hold
        if (Input.GetMouseButton(0))
        {
            //move ship towards finger position (add some offset here...)
            deltaMousePos = (newMousePos - oldMousePos);
            if (deltaMousePos.sqrMagnitude > tolerance)
            {
                transform.position = Vector3.Lerp(transform.position, transform.position + deltaMousePos, movePercent);
            }
        }
        oldMousePos.Set(newMousePos.x, newMousePos.y, newMousePos.z);
    }

    //move based on the direction of finger swipe
    void TouchMovement1()
    {
        if (Input.touchCount == 1)
        {
            switch (Input.GetTouch(0).phase)
            {
                //movePercent needs to be 0.01 here
                case TouchPhase.Moved:
                    //move ship in the direction of finger swipe
                    deltaMousePos = Input.GetTouch(0).deltaPosition;
                    motionSpeed = deltaMousePos.magnitude / Input.GetTouch(0).deltaTime;
                    moveVelocity = motionSpeed * deltaMousePos.normalized * Time.deltaTime * movePercent;
                    transform.position += moveVelocity;
                    break;
            }
        }
    }

    float Hermite(float v)
    {
        return v * v * (3.0f - 2.0f * v);
    }
    void Movement3()
    {
        //follow where mouse cursor points towards with some ~ NO SMOOTHING when clicking far away from player vessel
        //offset so the vessel isn't directly on top of mouse cursor / finger
        //snappy movement
        if (Input.GetMouseButton(0))
        {
            Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 lockPosition = transform.TransformPoint(Vector3.down);// 1 unity unit below triangle
            lockPosition = Vector3.Lerp(lockPosition, targetPos, movePercent);
            lockPosition.z = 0.0f;
            transform.position = lockPosition + Vector3.up;
        }
    }

}
