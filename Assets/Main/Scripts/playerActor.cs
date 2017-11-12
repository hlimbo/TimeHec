using UnityEngine;
using UnityEngine.UI;

public class playerActor : MonoBehaviour {

    public TouchJoystickController leftJoystick;
    public TouchJoystickController rightJoystick;
    public GameObject shieldObject;
    public Text positionText;
    public float moveSpeed;

    //2^-7
    private float tolerance = 0.0078125f;

    private float tolerance2 = 0.00001f;

    void Start () {
  
    }
	
	// Update is called once per frame
	void Update () {

        transform.Translate(leftJoystick.inputDirection * moveSpeed * Time.deltaTime);
        positionText.text = transform.position.ToString();

        //position the shield relative to where the player vessel is located
        //half circle rotation
        //if cos and sin values are close enough to being zero... set them as zero values.
        float x = Mathf.Abs(Mathf.Cos(Mathf.PI / 2.0f + (rightJoystick.inputDirection.x * Mathf.PI / 2.0f))) > tolerance ? -1.5f * Mathf.Cos(Mathf.PI / 2.0f + (rightJoystick.inputDirection.x * Mathf.PI / 2.0f)) : 0.0f;
        float y = Mathf.Abs(Mathf.Sin(Mathf.PI / 2.0f + (rightJoystick.inputDirection.x * Mathf.PI))) > tolerance ? 1.5f * Mathf.Sin(Mathf.PI / 2.0f + (rightJoystick.inputDirection.x * Mathf.PI / 2.0f)) : 0.0f;
        Vector2 shieldPos = new Vector2(x,y);
        shieldObject.transform.position = transform.TransformPoint(shieldPos);
        float targetAngleInRadians = Mathf.Atan2(y, x) - Mathf.PI / 2.0f;
        Quaternion newQuat = Quaternion.Euler(0.0f, 0.0f, targetAngleInRadians * Mathf.Rad2Deg);
        shieldObject.transform.rotation = Quaternion.RotateTowards(shieldObject.transform.rotation, newQuat, 90.0f * Time.deltaTime);
        //shieldObject.transform.RotateAround(transform.position, Vector3.forward, 45.0f * Time.deltaTime);
    }
}
