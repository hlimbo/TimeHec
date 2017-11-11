using UnityEngine;
using UnityEngine.UI;

public class playerActor : MonoBehaviour {

    public TouchJoystickController leftJoystick;
    public TouchJoystickController rightJoystick;
    public GameObject shieldObject;
    public Text positionText;
    public float moveSpeed;

    void Start () {
  
    }
	
	// Update is called once per frame
	void Update () {

        transform.Translate(leftJoystick.inputDirection * moveSpeed * Time.deltaTime);
        positionText.text = transform.position.ToString();

        //position the shield relative to where the player vessel is located
        Vector2 shieldPos = new Vector2(Mathf.Cos(rightJoystick.inputDirection.x - Mathf.PI / 2.0f), Mathf.Sin(rightJoystick.inputDirection.y + Mathf.PI / 2));
        shieldObject.transform.position = transform.TransformPoint(shieldPos);
    }
}
