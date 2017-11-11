using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Used to see more information about the Touch class
//Goal: I want to see how many fingers can be pressed at once on my Nexus6
public class InfoTouch : MonoBehaviour {

    void OnGUI()
    {
        foreach(Touch touch in Input.touches)
        {
            string message = "fingerId: " + touch.fingerId + "\n";
            message += "altitudeAngle: " + touch.altitudeAngle + "\n";
            message += "azimuthAngle: " + touch.azimuthAngle + "\n";
            message += "deltaPosition: " + touch.deltaPosition + "\n";
            message += "phase: " + touch.phase.ToString() + "\n";
            message += "type: " + touch.type.ToString() + "\n";
            message += "position: " + touch.position + "\n";
            message += "raw position: " + touch.rawPosition + "\n";
            message += "radius: " + touch.radius + "\n";
            message += "radius Variance: " + touch.radiusVariance + "\n";
            message += "tapCount: " + touch.tapCount + "\n";

            //roughly 3 touch messages will fit on a nexus 6 using this config
            int num = touch.fingerId;
            GUI.Label(new Rect(0 + 130 * num, 0, 120, 300), message);

        }

        Debug.Log("Touch count: " + Input.touchCount);
    }
}
