using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineDrawer : MonoBehaviour {

    private LineRenderer lr;
    private PlayerController pc;
    public List<Vector3> targetPositions;
    [SerializeField]
    int currentIndex = 0;
    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        pc = FindObjectOfType<PlayerController>();
        targetPositions = new List<Vector3>();
    }

    // Use this for initialization
    void Start () {
        lr.positionCount = 0;
	}
	
	// Update is called once per frame
	void Update () {

        if (pc.isTimeFrozen)
        {
            bool clicked = Input.GetMouseButtonDown(0);
            if (clicked)
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D something = Physics2D.Raycast(mousePos, Vector2.zero, 0f);
                if (something.collider != null)
                {
                    if (!pc.targetList.Contains(something.collider.gameObject))
                    {
                        lr.positionCount += 1;
                        mousePos.z = 0.0f;
                        lr.SetPosition(currentIndex++, mousePos);
                    }
                }
            }
        }
        else
        {
            lr.positionCount = 0;
            currentIndex = 0;
        }
	}
}
