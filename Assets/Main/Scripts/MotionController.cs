using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controller moves towards the direction of where the user swipes his/her
//finger on the phone..
//This attempts to resolve the issue of not being able to see the player vessel
//due to finger being on top of player vessel.
public class MotionController : MonoBehaviour {

    public float movePercent;
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

    //positions for touch are relative to the device's screen resolution (e.g. 1080x1920)
    private Vector2 oldTouchPos;
    private Vector2 newTouchPos;

    private MotionDebug motionDebug;
    private CooldownTimer cd;

    void Awake()
    {
        motionDebug = FindObjectOfType<MotionDebug>();
        cd = GetComponent<CooldownTimer>();
    }

    void Start()
    {
        oldTouchPos = Vector2.zero;
        motionSpeed = 0.0f;
        moveVelocity = Vector3.zero;

        oldMousePos = Vector3.zero;
        newMousePos = Vector3.zero;
        // movePercent = Input.touchSupported ? 0.01f : 0.5f;
       // movePercent = 0.5f;
    }

	void Update ()
    {
        movePercent = motionDebug.percent;

        //free movement
        //TouchMovement1();
        //ShieldAbility();

        //tight movement
        TouchMovement2();
        ShieldAbility();

        //if (Input.touchSupported)
        //    TouchMovement2();
        //else
        //    MouseMovement2();
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
            //movePercent = 0.01f;
            switch (Input.GetTouch(0).phase)
            {
                //movePercent needs to be 0.01 here
                case TouchPhase.Moved:
                    //move ship in the direction of finger swipe
                    deltaMousePos = Input.GetTouch(0).deltaPosition;
                    motionSpeed = deltaMousePos.magnitude / Input.GetTouch(0).deltaTime;//finger motion speed e.g. how fast or slow finger swipes on screen
                    moveVelocity = motionSpeed * deltaMousePos.normalized * Time.deltaTime * movePercent;
                    moveVelocity.z = 0.0f;
                    transform.position += moveVelocity;
                    break;
            }
        }
    }

    float Hermite(float v)
    {
        return v * v * (3.0f - 2.0f * v);
    }
    void MouseMovement2()
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

    void TouchMovement2()
    {
        //1 finger touch ~ movement
        if(Input.touchCount == 1)
        {
           // movePercent = 0.15f;
            Touch oneTouch = Input.GetTouch(0);
            Vector3 targetPos = Camera.main.ScreenToWorldPoint(oneTouch.position);
            if ((targetPos - transform.position).sqrMagnitude > tolerance)
            {
                switch (oneTouch.phase)
                {
                    case TouchPhase.Moved:
                    case TouchPhase.Stationary:
                        Vector3 lockPosition = transform.TransformPoint(Vector3.down);
                        lockPosition = Vector3.Lerp(lockPosition, targetPos, movePercent);
                        lockPosition.z = 0.0f;
                        transform.position = lockPosition + Vector3.up;
                        break;
                }
            }

        }
    }

    void ShieldAbility()
    {
        if (Input.touchCount == 2) // shield ability ~ for now tapping 2 fingers on screen will activate shield
        {
            bool firstFingerOnScreen = Input.GetTouch(0).phase == TouchPhase.Began ||
                Input.GetTouch(0).phase == TouchPhase.Stationary ||
                Input.GetTouch(0).phase == TouchPhase.Moved;
            if (!cd.isOnCooldown && firstFingerOnScreen && Input.GetTouch(1).phase == TouchPhase.Began)
            {
                print("shield entering cooldown");
                cd.isOnCooldown = true;
                transform.GetChild(0).gameObject.SetActive(true);
            }

        }

        //if not on shield isn't on cooldown anymore disable its gameobject
        if (!cd.isOnCooldown)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }

}
