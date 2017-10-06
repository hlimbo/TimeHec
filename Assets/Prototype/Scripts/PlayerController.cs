using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {

    public float speed;
    private Vector2 direction;
    private Vector2 velocity;
    private Rigidbody2D rb;

    public KeyCode slowdownKey;
    [SerializeField]
    private bool slowdownActive = false;

    [SerializeField]
    private float startSlowdownTime;
    [SerializeField]
    private float abilityDuration;
    public float slowdownDuration = 2.0f;
    public float freezeDuration = 1.0f;
    public float speedupDuration = 2.0f;

    [SerializeField]
    private float originalTimeScale;
    public Texture2D crosshair;
    public bool isTimeFrozen = false;

    //arrow controller needs know its targets to seek to
    public List<GameObject> targetList;//used to see in the inspector which targets were selected by the player

    [SerializeField]
    private bool hasDestructionStarted = false;

    public GameObject arrowPrefab;
    public GameObject arrowGO;

    private void Awake()
    {
        targetList = new List<GameObject>();
    }

    // Use this for initialization
    void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        direction = new Vector2(0.0f, 0.0f);
        velocity = new Vector2(0.0f, 0.0f);
        abilityDuration = slowdownDuration + freezeDuration;
        originalTimeScale = Time.timeScale;
	}
	
	// Update is called once per frame
	void Update () {

        direction.x = Input.GetAxisRaw("Horizontal");
        direction.y = Input.GetAxisRaw("Vertical");
        velocity.Set(direction.x * speed, direction.y * speed);
        //use this temporarily to demonstrate that the player can in fact move at full speed
        //independent of everyone else being slowed down
        transform.position += (Vector3)velocity * Time.unscaledDeltaTime;
        //rigidbodies get updated internally in a physics update.. therefore, we cannot
        //use rigidbodies for collision detection if we want to have player not be affected
        //by time scale...unless we create our own custom "environment scale" for all gamebojects
        //but the player to get affected by
        //rb.velocity = velocity * Time.unscaledDeltaTime;

        if(!slowdownActive && Input.GetKeyDown(slowdownKey))
        {
            //spawn arrow here
            arrowGO = Instantiate<GameObject>(arrowPrefab);

            slowdownActive = true;
            //use this instead of Time.time as Time.time is affected by Time.timescale
            startSlowdownTime = Time.realtimeSinceStartup;
            StartCoroutine(SlowdownTime());
        }

        if(isTimeFrozen)
        {
            bool clicked = Input.GetMouseButtonDown(0);
            if(clicked)
            {
               // Debug.Log("I've been clicked at pixel coordinates: " + Input.mousePosition);
                //note: viewport coordinates are normalized screen coordinates where
                // the bottom left corner is measured as (0,0) and the top right corner is measured as (1,1)
                /*
                 *       -------------- (1,1)
                 *       |            | 
                 *       |            |
                 *       |            |
                 *       --------------
                 *      (0,0)
                 */
              //  Debug.Log("viewport coordinates: " + Camera.main.ScreenToViewportPoint(Input.mousePosition));
              //  Debug.Log("world coordinates: " + Camera.main.ScreenToWorldPoint(Input.mousePosition)); // measured in unity units

                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D something = Physics2D.Raycast(mousePos, Vector2.zero, 0f);
                if(something.collider != null)
                {
                    Debug.Log(something.collider.gameObject.name);
                    if (!targetList.Contains(something.collider.gameObject))
                        targetList.Add(something.collider.gameObject);
                }
            }
        }

        //will need to implement a state enum.....for polish possibly
        //if (!hasDestructionStarted && Time.timeScale == originalTimeScale)
        //{
        //    hasDestructionStarted = true;
        //    StartCoroutine(DestroyTargets());
        //}

    }

    IEnumerator DestroyTargets()
    {
        //destroy all targets every 2 seconds
        while(targetList.Count != 0)
        {
            GameObject targetToRemove = targetList[0];
            targetList.RemoveAt(0);
            yield return new WaitForSeconds(2.0f);
            Destroy(targetToRemove);
        }

        hasDestructionStarted = false;

        yield return null;
    }

    //See http://wiki.unity3d.com/index.php?title=Mathfx for easing in functions!
    IEnumerator SlowdownTime()
    {
        float ptol = 0.00390625f;//number is a perfect power of 2.
        float elapsedTime = Time.realtimeSinceStartup - startSlowdownTime;
        while (elapsedTime < slowdownDuration)
        {
            //here I want to find the rate required to decrease Time.scaletime to 0 at slowdownDuration (e.g. 2 seconds)
            //I want the timeScale to gradually go down until it reaches 0 ~ effect Time slowly slows down to a halt.
            float percent = elapsedTime / slowdownDuration;
            if (1.0f - percent < ptol) percent = 1.0f;
            Time.timeScale = Mathf.Lerp(originalTimeScale, 0.0f, percent);
            //have to set timescale to zero here in the event it never gets exactly set to zero due to numerical error
            if(Time.timeScale < ptol) Time.timeScale = 0.0f;
            yield return null;
            elapsedTime = Time.realtimeSinceStartup - startSlowdownTime;
        }

        //Debug.Log("ELAPSED");
        yield return StartCoroutine(FreezeTime());
    }

    IEnumerator FreezeTime()
    {
        isTimeFrozen = true;
        //Debug.Log("Time frozen!");
        //change cursor to a crosshair
        Cursor.SetCursor(crosshair, Vector2.zero, CursorMode.Auto);

        yield return new WaitForSecondsRealtime(freezeDuration);
        yield return StartCoroutine(SpeedupTime());
    }

    //serve as a cooldown time for the player !
    IEnumerator SpeedupTime()
    {
        isTimeFrozen = false;
        //Debug.Log("Speeding UP");

        //change cursor back to default cursor
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        float ptol = 0.00390625f;
        float startSpeedUpTime = Time.realtimeSinceStartup;
        float elapsedTime = Time.realtimeSinceStartup - startSpeedUpTime;
        while(elapsedTime < speedupDuration)
        {
            float percent = elapsedTime / speedupDuration;
            if (1.0f - percent < ptol) percent = 1.0f;
            Time.timeScale = Mathf.Lerp(0.0f, originalTimeScale, percent);
            yield return null;
            elapsedTime = Time.realtimeSinceStartup - startSpeedUpTime;
        }

        slowdownActive = false;
        //Debug.Log("Time.timeScale: " + Time.timeScale);
        //set time scale to 1 here where numerical error may not set this value exactly to 1
        Time.timeScale = originalTimeScale;
        //Debug.Log("Time.timeScale1: " + Time.timeScale);
        yield return null;
    }
}
