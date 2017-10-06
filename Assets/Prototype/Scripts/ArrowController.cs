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
        originalPos = transform.position;
    }

    // Use this for initialization
    void Start () {

        speed = 300;
        oldTimeScale = Time.timeScale;
        pController = FindObjectOfType<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {

        if(Time.timeScale > oldTimeScale)
        {
            //destroy the arrow game object if no targets were seeked
            if (pController.targetList.Count == 0)
                Destroy(this.gameObject);
            else
                areTargetsAvailable = true;

            oldTimeScale = Time.timeScale;
        }

        //if time is speeding back up seek targets to destroy
        if (pController.targetList.Count != 0)
        {
            //arrow needs to home towards target!
            targetToRemove = pController.targetList[0];
            direction = (targetToRemove.transform.position - this.transform.position).normalized;
            velocity = speed * direction;
            rb.velocity = velocity * Time.deltaTime;
        }
        else if(areTargetsAvailable) //if the targets were previously available, but are now all eliminated..
        {
            //return arrow back to original position and get destroyed.
            float turnaroundSpeed = 10.0f;
            rb.position = Vector2.MoveTowards(rb.position, originalPos, turnaroundSpeed * Time.deltaTime);
            if (rb.position == originalPos)
            {
                Debug.Log("arrived!");
                areTargetsAvailable = false;
                rb.velocity = Vector2.zero;
                if (!canDestroy)
                    StartCoroutine(DestroyDelay());
            }
            else
            {
                Debug.Log("rb position: " + rb.position);
                Debug.Log("transform.position: " + transform.position);
            }
        }

    }

    IEnumerator DestroyDelay()
    {
        canDestroy = true;
        yield return new WaitForSecondsRealtime(2.0f);
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
            pController.targetSet.Remove(targetToRemove);
            GameObject.Destroy(targetToRemove);
        }
    }
}
