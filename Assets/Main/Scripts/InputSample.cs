using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//used to test CooldownTimer.cs
public class InputSample : MonoBehaviour {

    public KeyCode abilityButton;
    private CooldownTimer cd;

    void Start()
    {
        cd = GetComponent<CooldownTimer>();
    }

    // Update is called once per frame
    void Update () {
		
        if(!cd.isOnCooldown && Input.GetKeyDown(abilityButton))
        {
            print("Entering cooldown");
            cd.isOnCooldown = true;
        }
	}
}
