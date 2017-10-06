using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ArrowController : MonoBehaviour {

    //arrow will be unaffected by timeScale!

    //measured in unity units per second
    //unity units depends on how many pixels is one unity unit for this particular game object
    public float speed;
    private Vector2 direction;
    [SerializeField]
    private Vector2 velocity;
    private Rigidbody2D rb;
    public float destructionDelay = 2.0f;

    //arrow needs to know about the targets player controller selected
    private PlayerController pController;

    private float oldTimeScale;

    //will probably would want to use quaternions to rotate the arrow
    [SerializeField]
    private GameObject targetToRemove;
    [SerializeField]
    private bool areTargetsAvailable = false;

    [SerializeField]
    private Vector2 originalPos;

    private bool canDestroy = false;

    private void Awake()
    {
        pController = FindObjectOfType<PlayerController>();
        originalPos = transform.position;
    }

    // Use this for initialization
    void Start () {

        speed = 300;
        oldTimeScale = Time.timeScale;
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {

        //remove arrow when time is speeding back up
        if(Time.timeScale > oldTimeScale)
        {
            //destroy the arrow game object if no targets were seeked
            if (pController.targetList.Count == 0)
            {
                Debug.Log("list is empty");
                Destroy(this.gameObject);
            }
            else
                areTargetsAvailable = true;

            oldTimeScale = Time.timeScale;
        }

        //if time is speeding back up seek targets to destroy
        if (pController.targetList.Count != 0)
        {
            //arrow needs to home towards target!
            targetToRemove = pController.targetList[0];

            Vector2 targetDiff = targetToRemove.transform.position - this.transform.position;
            float angle = Mathf.Atan2(targetDiff.y, targetDiff.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * speed);

            direction = targetDiff.normalized;
            velocity = speed * direction;
            rb.velocity = velocity * Time.deltaTime;
        }
        else if(areTargetsAvailable) //if the targets were previously available, but are now all eliminated..
        {

            TrailRenderer trail = transform.GetComponentInChildren<TrailRenderer>();
            trail.enabled = false;
            //return arrow back to original position and get destroyed.
           transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, Time.deltaTime * speed);

            float turnaroundSpeed = 10.0f;
            transform.position = Vector2.MoveTowards(transform.position, originalPos, turnaroundSpeed * Time.deltaTime);

            if ((Vector2)transform.position == originalPos)
            {
                Debug.Log("arrived!");
                areTargetsAvailable = false;
                rb.velocity = Vector2.zero;
                if (!canDestroy)
                    StartCoroutine(DestroyDelay());
            }
        }

    }

    IEnumerator DestroyDelay()
    {
        canDestroy = true;
        yield return new WaitForSecondsRealtime(destructionDelay);
        Destroy(this.gameObject);
        canDestroy = false;
        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //switch target as long as targetList is not empty
        if(targetToRemove == collision.gameObject)
        {
            pController.targetList.RemoveAt(0);
            GameObject.Destroy(targetToRemove);
        }
    }
}
