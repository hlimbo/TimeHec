using UnityEngine;
using UnityEngine.UI;

public class playerActor : MonoBehaviour {

    public TouchJoystickController leftJoystick;
    public TouchJoystickController rightJoystick;
    public Transform shieldPivot;
    public Text positionText;
    public float moveSpeed;
    //2^-7
    private float tolerance = 0.0078125f;
    public float angularSpeed = 90.0f;
    public float rotationRadius = 1.5f;

	// Update is called once per frame
	void Update () {

        //have the analog sticks point to the direction the shield should be facing!
        //I needed to swap the x and y components for this to work!
        float targetAngleInRadians = Mathf.Atan2(-1.0f * rightJoystick.inputDirection.x, rightJoystick.inputDirection.y);
        Quaternion targetRotation = Quaternion.Euler(0.0f, 0.0f, targetAngleInRadians * Mathf.Rad2Deg);
        shieldPivot.rotation = Quaternion.RotateTowards(shieldPivot.rotation, targetRotation, angularSpeed * Time.deltaTime);

        transform.Translate(leftJoystick.inputDirection * moveSpeed * Time.deltaTime);
        positionText.text = transform.position.ToString();
        shieldPivot.transform.position = this.transform.position;
    }
}
