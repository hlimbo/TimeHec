using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMover : MonoBehaviour {

    public float moveSpeed;
    public float patrolDist;
    [SerializeField]
    private float patrolTime;
    private int yDirection;
    private Rigidbody2D rb;
    private Vector2 velocity;

	// Use this for initialization
	void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        yDirection = -1;
        velocity = new Vector2(0.0f, 0.0f);
        Debug.Assert(moveSpeed != 0.0f);
        patrolTime = (1 / moveSpeed) * patrolDist;
        StartCoroutine(ChangeDirection());
	}
	
	// Update is called once per frame
	void Update ()
    {
        velocity.Set(0.0f, moveSpeed * yDirection);
        rb.velocity = velocity * Time.deltaTime;
	}

    IEnumerator ChangeDirection()
    {
        while(true)
        {
            yDirection = (yDirection == 1) ? -1 : 1;
            //real time seems to have not much effect here.. when the patrol units are slowed down.
            yield return new WaitForSecondsRealtime(patrolTime);
        }
    }
}
