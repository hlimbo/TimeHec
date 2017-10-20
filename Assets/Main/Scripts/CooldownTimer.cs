using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//used to count how long an ability can be cast again (measured in seconds)
//can be used for enemy or player ability cooldowns such as shield bubbble

//this component should be initially disabled and becomes activated
//by another script that requires its functionality.. e.g. player input component triggers the cooldown when shield effect activates with key press s.
public class CooldownTimer : MonoBehaviour {

    public float duration;
    public float updateFrequency = 1.0f;//how often the cooldown timer counts down by
    [SerializeField]
    private float startTime;
    [SerializeField]
    private float elapsedTime;

    //I'm using enabled flag because it is convenient to see when this component is on or off cooldown in the editor
    public bool isOnCooldown { get { return this.enabled; } set { this.enabled = value; } }
   
    public float GetCurrentCooldownTime()
    {
        return isOnCooldown ? duration - Mathf.Floor(elapsedTime) : 0.0f;
    }

	void Awake ()
    {
        //disable this script from calling Update() every frame
        isOnCooldown = false;
	}

    void OnEnable()
    {
        StartCoroutine(Countdown());
        print("starting");
        startTime = Time.time;
    }

    void OnDisable()
    {
        StopCoroutine(Countdown());
        print("stopping");
    }

    //alternate way of counting down if needed
    //void Update()
    //{
    //    elapsedTime = Time.time - startTime;
    //    if (elapsedTime < duration)
    //        WaitOneSecond();
    //    else
    //        isOnCooldown = false;
    //}

    //IEnumerator WaitOneSecond()
    //{
        
    //    yield return new WaitForSeconds(updateFrequency);
    //}

    //first way of doing it
    //advantage over the alternate way is that I can use either awake or start function to init variables
    //this only updates the elapsedTime once every updateFrequency value (e.g. once every second)
    IEnumerator Countdown()
    {
        startTime = Time.time;
        elapsedTime = Time.time - startTime;
        while(elapsedTime < duration)
        {
            yield return new WaitForSeconds(updateFrequency);
            elapsedTime = Time.time - startTime;
        }

        isOnCooldown = false;
        yield return null;
    }
}
