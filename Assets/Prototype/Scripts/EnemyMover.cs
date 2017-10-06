using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMover : MonoBehaviour {

    public float moveSpeed;
    public float patrolDist;
    [SerializeField]
    private float patrolTime;
    [SerializeField]
    private int yDirection;
    private Rigidbody2D rb;
    private Vector2 velocity;

    [SerializeField]
    private Vector2 originalPos;

    private float startPatrolTime;

	// Use this for initialization
	void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        originalPos = new Vector2(rb.position.x, rb.position.y);
        velocity = new Vector2(0.0f, 0.0f);
        Debug.Assert(moveSpeed != 0.0f);
        yDirection = 1;
        patrolTime = (1 / moveSpeed) * patrolDist;
        startPatrolTime = Time.time;
	}
	
	// Update is called once per frame
	void Update ()
    {
        velocity.Set(0.0f, moveSpeed * yDirection);
        rb.velocity = velocity * Time.deltaTime;

        if(Time.timeScale != 0.0f)
        {
            if(Time.time - startPatrolTime > patrolTime)
            {
                float distError = Mathf.Abs(rb.position.y - originalPos.y) / patrolDist;
                //Debug.Log("distance error: " + distError);
                startPatrolTime = Time.time;
                yDirection = -yDirection;
            }
        }

	}

}
