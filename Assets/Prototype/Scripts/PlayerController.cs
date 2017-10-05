using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField]
    private bool isTimeFrozen = false;

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

        isTimeFrozen = Time.timeScale == 0.0f;
        direction.x = Input.GetAxisRaw("Horizontal");
        direction.y = Input.GetAxisRaw("Vertical");
        velocity.Set(direction.x * speed, direction.y * speed);
        rb.velocity = velocity * Time.deltaTime;

        if(!slowdownActive && Input.GetKeyDown(slowdownKey))
        {
            slowdownActive = true;
            //use this instead of Time.time as Time.time is affected by Time.timescale
            startSlowdownTime = Time.realtimeSinceStartup;
            StartCoroutine(SlowdownTime());
        }

        if(isTimeFrozen)
        {
            //enable click controls here
        }

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
            yield return null;
            elapsedTime = Time.realtimeSinceStartup - startSlowdownTime;
        }

        Debug.Log("ELAPSED");
        yield return StartCoroutine(FreezeTime());
    }

    IEnumerator FreezeTime()
    {
        Debug.Log("Time frozen!");
        //change cursor to a crosshair
        Cursor.SetCursor(crosshair, Vector2.zero, CursorMode.ForceSoftware);

        yield return new WaitForSecondsRealtime(freezeDuration);
        yield return StartCoroutine(SpeedupTime());
    }

    //serve as a cooldown time for the player !
    IEnumerator SpeedupTime()
    {
        Debug.Log("Speeding UP");

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
        Debug.Log("Time.timeScale: " + Time.timeScale);
        //set time scale to 1 here where numerical error may not set this value exactly to 1
        Time.timeScale = originalTimeScale;
        Debug.Log("Time.timeScale1: " + Time.timeScale);
        yield return null;
    }
}
