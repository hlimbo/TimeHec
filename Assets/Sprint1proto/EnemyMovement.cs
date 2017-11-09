using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {
    public Transform[] Waypoints;
    public float speed;
    public int ourWaypoint;
    public bool Patrol = true;
    public Vector2 Target;
    public Vector2 MoveDirection;
    public Vector2 Velocity;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
    void Update ()
    {
	    if(ourWaypoint < Waypoints.Length)
        {
            Target = Waypoints[ourWaypoint].position;
            Vector2 position = transform.position;
            MoveDirection = Target - position;
            Velocity = GetComponent<Rigidbody2D>().velocity;
            if (MoveDirection.magnitude < 1)
                ourWaypoint++;
            else
                Velocity = MoveDirection.normalized * speed;
        }
        else
        {
            if (Patrol)
                ourWaypoint = 0;
            else
                Velocity = Vector2.zero;
        }
        GetComponent<Rigidbody2D>().velocity = Velocity;
        {
           transform.Rotate(new Vector3(0, 0, 300) * Time.deltaTime);
        }
	}
}
