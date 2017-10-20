using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownDisplay : MonoBehaviour {

    private Text gText;
    private CooldownTimer cd;

	void Start ()
    {
        gText = GetComponent<Text>();
        //lets not do this later down the road!
        cd = FindObjectOfType<CooldownTimer>();
	}
	
	void Update ()
    {
        gText.text = cd.GetCurrentCooldownTime().ToString();
	}
}
