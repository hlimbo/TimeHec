using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//used for android debugging
public class MotionDebug : MonoBehaviour {

    private MotionController controller;
    public Text touchSpeed;
    public Text moveSpeed;
    public Text movePercent;
    public Slider percentSlider;

    [Range(0.0f,1.0f)]
    public float percent;

	// Use this for initialization
	void Awake () {
        controller = FindObjectOfType<MotionController>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        touchSpeed.text = controller.motionSpeed.ToString();
        moveSpeed.text = controller.moveVelocity.ToString();

        percent = percentSlider.value;
        movePercent.text = percent.ToString();
	}
}
