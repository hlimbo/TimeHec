using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class TouchJoystickController : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler {

    private Image joystickOuter;
    private Image joystickNub;
    public Vector2 inputDirection;

    void Start ()
    {
        joystickOuter = GetComponent<Image>();
        joystickNub = transform.GetChild(0).GetComponent<Image>();
        inputDirection = Vector2.zero;
    }
	
    public void OnDrag(PointerEventData eventData)
    {
        //since the local space of joystickOuter.rectTransform is the canvas
        ///this just returns us the pixel coordinates of the screen when ondrag is invoked
        Vector2 localPoint;
        //gives us the input direction
        RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickOuter.rectTransform, eventData.position, eventData.pressEventCamera, out localPoint);

        //rectTransform.sizeDelta corresponds to the Width and Height of the rectTransform component
        //here we want to convert localPoints here relative to far away or close 
        //the localPoints are from the joystickOuter's rectTransform as a percentage
        localPoint.x = localPoint.x / joystickOuter.rectTransform.sizeDelta.x;
        localPoint.y = localPoint.y / joystickOuter.rectTransform.sizeDelta.y;

        //Debug.Log("localPoint: " + localPoint);
       // Debug.Log("rectTransform.sizeDelta: " + joystickOuter.rectTransform.sizeDelta);

        //unsure why multiplying by 2 and adding 1 here..this probably makes sure that the joystickNub is right under your finger
        //when moving it around.. this might depend on which side of the screen you put the joystick control at (left or right side?)
        float x = (joystickOuter.rectTransform.pivot.x == 1f) ? localPoint.x * 2 + 1 : localPoint.x * 2 - 1;
        float y = (joystickOuter.rectTransform.pivot.y == 1f) ? localPoint.y * 2 + 1 : localPoint.y * 2 - 1;

        //float y = localPoint.y * 2 - 1;

        //Debug.Log("x: " + x);
        //Debug.Log("y: " + y);

        inputDirection = new Vector2(x, y);
        //if the pointer position is outside of the joystickOuter's bounds...normalize the inputDirection
        inputDirection = (inputDirection.magnitude > 1) ? inputDirection.normalized : inputDirection;

        //define the area in which the joystick can move around
        joystickNub.rectTransform.anchoredPosition = new Vector2(inputDirection.x * (joystickOuter.rectTransform.sizeDelta.x/3), inputDirection.y * (joystickOuter.rectTransform.sizeDelta.y/3));
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    //resets the inputDirection of the joystick if finger is not on it
    public void OnPointerUp(PointerEventData eventData)
    {
        inputDirection = Vector3.zero;
        //resets the position of the nub on the center of the joystickOuter's rectTransform area
        joystickNub.rectTransform.anchoredPosition = Vector3.zero;
    }


}
